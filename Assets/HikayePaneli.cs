using TMPro;
using UnityEngine;

public class HikayePaneli : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI baslikYazisi;
    public TextMeshProUGUI aciklamaYazisi;
    public GameObject panel;

    [Header("Metin")]
    public string baslik = "Güvenli Gün Macerası";
    [TextArea(2, 4)] public string aciklama = "Bugün güvenli davranışları birlikte öğreneceğiz.";
    public float otomatikKapanmaSuresi = 4f;

    private float sayac;

    private void Start()
    {
        PaneliGoster(baslik, aciklama);
    }

    private void Update()
    {
        if (panel == null || !panel.activeSelf)
        {
            return;
        }

        sayac += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E) || sayac >= otomatikKapanmaSuresi)
        {
            panel.SetActive(false);
        }
    }

    public void PaneliGoster(string yeniBaslik, string yeniAciklama)
    {
        sayac = 0f;

        if (baslikYazisi != null)
        {
            baslikYazisi.text = yeniBaslik;
        }

        if (aciklamaYazisi != null)
        {
            aciklamaYazisi.text = yeniAciklama;
        }

        if (panel != null)
        {
            panel.SetActive(true);
        }
    }
}
