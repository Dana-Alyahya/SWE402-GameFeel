
using System.Collections;
using UnityEngine;
using TMPro;

public class LifeLossFeedback : MonoBehaviour
{
    [Header("Lives UI")]
    [SerializeField] private RectTransform livesText;

    [Header("Flash Settings")]
    [SerializeField] private Color flashColor =
        new Color(1f, 0.25f, 0.3f, 1f);

    [SerializeField] private float animationDuration = 0.35f;

    [Header("Shake Settings")]
    [SerializeField] private float shakeAmount = 8f;
    [SerializeField] private float shakeSpeed = 35f;

    private TMP_Text livesTMP;
    private Color originalColor;
    private Vector2 originalPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (livesText != null)
        {
            livesTMP = livesText.GetComponent<TMP_Text>();
            originalPosition = livesText.anchoredPosition;

            if (livesTMP != null)
                originalColor = livesTMP.color;
        }
    }

    private void OnEnable()
    {
        GameEvents.BallLost += OnBallLost;
    }

    private void OnDisable()
    {
        GameEvents.BallLost -= OnBallLost;

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        RestoreOriginalAppearance();
    }

    private void OnBallLost()
    {
        if (livesText == null)
            return;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(AnimateLifeLoss());
    }

    private IEnumerator AnimateLifeLoss()
    {
        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);
            float intensity = 1f - t;

            // Shake horizontally, fading out over time
            float offset =
                Mathf.Sin(timer * shakeSpeed) *
                shakeAmount * intensity;

            livesText.anchoredPosition =
                originalPosition + new Vector2(offset, 0f);

            // Flash red, then return to white
            if (livesTMP != null)
            {
                livesTMP.color =
                    Color.Lerp(flashColor, originalColor, t);
            }

            yield return null;
        }

        RestoreOriginalAppearance();
        animationCoroutine = null;
    }

    private void RestoreOriginalAppearance()
    {
        if (livesText != null)
            livesText.anchoredPosition = originalPosition;

        if (livesTMP != null)
            livesTMP.color = originalColor;
    }
}

