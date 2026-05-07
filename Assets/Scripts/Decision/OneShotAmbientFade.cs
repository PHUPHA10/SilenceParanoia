using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OneShotAmbientFade : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Timing")]
    public float delayBeforePlay = 5f;

    [Header("Fade")]
    public float fadeInDuration = 2f;
    public float stayDuration = 3f;
    public float fadeOutDuration = 4f;

    [Header("Volume")]
    [Range(0f, 1f)]
    public float targetVolume = 0.4f;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.volume = 0f;

        StartCoroutine(PlayAmbient());
    }

    IEnumerator PlayAmbient()
    {
        yield return new WaitForSeconds(delayBeforePlay);

        audioSource.Play();

        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                0f,
                targetVolume,
                timer / fadeInDuration
            );

            yield return null;
        }

        audioSource.volume = targetVolume;

        yield return new WaitForSeconds(stayDuration);

        timer = 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(
                targetVolume,
                0f,
                timer / fadeOutDuration
            );

            yield return null;
        }

        audioSource.volume = 0f;

        audioSource.Stop();
    }
}