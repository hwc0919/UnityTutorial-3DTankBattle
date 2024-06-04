using UnityEngine;
using UnityEngine.UI;

namespace MyTank
{
    public class TankHealth : MonoBehaviour
    {
        public float m_StartingHealth = 100f;
        public Slider m_Slider;
        public Image m_FillImage;
        public Color m_FullHealthColor = Color.green;
        public Color m_ZeroHealthColor = Color.red;
        public GameObject m_ExplosionPrefab;

        private GameObject m_ExplosionInstance;
        private ParticleSystem m_ExplosionParticles;
        private AudioSource m_ExplosionAudio;
        private float m_CurrentHealth;
        private bool m_Dead;

        private void Awake()
        {
            m_ExplosionInstance = Instantiate(m_ExplosionPrefab);
            m_ExplosionInstance.SetActive(false);
            m_ExplosionAudio = m_ExplosionInstance.GetComponent<AudioSource>();
            m_ExplosionParticles = m_ExplosionInstance.GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
            SetHealthUI();
        }

        public void TakeDamage(float amount)
        {
            m_CurrentHealth -= amount;
            SetHealthUI();

            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath();
            }
        }

        private void OnDeath()
        {
            m_Dead = true;

            m_ExplosionInstance.transform.position = transform.position;
            m_ExplosionInstance.SetActive(true);

            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            gameObject.SetActive(false);
        }

        private void SetHealthUI()
        {
            m_Slider.value = m_CurrentHealth;
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }
    }
}
