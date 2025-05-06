using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [System.Serializable]
    public class AreaMusic
    {
        public string areaName;
        public AudioClip musicClip;
    }

    public AudioSource audioSource;
    public AreaMusic[] areaMusicList;
    public float fadeDuration = 1.5f;

    private float currentTrackTime = 0f;
    private AudioClip currentAreaClip;
    private bool isOverridden = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ChangeMusic(string areaName)
    {
        foreach (var area in areaMusicList)
        {
            if (area.areaName == areaName)
            {
                if (audioSource.clip != area.musicClip && !isOverridden)
                {
                    currentAreaClip = area.musicClip;
                    StartCoroutine(FadeMusic(area.musicClip));
                }
                return;
            }
        }
    }

    public void OverrideMusic(AudioClip overrideClip)
    {
        if (audioSource.clip != overrideClip && !isOverridden)
        {
            isOverridden = true;
            currentTrackTime = audioSource.time;
            StartCoroutine(FadeMusic(overrideClip));
        }
    }

    public void RevertToAreaMusic()
    {
        if (isOverridden)
        {
            isOverridden = false;
            StartCoroutine(FadeMusic(currentAreaClip));
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float volume = audioSource.volume;
        currentTrackTime = audioSource.time;

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(volume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.time = Mathf.Min(currentTrackTime, newClip.length - 0.1f);
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, volume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = volume;
    }
}
