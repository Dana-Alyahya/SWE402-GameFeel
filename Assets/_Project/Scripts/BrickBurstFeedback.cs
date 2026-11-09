
using UnityEngine;

public class BrickBurstFeedback : MonoBehaviour
{
    [SerializeField] private ParticleSystem brickBurstPrefab;

    private void OnEnable()
    {
        GameEvents.BrickDestroyed += OnBrickDestroyed;
    }

    private void OnDisable()
    {
        GameEvents.BrickDestroyed -= OnBrickDestroyed;
    }

    private void OnBrickDestroyed(ImpactInfo info)
    {
        if (brickBurstPrefab == null)
        {
            Debug.LogWarning("BrickBurst prefab is not assigned!");
            return;
        }

        // Spawn particles at the center of the destroyed brick
        ParticleSystem burst = Instantiate(
            brickBurstPrefab,
            info.Center,
            Quaternion.identity
        );

        // Play the particle effect
        burst.Play();

        // Calculate how long the effect lasts
        float lifetime = burst.main.duration
                       + burst.main.startLifetime.constantMax;

        // Destroy the particles after the effect finishes
        Destroy(burst.gameObject, lifetime + 0.5f);
    }
}
