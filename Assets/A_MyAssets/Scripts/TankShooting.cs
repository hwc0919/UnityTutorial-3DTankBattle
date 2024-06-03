using UnityEngine;
using UnityEngine.UI;

namespace MyTank
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;
        public Rigidbody m_Shell;
        public Transform m_FireTransform;
        public Slider m_AimSlider;
        public AudioSource m_shootingAudio;
        public AudioClip m_ChargingClip;
        public AudioClip m_FireClip;

        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.

        private string m_FireButton;
        private float m_CurrentLaunchForce;
        private float m_ChargeSpeed;
        private bool m_Fired = false;

        private void OnEnable()
        {
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
        }

        // Start is called before the first frame update
        private void Start()
        {
            m_FireButton = "Fire" + m_PlayerNumber;
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }

        // Update is called once per frame
        private void Update()
        {
            m_AimSlider.value = m_MinLaunchForce;

            // current force excced max force, fire now
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            // fire button just pressed
            else if (Input.GetButtonDown(m_FireButton))
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                // audio
                m_shootingAudio.clip = m_ChargingClip;
                m_shootingAudio.Play();
            }
            // fire button being held
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            else if (Input.GetButtonUp(m_FireButton) && !m_Fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            m_Fired = true;
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
            Debug.Log("gameObject: " + shellInstance.gameObject.ToString());
            m_CurrentLaunchForce = m_MinLaunchForce;

            // audio
            m_shootingAudio.clip = m_FireClip;
            m_shootingAudio.Play();
        }
    }
}
