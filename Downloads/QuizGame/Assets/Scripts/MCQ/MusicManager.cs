using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;
    public void PlayMusic()
{
    if (audioSource != null && !audioSource.isPlaying)
    {
        audioSource.volume = 1f;
        audioSource.Play();
    }
}

    void Awake()
    {
        // Если уже есть экземпляр, удалим текущий (во избежание дублирования)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void FadeOutAndStop(float duration)
    {
        StartCoroutine(FadeOut(duration));
    }

    private System.Collections.IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            yield return null;
        }

        audioSource.Stop();
    }

    public void MuteMusic()
{
    if (audioSource != null)
        audioSource.volume = 0f;
}

public void UnmuteMusic()
{
    if (audioSource != null)
        audioSource.volume = 1f; // или сохранённое значение
}

}
