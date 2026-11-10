
using System.Collections;
using UnityEngine;

public class PaddleColorFeedback : MonoBehaviour
{
    [Header("Paddle Visual")]
    [SerializeField] private SpriteRenderer paddleRenderer;

    [Header("Flash Settings")]
    [SerializeField] private float flashDuration = 0.15f;

    [SerializeField] private Color[] flashColors = new Color[]
    {
        new Color(1f, 0.86f, 0.33f),  // Yellow
        new Color(1f, 0.46f, 0.72f),  // Pink
        new Color(0.71f, 0.54f, 1f),  // Purple
        new Color(0.31f, 0.80f, 0.77f), // Cyan
        new Color(1f, 0.67f, 0.33f)   // Orange
    };

    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (paddleRenderer != null)
        {
            originalColor = paddleRenderer.color;
        }
    }

    private void OnEnable()
    {
        GameEvents.BallHitPaddle += OnBallHitPaddle;
    }

    private void OnDisable()
    {
        GameEvents.BallHitPaddle -= OnBallHitPaddle;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (paddleRenderer != null)
        {
            paddleRenderer.color = originalColor;
        }
    }

    private void OnBallHitPaddle(ImpactInfo info)
    {
        if (paddleRenderer == null ||
            flashColors == null ||
            flashColors.Length == 0)
            return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashColor());
    }

    private IEnumerator FlashColor()
    {
        // Pick a random color from the palette
        Color selectedColor =
            flashColors[Random.Range(0, flashColors.Length)];

        paddleRenderer.color = selectedColor;

        float timer = 0f;

        // Fade back to the original paddle color
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / flashDuration);

            paddleRenderer.color =
                Color.Lerp(selectedColor, originalColor, t);

            yield return null;
        }

        paddleRenderer.color = originalColor;
        flashCoroutine = null;
    }
}
