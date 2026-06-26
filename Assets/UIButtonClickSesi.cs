using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonClickSesi : MonoBehaviour
{
    private static UIButtonClickSesi instance;
    private static AudioClip clickClip;
    private readonly HashSet<int> baglananButonlar = new HashSet<int>();

    private AudioSource audioSource;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Baslat()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("UI_Click_Sesi");
            DontDestroyOnLoad(obj);
            instance = obj.AddComponent<UIButtonClickSesi>();
        }

        instance.ButonlariBagla();
        SceneManager.sceneLoaded -= SahneYuklendi;
        SceneManager.sceneLoaded += SahneYuklendi;
    }

    private static void SahneYuklendi(Scene scene, LoadSceneMode mode)
    {
        if (instance != null)
        {
            instance.ButonlariBagla();
        }
    }

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.volume = 0.35f;
        clickClip = ClickClipOlustur();
    }

    private void ButonlariBagla()
    {
        foreach (Button button in FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (button == null || baglananButonlar.Contains(button.GetInstanceID()))
            {
                continue;
            }

            baglananButonlar.Add(button.GetInstanceID());
            button.onClick.AddListener(ClickCal);
        }
    }

    private void ClickCal()
    {
        if (audioSource != null && clickClip != null)
        {
            audioSource.PlayOneShot(clickClip);
        }
    }

    private static AudioClip ClickClipOlustur()
    {
        const int sampleRate = 44100;
        const float duration = 0.08f;
        int sampleCount = Mathf.CeilToInt(sampleRate * duration);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = Mathf.Exp(-42f * t);
            float tone = Mathf.Sin(2f * Mathf.PI * 760f * t) * 0.55f +
                         Mathf.Sin(2f * Mathf.PI * 1180f * t) * 0.25f;
            samples[i] = tone * envelope;
        }

        AudioClip clip = AudioClip.Create("AfetBilinci_Click", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
