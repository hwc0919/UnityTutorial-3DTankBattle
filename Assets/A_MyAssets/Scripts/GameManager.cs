using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyTank
{
    public class GameManager : MonoBehaviour
    {
        public GameObject m_TankPrefab;
        public TankManager[] m_Tanks;

        void Awake()
        {
            for (int i = 0; i < m_Tanks.Length; ++i)
            {
                var spawnPoint = m_Tanks[i].m_SpawnPoint;
                m_Tanks[i].m_TankInstance = Instantiate(m_TankPrefab, spawnPoint.position, spawnPoint.rotation);
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].Setup();
            }
        }

        void Start()
        {
        }
    }
}
