using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CantaEsyasiEtkilesimi : MonoBehaviour
{
    public enum EsyaTuru
    {
        Dogru,
        Yanlis
    }

    [Header("Esya")]
    public EsyaTuru esyaTuru = EsyaTuru.Dogru;
    public string esyaId;
    public string yonergeMetni = "E tusuna bas: Cantaya ekle";
    public string aciklamaMetni;
    public string yanlisMesaj = "Bu esya simdi gerekli degil.";

    [Header("Baglantilar")]
    public CantaKontrol cantaKontrol;
    public TextMeshProUGUI yonergeYazisi;

    private bool oyuncuAlanda;
    private bool kullanildi;
    private Transform oyuncu;

    private static readonly HashSet<CantaEsyasiEtkilesimi> YakindakiEsyalar = new();

    private void Update()
    {
        if (!oyuncuAlanda || kullanildi || EnYakinEsyayiBul() != this ||
            !Input.GetKeyDown(KeyCode.E))
        {
            return;
        }

        kullanildi = true;
        YaziYaz("");

        if (cantaKontrol == null)
        {
            cantaKontrol = Object.FindFirstObjectByType<CantaKontrol>();
        }

        if (cantaKontrol == null)
        {
            kullanildi = false;
            return;
        }

        if (esyaTuru == EsyaTuru.Dogru)
        {
            cantaKontrol.DogruEsyaTopla(gameObject, esyaId, aciklamaMetni);
        }
        else
        {
            cantaKontrol.YanlisEsyaSecildi(yanlisMesaj);
            kullanildi = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!OyuncuMu(other) || kullanildi)
        {
            return;
        }

        oyuncuAlanda = true;
        oyuncu = other.transform;
        YakindakiEsyalar.Add(this);
        YonergeyiGuncelle();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!OyuncuMu(other))
        {
            return;
        }

        oyuncuAlanda = false;
        YakindakiEsyalar.Remove(this);
        YonergeyiGuncelle();
    }

    private void OnDisable()
    {
        YakindakiEsyalar.Remove(this);
        YonergeyiGuncelle();
    }

    private bool OyuncuMu(Collider other)
    {
        return other.CompareTag("Player") || other.gameObject.name == "Oyuncu";
    }

    private CantaEsyasiEtkilesimi EnYakinEsyayiBul()
    {
        YakindakiEsyalar.RemoveWhere(esya =>
            esya == null || !esya.oyuncuAlanda || esya.kullanildi);

        Transform hedefOyuncu = oyuncu;
        foreach (CantaEsyasiEtkilesimi esya in YakindakiEsyalar)
        {
            if (hedefOyuncu == null && esya.oyuncu != null)
            {
                hedefOyuncu = esya.oyuncu;
            }
        }

        CantaEsyasiEtkilesimi enYakin = null;
        float enKisaMesafe = float.MaxValue;

        foreach (CantaEsyasiEtkilesimi esya in YakindakiEsyalar)
        {
            if (hedefOyuncu == null)
            {
                break;
            }

            float mesafe = (esya.transform.position - hedefOyuncu.position).sqrMagnitude;
            if (mesafe < enKisaMesafe)
            {
                enKisaMesafe = mesafe;
                enYakin = esya;
            }
        }

        return enYakin;
    }

    private void YonergeyiGuncelle()
    {
        CantaEsyasiEtkilesimi enYakin = EnYakinEsyayiBul();
        if (enYakin != null)
        {
            enYakin.YaziYaz(enYakin.yonergeMetni);
        }
        else
        {
            YaziYaz("");
        }
    }

    private void YaziYaz(string metin)
    {
        if (yonergeYazisi != null)
        {
            yonergeYazisi.text = metin;
        }
    }
}
