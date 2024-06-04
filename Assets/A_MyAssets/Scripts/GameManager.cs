using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyTank
{
    public class GameManager : MonoBehaviour
    {
        public float m_StartDelay = 3f;
        public float m_EndDelay = 3f;

        public CameraControl m_CameraControl;

        public GameObject m_TankPrefab;
        public TankManager[] m_Tanks;

        private WaitForSeconds m_StartWait;
        private WaitForSeconds m_EndWait;

        void Start()
        {
            m_StartWait = new WaitForSeconds(m_StartDelay);
            m_EndWait = new WaitForSeconds(m_EndDelay);

            SpawnAllTanks();
            SetCameraTargets();
        }

        void SpawnAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; ++i)
            {
                var spawnPoint = m_Tanks[i].m_SpawnPoint;
                m_Tanks[i].m_TankInstance = Instantiate(m_TankPrefab, spawnPoint.position, spawnPoint.rotation);
                m_Tanks[i].m_TankInstance.name = "Tank " + (i + 1).ToString();
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].Setup();
            }
        }

        void SetCameraTargets()
        {
            Transform[] targets = new Transform[m_Tanks.Length];
            for (int i = 0; i < targets.Length; ++i)
            {
                targets[i] = m_Tanks[i].m_TankInstance.transform;
            }

            m_CameraControl.m_Targets = targets;
        }
    }
}
