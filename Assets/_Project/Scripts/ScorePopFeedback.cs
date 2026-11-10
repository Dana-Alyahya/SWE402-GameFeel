
using System.Collections;
using UnityEngine;
using TMPro;

public class ScorePopFeedback : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private RectTransform scoreText;

    [Header("Pop Settings")]
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popDuration = 0.1f;
    [SerializeField] private float recoveryDuration = 0.2f;

    [Header("Color Flash")]
    [SerializeField] private Color flashColor =
        new Color(1f, 0.75f, 0.2f, 1f);

    private Vector3 originalScale;
    private Color originalColor;
    private TMP_Text scoreTMP;
    private Coroutine popCoroutine;

    private void Awake()
    {
        if (scoreText != null)
        {
            originalScale = scoreText.localScale;

            scoreTMP = scoreText.GetComponent<TMP_Text>();

            if (scoreTMP != null)
            {
                originalColor = scoreTMP.color;
            }
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
            scoreText.localScale = originalScale;

        if (scoreTMP != null)
            scoreTMP.color = originalColor;
    }

    private void OnScoreChanged(int newScore)
    {
        if (scoreText == null)
            return;

        if (popCoroutine != null)
            StopCoroutine(popCoroutine);

        popCoroutine = StartCoroutine(PopAnimation());
    }

    private IEnumerator PopAnimation()
    {
        Vector3 enlargedScale = originalScale * popScale;
        Vector3 startScale = scoreText.localScale;

        Color startColor = scoreTMP != null
            ? scoreTMP.color
            : Color.white;

        float timer = 0f;

        // Enlarge the score and flash golden yellow
        while (timer < popDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / popDuration);

            scoreText.localScale =
                Vector3.Lerp(startScale, enlargedScale, t);

            if (scoreTMP != null)
                scoreTMP.color =
                    Color.Lerp(startColor, flashColor, t);

            yield return null;
        }

        timer = 0f;

        // Return smoothly to original size and color
        while (timer < recoveryDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / recoveryDuration);

            t = 1f - Mathf.Pow(1f - t, 3f);

            scoreText.localScale =
                Vector3.Lerp(enlargedScale, originalScale, t);

            if (scoreTMP != null)
                scoreTMP.color =
                    Color.Lerp(flashColor, originalColor, t);

            yield return null;
        }

        scoreText.localScale = originalScale;

        if (scoreTMP != null)
            scoreTMP.color = originalColor;

        popCoroutine = null;
    }
}
