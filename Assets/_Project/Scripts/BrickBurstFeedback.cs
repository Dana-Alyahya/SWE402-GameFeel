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

        ParticleSystem burst = Instantiate(
            brickBurstPrefab,
            info.Center,
            Quaternion.identity
        );

        // Tint the particles using the destroyed brick's color
        var main = burst.main;
        main.startColor = info.Color;

        burst.Play();

        float lifetime = burst.main.duration
                       + burst.main.startLifetime.constantMax;

        Destroy(burst.gameObject, lifetime + 0.5f);
    }
}