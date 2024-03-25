using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleCollector : MonoBehaviour
{
    public UnityEvent OnParticleCollect;
    private ParticleSystem _particleSystem;
    List<ParticleSystem.Particle> _particles = new List<ParticleSystem.Particle>();


    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        int triggeredParticles = _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            Debug.Log("particleshahaha");
            ParticleSystem.Particle particle = _particles[i];
            particle.remainingLifetime = 0;
            _particles[i] = particle;
            OnParticleCollect.Invoke();
        }

        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _particles);
    }
}
