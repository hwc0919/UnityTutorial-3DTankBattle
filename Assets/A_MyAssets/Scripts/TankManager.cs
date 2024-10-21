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
        internal string m_ColoredPlayerText;
        internal GameObject m_TankInstance;
        internal int m_Wins; // Should be saved in GameManager.

        private TankMovement m_Movement;
        private TankShooting m_Shooting;

        public void Setup()
        {
            m_Movement = m_TankInstance.GetComponent<TankMovement>();
            m_Shooting = m_TankInstance.GetComponent<TankShooting>();

            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;

            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

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

        public void DisableControl()
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;
        }

        public void EnableControl()
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;
        }

        public void Reset()
        {
            m_TankInstance.transform.position = m_SpawnPoint.position;
            m_TankInstance.transform.rotation = m_SpawnPoint.rotation;

            m_TankInstance.SetActive(false);
            m_TankInstance.SetActive(true);
        }
    }
}
