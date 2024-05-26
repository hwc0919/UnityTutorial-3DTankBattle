using System.Collections;
using UnityEngine;

namespace MyTank
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;
        public float m_Speed = 12f;
        public float m_TurnSpeed = 180f;
        public AudioSource m_MovementAudio;
        public AudioClip m_EngineIdling;
        public AudioClip m_EngineDriving;
        public float m_PitchRange = 0.2f;

        private Rigidbody m_Rigidbody;
        private string m_MovementAxisName;
        private string m_TurnAxisName;
        private float m_MovementInputValue;
        private float m_TurnInputValue;
        private float m_OriginalPitch;
        private ParticleSystem[] m_particleSystems;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }

        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            m_OriginalPitch = m_MovementAudio.pitch;
        }

        // Update is called once per frame
        private void Update()
        {
            m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

            EngineAudio();
        }

        private void FixedUpdate()
        {
            Move();
            Turn();
        }

        private void EngineAudio()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        private void Move()
        {
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        private void Turn()
        {
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }
}