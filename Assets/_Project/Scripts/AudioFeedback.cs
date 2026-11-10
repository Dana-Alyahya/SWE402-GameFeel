
using UnityEngine;

public class AudioFeedback : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip paddleSound;
    [SerializeField] private AudioClip brickSound;
    [SerializeField] private AudioClip wallSound;
    [SerializeField] private AudioClip ballLossSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float paddleVolume = 0.7f;

    [Range(0f, 1f)]
    [SerializeField] private float brickVolume = 0.3f;

    [Range(0f, 1f)]
    [SerializeField] private float wallVolume = 0.4f;

    [Range(0f, 1f)]
    [SerializeField] private float ballLossVolume = 0.6f;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        GameEvents.BallHitPaddle += OnBallHitPaddle;
        GameEvents.BrickDestroyed += OnBrickDestroyed;
        GameEvents.BallHitWall += OnBallHitWall;
        GameEvents.BallLost += OnBallLost;
    }

    private void OnDisable()
    {
        GameEvents.BallHitPaddle -= OnBallHitPaddle;
        GameEvents.BrickDestroyed -= OnBrickDestroyed;
        GameEvents.BallHitWall -= OnBallHitWall;
        GameEvents.BallLost -= OnBallLost;
    }

    private void OnBallHitPaddle(ImpactInfo info)
    {
        PlaySound(paddleSound, paddleVolume);
    }

    private void OnBrickDestroyed(ImpactInfo info)
    {
        PlaySound(brickSound, brickVolume);
    }

    private void OnBallHitWall(ImpactInfo info)
    {
        PlaySound(wallSound, wallVolume);
    }

    private void OnBallLost()
    {
        PlaySound(ballLossSound, ballLossVolume);
    }

    private void PlaySound(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }
}
