using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MyTank
{
    public class GameManager : MonoBehaviour
    {
        public float m_StartDelay = 3f;
        public float m_EndDelay = 3f;

        public CameraControl m_CameraControl;

        public GameObject m_TankPrefab;
        public TankManager[] m_Tanks;
        public int m_PlayerNum = 2;

        private WaitForSeconds m_StartWait;
        private WaitForSeconds m_EndWait;

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

            SceneManager.LoadScene(0);
        }

        private IEnumerator RoundStarting()
        {
            yield return new WaitForSeconds(2);
        }

        IEnumerator RoundPlaying()
        {
            while (!OneTankLeft())
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator RoundEnding()
        {
            yield return new WaitForSeconds(1);
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
    }
}
