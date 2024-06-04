using System;
using UnityEngine;

namespace MyTank
{
    public class ShellExplosion : MonoBehaviour
    {
        public LayerMask m_TankLayerMask;
        public ParticleSystem m_ExplosionParticles;
        public AudioSource m_ExplosionAudio;
        public float m_MaxDamage = 100f;
        public float m_ExplosionForce = 1000f;
        public float m_MaxLifeTime = 2f;
        public float m_ExplosionRadius = 5f;

        private void Start()
        {
            Destroy(gameObject, m_MaxLifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankLayerMask);
            for (int i = 0; i < colliders.Length; ++i)
            {
                Rigidbody targetRb = colliders[i].GetComponent<Rigidbody>();
                if (!targetRb)
                    continue;
                targetRb.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

                TankHealth tankHealth = targetRb.GetComponent<TankHealth>();
                if (!tankHealth)
                    continue;
                tankHealth.TakeDamage(CalculateDamage(targetRb.position));
            }

            m_ExplosionParticles.transform.parent = null;
            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
            Destroy(gameObject);
        }

        private float CalculateDamage(Vector3 targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            if (diff.magnitude >= m_ExplosionRadius)
            {
                return 0;
            }
            return m_MaxDamage * (m_ExplosionRadius - diff.magnitude) / m_ExplosionRadius;
        }
    }
}
