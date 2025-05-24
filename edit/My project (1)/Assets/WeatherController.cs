using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeatherController : MonoBehaviour
{
    [Header("Particle Systems")]
    public ParticleSystem rainSystem;
    public ParticleSystem snowSystem;

    [Header("Wind Zone")]
    public WindZone windZone;
    public float windStrengthOn = 1f;

    [Header("Main Lighting")]
    public Light mainLight;

    [Header("Lightning Settings")]
    public Light lightningLight;
    public Vector3 lightningAreaCenter;
    public Vector3 lightningAreaSize = new Vector3(20, 20, 20);
    public float lightningPeakIntensity = 25f;
    public float lightningBaseIntensity = 8f;
    public float flashDuration = 0.1f;
    public int minSubFlashes = 1;
    public int maxSubFlashes = 3;
    public float subFlashInterval = 0.05f;
    public float minFlashDelay = 0.3f;
    public float maxFlashDelay = 1.5f;

    [Header("Audio Sources")]
    public AudioSource backgroundMusic;   // основная фоновая музыка
    public AudioSource ambientMusic;      // дополнительный фоновый трек
    public AudioSource rainSound;
    public AudioSource snowSound;
    public AudioSource thunderSound;
    public AudioSource windSound;

    [Header("UI Buttons")]
    public Button clearButton;
    public Button rainButton;
    public Button snowButton;
    public Button thunderButton;
    public Button windButton;

    [Header("Screen Flash Overlay")]
    public Image flashImage;
    public float flashFadeSpeed = 5f;

    [Header("Snow Objects (в сцене)")]
    public GameObject[] snowObjects;

    private float initialMainIntensity;
    private bool windOn = false;

    void Awake()
    {
        // Сохраняем яркость основного света
        if (mainLight != null)
        {
            initialMainIntensity = mainLight.intensity;
            mainLight.enabled = true;
        }

        // Подготовка вспышки молнии
        if (lightningLight != null)
        {
            lightningLight.enabled = false;
            lightningLight.intensity = 0f;
        }

        if (flashImage != null)
        {
            var c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }

        // Стартуем оба фоновых трека
        if (ambientMusic != null)
            ambientMusic.Play();
        if (backgroundMusic != null)
            backgroundMusic.Play();

        // По умолчанию выключаем все эффекты
        ToggleRain(false);
        ToggleSnow(false);
        ToggleFog(false);
        ToggleWind(false);
    }

    void Start()
    {
        clearButton.onClick.AddListener(OnClear);
        rainButton.onClick.AddListener(() => { StopBackgroundMusic(); OnRain(); });
        snowButton.onClick.AddListener(() => { StopBackgroundMusic(); OnSnow(); });
        thunderButton.onClick.AddListener(() => { StopBackgroundMusic(); OnThunderstorm(); });
        windButton.onClick.AddListener(() => { StopBackgroundMusic(); OnWindToggle(); });

        OnClear();
    }

    void StopBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying)
            backgroundMusic.Stop();
        // ambientMusic не трогаем — он играет всегда
    }

    void ToggleRain(bool on)
    {
        if (rainSystem != null)
        {
            var e = rainSystem.emission;
            e.enabled = on;
            if (on) rainSystem.Play();
            else   rainSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (rainSound != null)
        {
            if (on  && !rainSound.isPlaying) rainSound.Play();
            if (!on &&  rainSound.isPlaying) rainSound.Stop();
        }
    }

    void ToggleSnow(bool on)
    {
        if (snowSystem != null)
        {
            var e = snowSystem.emission;
            e.enabled = on;
            if (on) snowSystem.Play();
            else   snowSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        if (snowSound != null)
        {
            if (on  && !snowSound.isPlaying) snowSound.Play();
            if (!on &&  snowSound.isPlaying) snowSound.Stop();
        }
        foreach (var obj in snowObjects)
            if (obj != null)
                obj.SetActive(on);
    }

    void ToggleFog(bool on)
    {
        RenderSettings.fog = on;
    }

    void ToggleWind(bool on)
    {
        windOn = on;
        if (windZone != null)
            windZone.windMain = on ? windStrengthOn : 0f;

        var colors = windButton.colors;
        colors.normalColor = windOn ? Color.green : Color.red;
        windButton.colors = colors;

        if (windSound != null)
        {
            if (on  && !windSound.isPlaying) windSound.Play();
            if (!on &&  windSound.isPlaying) windSound.Stop();
        }
    }

    void OnClear()
    {
        ToggleRain(false);
        ToggleSnow(false);
        ToggleFog(false);

        StopAllCoroutines();
        if (lightningLight != null)
            lightningLight.enabled = false;

        if (mainLight != null)
            mainLight.intensity = initialMainIntensity;

        if (flashImage != null)
        {
            var c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
        }

        if (rainSound    != null && rainSound.isPlaying)    rainSound.Stop();
        if (snowSound    != null && snowSound.isPlaying)    snowSound.Stop();
        if (thunderSound != null && thunderSound.isPlaying) thunderSound.Stop();

        // Возвращаем основную музыку,
        // дополнительная играет без перерыва
        if (backgroundMusic != null)
            backgroundMusic.Play();
    }

    void OnRain()
    {
        ToggleRain(true);
        ToggleSnow(false);
        ToggleFog(true);

        if (rainSystem != null)
        {
            var e = rainSystem.emission; e.rateOverTime = 150f;
            var m = rainSystem.main;     m.startSpeed    = 10f;
        }

        RenderSettings.fogColor   = new Color(0.5f,0.5f,0.5f);
        RenderSettings.fogDensity = 0.02f;

        StopAllCoroutines();
        if (lightningLight != null) lightningLight.enabled = false;
        if (mainLight    != null) mainLight.intensity = initialMainIntensity;

        if (rainSound != null && !rainSound.isPlaying) rainSound.Play();
        if (windOn    && windSound != null && !windSound.isPlaying) windSound.Play();
    }

    void OnSnow()
    {
        ToggleSnow(true);
        ToggleRain(false);
        ToggleFog(true);

        RenderSettings.fogColor   = new Color(0.8f,0.8f,0.9f);
        RenderSettings.fogDensity = 0.01f;

        StopAllCoroutines();
        if (lightningLight != null) lightningLight.enabled = false;
        if (mainLight    != null) mainLight.intensity = initialMainIntensity;

        if (windOn    && windSound  != null && !windSound.isPlaying) windSound.Play();
    }

    void OnThunderstorm()
    {
        ToggleRain(true);
        ToggleSnow(false);
        ToggleFog(true);

        if (rainSystem != null)
        {
            var e = rainSystem.emission; e.rateOverTime = 500f;
            var m = rainSystem.main;     m.startSpeed    = 20f;
        }

        RenderSettings.fogColor   = new Color(0.15f,0.15f,0.2f);
        RenderSettings.fogDensity = 0.07f;
        if (mainLight != null) mainLight.intensity = initialMainIntensity * 0.6f;

        if (thunderSound != null) thunderSound.Play();
        if (rainSound    != null && !rainSound.isPlaying) rainSound.Play();
        if (windOn       && windSound  != null && !windSound.isPlaying) windSound.Play();

        StartCoroutine(LightningRoutine());
    }

    void OnWindToggle()
    {
        ToggleWind(!windOn);
    }

    IEnumerator LightningRoutine()
    {
        if (lightningLight == null) yield break;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlashDelay, maxFlashDelay));

            var randomPos = lightningAreaCenter + new Vector3(
                Random.Range(-lightningAreaSize.x, lightningAreaSize.x),
                Random.Range(-lightningAreaSize.y, lightningAreaSize.y),
                Random.Range(-lightningAreaSize.z, lightningAreaSize.z)
            );
            lightningLight.transform.position = randomPos;

            yield return StartCoroutine(DoLightningFlash());
        }
    }

    IEnumerator DoLightningFlash()
    {
        lightningLight.enabled   = true;
        lightningLight.intensity = lightningPeakIntensity;
        if (flashImage != null)
            flashImage.color = new Color(1,1,1,1);
        if (flashImage != null)
            StartCoroutine(FadeFlash());

        yield return new WaitForSeconds(flashDuration);

        lightningLight.intensity = lightningBaseIntensity;
        int count = Random.Range(minSubFlashes, maxSubFlashes + 1);
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(subFlashInterval);
            lightningLight.intensity = lightningPeakIntensity * 0.5f;
            yield return new WaitForSeconds(subFlashInterval);
            lightningLight.intensity = lightningBaseIntensity;
        }

        lightningLight.enabled = false;
    }

    IEnumerator FadeFlash()
    {
        var c = flashImage.color;
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * flashFadeSpeed;
            flashImage.color = c;
            yield return null;
        }
    }
}
