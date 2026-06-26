using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EkranKarartma : MonoBehaviour
{
    public Image siyahGorsel;
    public float gecisSuresi = 1f;

    private void Awake()
    {
        if (siyahGorsel == null)
        {
            return;
        }

        siyahGorsel.gameObject.SetActive(true);
        Color renk = siyahGorsel.color;
        renk.a = 1f;
        siyahGorsel.color = renk;
    }

    private void Start()
    {
        if (siyahGorsel != null)
        {
            StartCoroutine(EkraniAydinlat());
        }
    }

    public void YeniSahneyeGec(string sahneAdi)
    {
        if (siyahGorsel != null)
        {
            StartCoroutine(EkraniKarartVeGec(sahneAdi));
        }
        else
        {
            SceneManager.LoadScene(sahneAdi);
        }
    }

    public void SahneyiYenidenBaslat()
    {
        YeniSahneyeGec(SceneManager.GetActiveScene().name);
    }

    private IEnumerator EkraniAydinlat()
    {
        float zaman = 0f;
        while (zaman < gecisSuresi)
        {
            zaman += Time.deltaTime;
            Color renk = siyahGorsel.color;
            renk.a = Mathf.Lerp(1f, 0f, zaman / Mathf.Max(gecisSuresi, 0.01f));
            siyahGorsel.color = renk;
            yield return null;
        }

        Color sonRenk = siyahGorsel.color;
        sonRenk.a = 0f;
        siyahGorsel.color = sonRenk;
        siyahGorsel.gameObject.SetActive(false);
    }

    private IEnumerator EkraniKarartVeGec(string sahneAdi)
    {
        siyahGorsel.gameObject.SetActive(true);

        float zaman = 0f;
        while (zaman < gecisSuresi)
        {
            zaman += Time.deltaTime;
            Color renk = siyahGorsel.color;
            renk.a = Mathf.Lerp(0f, 1f, zaman / Mathf.Max(gecisSuresi, 0.01f));
            siyahGorsel.color = renk;
            yield return null;
        }

        SceneManager.LoadScene(sahneAdi);
    }
}
