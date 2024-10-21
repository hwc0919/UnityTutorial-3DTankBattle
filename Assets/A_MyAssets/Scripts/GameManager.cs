using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace MyTank
{
    public class GameManager : MonoBehaviour
    {
        public float m_StartDelay = 3f;
        public float m_EndDelay = 3f;

        public CameraControl m_CameraControl;
        public Text m_MessageText;
        public GameObject m_TankPrefab;
        public TankManager[] m_Tanks;
        public int m_PlayerNum = 2;

        private WaitForSeconds m_StartWait;
        private WaitForSeconds m_EndWait;

        private int m_RoundNumber = 0;
        private TankManager m_RoundWinner;

        void Start()
        {
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);

            SpawnAllTanks();
            SetCameraTargets();

            StartCoroutine(GameLoop());
        }

        void SpawnAllTanks()
        {
            m_PlayerNum = Math.Min(m_PlayerNum, m_Tanks.Length);
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                var tank = m_Tanks[i];
                var spawnPoint = tank.m_SpawnPoint;
                tank.m_TankInstance = Instantiate(m_TankPrefab, spawnPoint.position, spawnPoint.rotation);
                tank.m_TankInstance.name = "Tank " + (i + 1).ToString();
                tank.m_PlayerNumber = i + 1;
                tank.Setup();
            }
        }

        void SetCameraTargets()
        {
            Transform[] targets = new Transform[m_PlayerNum];
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                targets[i] = m_Tanks[i].m_TankInstance.transform;
            }

            m_CameraControl.m_Targets = targets;
        }

        IEnumerator GameLoop()
        {
            Debug.Log("Starting");
            yield return StartCoroutine(RoundStarting());
            Debug.Log("Playing");
            yield return StartCoroutine(RoundPlaying());
            Debug.Log("Ending");
            yield return StartCoroutine(RoundEnding());

            StartCoroutine(GameLoop());
        }

        private IEnumerator RoundStarting()
        {
            ResetAllTanks();
            DisableTankControl();

            m_CameraControl.SetStartPositionAndSize();

            ++m_RoundNumber;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            yield return m_StartWait;
        }

        IEnumerator RoundPlaying()
        {
            EnableTankControl();
            m_MessageText.text = string.Empty;

            while (!OneTankLeft())
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator RoundEnding()
        {
            DisableTankControl();
            m_RoundWinner = null;
            m_RoundWinner = GetRoundWinner();
            if (m_RoundWinner != null)
            {
                m_RoundWinner.m_Wins++;
            }

            m_MessageText.text = GetEndMessage();

            yield return m_EndWait;
        }

        bool OneTankLeft()
        {
            int numTanks = 0;
            for (var i = 0; i < m_PlayerNum; ++i)
            {
                if (m_Tanks[i].m_TankInstance.activeSelf)
                    ++numTanks;
            }
            return numTanks <= 1;
        }

        private TankManager GetRoundWinner()
        {
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                if (m_Tanks[i].m_TankInstance.activeSelf)
                {
                    return m_Tanks[i];
                }
            }
            return null;
        }
        private string GetEndMessage()
        {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            // If there is a winner then change the message to reflect that.
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // Add some line breaks after the initial message.
            message += "\n\n\n\n";

            // Go through all the tanks and add each of their scores to the message.
            for (int i = 0; i < m_PlayerNum; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
            }

            // If there is a game winner, change the entire message to reflect that.
            //if (m_GameWinner != null)
            //    message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

            return message;
        }

        private void ResetAllTanks()
        {
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                m_Tanks[i].Reset();
            }
        }

        private void EnableTankControl()
        {
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                m_Tanks[i].EnableControl();
            }
        }
        private void DisableTankControl()
        {
            for (int i = 0; i < m_PlayerNum; ++i)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}
