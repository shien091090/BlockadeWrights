using System;
using UnityEngine;

namespace GameCore
{
    public class ParticleFollowEffect : MonoBehaviour
    {
        [SerializeField] private float speed;

        private IAttackTarget target;
        private ParticleSystem particleSystem;
        private ParticleSystem.Particle[] particles;
        private Action callback;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        }

        private void Update()
        {
            if (target == null)
                return;

            int numParticles = particleSystem.GetParticles(particles);

            bool isHit = false;
            for (int i = 0; i < numParticles; i++)
            {
                Vector3 directionToTarget = ((Vector3)target.GetTransform.Position - particles[i].position).normalized;
                float distance = Vector3.Distance((Vector3)target.GetTransform.Position, particles[i].position);
                Debug.Log($"distance: {distance}");
                if (distance <= 0.01f)
                    isHit = true;

                particles[i].velocity = directionToTarget * speed;
            }

            particleSystem.SetParticles(particles, numParticles);

            if (isHit)
            {
                target = null;
                callback?.Invoke();
                callback = null;
                gameObject.SetActive(false);
            }
        }

        public void StartFollow(IAttackTarget followTarget, Action onCompletedCallback)
        {
            target = followTarget;
            callback = onCompletedCallback;
        }
    }
}