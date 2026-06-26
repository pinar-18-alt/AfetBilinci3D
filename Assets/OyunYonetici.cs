using UnityEngine;

public class OyunYonetici : MonoBehaviour
{
    public static OyunYonetici info { get; private set; }

    [Header("Oyuncu Verileri")]
    public string oyuncuAdi = "Oyuncu";
    public int oyuncuPuani = 100;
    public float gecenSure = 0f;
    public int tamamlananGorevSayisi = 0;
    public string sonGeriBildirim = "";

    private bool oyunAktifMi = false;

    void Awake()
    {
        if (info != null && info != this)
        {
            Destroy(gameObject);
            return;
        }

        info = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (oyunAktifMi)
        {
            gecenSure += Time.deltaTime;
        }
    }

    public void OyunuBaslat(string isim)
    {
        oyuncuAdi = isim;
        oyuncuPuani = 100;
        gecenSure = 0f;
        tamamlananGorevSayisi = 0;
        sonGeriBildirim = "";
        oyunAktifMi = true;
    }

    public void PuanDegistir(int miktar)
    {
        oyuncuPuani = Mathf.Clamp(oyuncuPuani + miktar, 0, 100);
    }

    public void GorevTamamlandi(string geriBildirim)
    {
        tamamlananGorevSayisi++;
        sonGeriBildirim = geriBildirim;
    }

    public void YanlisSecimYapildi()
    {
        PuanDegistir(-10);

        EkranKarartma karartma = Object.FindFirstObjectByType<EkranKarartma>();
        if (karartma != null)
        {
            karartma.YeniSahneyeGec(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }

    public void OyunuBitir()
    {
        oyunAktifMi = false;
    }

    public void SahneyeGec(string sahneAdi)
    {
        EkranKarartma karartma = Object.FindFirstObjectByType<EkranKarartma>();
        if (karartma != null)
        {
            karartma.YeniSahneyeGec(sahneAdi);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sahneAdi);
        }
    }
}

