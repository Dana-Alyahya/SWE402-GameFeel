
using System.Collections;
using UnityEngine;

public class PaddleSquashFeedback : MonoBehaviour
{
    [Header("Paddle Visual")]
    [SerializeField] private Transform paddleVisual;

    [Header("Squash and Stretch Settings")]
    [SerializeField] private float squashAmount = 0.25f;
    [SerializeField] private float squashDuration = 0.08f;
    [SerializeField] private float recoveryDuration = 0.15f;

    private Vector3 originalScale;
    private Coroutine squashCoroutine;

    private void Awake()
    {
        if (paddleVisual != null)
        {
            originalScale = paddleVisual.localScale;
        }
    }

    private void OnEnable()
    {
        GameEvents.BallHitPaddle += OnBallHitPaddle;
    }

    private void OnDisable()
    {
        GameEvents.BallHitPaddle -= OnBallHitPaddle;

        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
            squashCoroutine = null;
        }

        if (paddleVisual != null)
        {
            paddleVisual.localScale = originalScale;
        }
    }

    private void OnBallHitPaddle(ImpactInfo info)
    {
        if (paddleVisual == null)
            return;

        if (squashCoroutine != null)
        {
            StopCoroutine(squashCoroutine);
        }

        squashCoroutine = StartCoroutine(SquashAndStretch());
    }

    private IEnumerator SquashAndStretch()
    {
        Vector3 squashScale = new Vector3(
            originalScale.x * (1f + squashAmount),
            originalScale.y * (1f - squashAmount),
            originalScale.z
        );

        float timer = 0f;
        Vector3 startScale = paddleVisual.localScale;

        // Squash when the ball hits the paddle
        while (timer < squashDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / squashDuration);

            paddleVisual.localScale =
                Vector3.Lerp(startScale, squashScale, t);

            yield return null;
        }

        timer = 0f;

        // Stretch back to the original shape
        while (timer < recoveryDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / recoveryDuration);

            // Smooth spring-like recovery
            t = 1f - Mathf.Pow(1f - t, 3f);

            paddleVisual.localScale =
                Vector3.Lerp(squashScale, originalScale, t);

            yield return null;
        }

        paddleVisual.localScale = originalScale;
        squashCoroutine = null;
    }
}
