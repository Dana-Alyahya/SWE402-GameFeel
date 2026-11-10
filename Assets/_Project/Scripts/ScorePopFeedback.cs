
using System.Collections;
using UnityEngine;

public class ScorePopFeedback : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private RectTransform scoreText;

    [Header("Pop Settings")]
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popDuration = 0.1f;
    [SerializeField] private float recoveryDuration = 0.2f;

    private Vector3 originalScale;
    private Coroutine popCoroutine;

    private void Awake()
    {
        if (scoreText != null)
        {
            originalScale = scoreText.localScale;
        }
    }

    private void OnEnable()
    {
        GameEvents.ScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        GameEvents.ScoreChanged -= OnScoreChanged;

        if (popCoroutine != null)
        {
            StopCoroutine(popCoroutine);
            popCoroutine = null;
        }

        if (scoreText != null)
        {
            scoreText.localScale = originalScale;
        }
    }

    private void OnScoreChanged(int newScore)
    {
        if (scoreText == null)
            return;

        if (popCoroutine != null)
        {
            StopCoroutine(popCoroutine);
        }

        popCoroutine = StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        Vector3 enlargedScale = originalScale * popScale;
        Vector3 startScale = scoreText.localScale;

        float timer = 0f;

        // Enlarge the score
        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / popDuration);

            scoreText.localScale =
                Vector3.Lerp(startScale, enlargedScale, t);

            yield return null;
        }

        timer = 0f;

        // Smoothly return to normal size
        while (timer < recoveryDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / recoveryDuration);

            t = 1f - Mathf.Pow(1f - t, 3f);

            scoreText.localScale =
                Vector3.Lerp(enlargedScale, originalScale, t);

            yield return null;
        }

        scoreText.localScale = originalScale;
        popCoroutine = null;
    }
}
