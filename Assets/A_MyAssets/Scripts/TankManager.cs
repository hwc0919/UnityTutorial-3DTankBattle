using System;
using UnityEngine;

namespace MyTank
{
    [Serializable]
    public class TankManager
    {
        public Color m_PlayerColor;
        public Transform m_SpawnPoint;

        internal int m_PlayerNumber;
        internal GameObject m_TankInstance;

        private TankMovement m_Movement;
        private TankShooting m_Shooting;

        public void Setup()
        {
            m_Movement = m_TankInstance.GetComponent<TankMovement>();
            m_Shooting = m_TankInstance.GetComponent<TankShooting>();

            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;

            // Change tank color
            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_TankInstance.GetComponentsInChildren<MeshRenderer>();
            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }
        }
    }
}
