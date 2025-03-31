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
    public float fadeDuration = 1.5f; // Smooth transition time

    private float currentTrackTime = 0f; // Keeps track of the playback position

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
                if (audioSource.clip != area.musicClip)
                {
                    StartCoroutine(FadeMusic(area.musicClip));
                }
                return;
            }
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float volume = audioSource.volume;
        currentTrackTime = audioSource.time; // Save current timestamp

        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(volume, 0, t / fadeDuration);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.time = Mathf.Min(currentTrackTime, newClip.length - 0.1f); // Ensure we don't exceed the track length
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, volume, t / fadeDuration);
            yield return null;
        }
    }
}
