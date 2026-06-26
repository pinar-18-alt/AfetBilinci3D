using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniSinavAkisi : MonoBehaviour
{
    private struct Soru
    {
        public string metin;
        public string[] secenekler;
        public int dogruIndex;
        public string aciklama;
    }

    private readonly Soru[] sorular =
    {
        new Soru
        {
            metin = "Depremden sonra yarali biri varsa hangi numarayi aramaliyiz?",
            secenekler = new[] { "112 Acil Cagri Merkezi", "Okul kantini", "Arkadasimizin telefonu" },
            dogruIndex = 0,
            aciklama = "112, afet ve acil durumlarda yardim istemek icin aranir."
        },
        new Soru
        {
            metin = "Toplanma alanina giderken nereden uzak durmaliyiz?",
            secenekler = new[] { "Elektrik direkleri ve yuksek binalar", "Parktaki acik alan", "Bos kaldirim" },
            dogruIndex = 0,
            aciklama = "Elektrik direkleri ve yuksek binalar tehlikeli olabilir."
        },
        new Soru
        {
            metin = "Acil durum cantasinda hangisi bulunmalidir?",
            secenekler = new[] { "Su", "Oyuncak araba", "Cikolata kutusu" },
            dogruIndex = 0,
            aciklama = "Su, acil durum cantasinin temel parcalarindandir."
        }
    };

    private string sonrakiSahne = "SkorEkrani";
    private Canvas canvas;
    private TextMeshProUGUI baslikText;
    private TextMeshProUGUI soruText;
    private TextMeshProUGUI geriBildirimText;
    private Button[] secenekButonlari;
    private Button devamButonu;
    private int soruIndex;
    private bool cevapBekliyor;

    public static void Baslat(string hedefSahne)
    {
        GameObject obj = new GameObject("MiniSinavAkisi");
        MiniSinavAkisi sinav = obj.AddComponent<MiniSinavAkisi>();
        sinav.sonrakiSahne = hedefSahne;
    }

    private void Start()
    {
        foreach (OyuncuKontrol kontrol in FindObjectsByType<OyuncuKontrol>(FindObjectsSortMode.None))
        {
            kontrol.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        KurArayuz();
        SoruyuGoster();
    }

    private void KurArayuz()
    {
        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
        }

        if (FindFirstObjectByType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        GameObject kararti = new GameObject("SinavKararti", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        kararti.transform.SetParent(canvas.transform, false);
        RectTransform karartiRect = kararti.GetComponent<RectTransform>();
        karartiRect.anchorMin = Vector2.zero;
        karartiRect.anchorMax = Vector2.one;
        karartiRect.offsetMin = Vector2.zero;
        karartiRect.offsetMax = Vector2.zero;
        kararti.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);

        GameObject panel = new GameObject("MiniSinavPaneli", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(820f, 620f);
        panel.GetComponent<Image>().color = new Color(1f, 0.96f, 0.78f, 0.96f);

        baslikText = CreateText(panel.transform, "SinavBaslik", new Vector2(0f, 240f), new Vector2(720f, 70f), 36f, "Harika! Kisa bir guvenlik sorusu.");
        soruText = CreateText(panel.transform, "Soru", new Vector2(0f, 130f), new Vector2(700f, 110f), 30f, "");
        geriBildirimText = CreateText(panel.transform, "GeriBildirim", new Vector2(0f, -170f), new Vector2(700f, 90f), 24f, "");
        geriBildirimText.color = new Color(0.10f, 0.32f, 0.42f);

        secenekButonlari = new Button[3];
        for (int i = 0; i < secenekButonlari.Length; i++)
        {
            int index = i;
            secenekButonlari[i] = CreateButton(panel.transform, "Secenek_" + i, new Vector2(0f, 42f - i * 74f), new Vector2(620f, 58f), "", () => Cevapla(index));
        }

        devamButonu = CreateButton(panel.transform, "DevamButonu", new Vector2(0f, -250f), new Vector2(300f, 58f), "Devam", DevamEt);
        devamButonu.gameObject.SetActive(false);
    }

    private void SoruyuGoster()
    {
        cevapBekliyor = true;
        Soru soru = sorular[soruIndex];
        baslikText.text = "Mini Sinav " + (soruIndex + 1) + " / " + sorular.Length;
        soruText.text = soru.metin;
        geriBildirimText.text = "";
        devamButonu.gameObject.SetActive(false);

        for (int i = 0; i < secenekButonlari.Length; i++)
        {
            secenekButonlari[i].interactable = true;
            secenekButonlari[i].GetComponentInChildren<TextMeshProUGUI>().text = soru.secenekler[i];
        }
    }

    private void Cevapla(int secim)
    {
        if (!cevapBekliyor)
        {
            return;
        }

        cevapBekliyor = false;
        Soru soru = sorular[soruIndex];
        bool dogru = secim == soru.dogruIndex;

        foreach (Button button in secenekButonlari)
        {
            button.interactable = false;
        }

        if (dogru)
        {
            geriBildirimText.text = "Dogru cevap! Aferin.";
        }
        else
        {
            if (OyunYonetici.info != null)
            {
                OyunYonetici.info.PuanDegistir(-5);
            }

            geriBildirimText.text = "Bir dahaki sefere dikkat. Dogru cevap: " +
                                    soru.secenekler[soru.dogruIndex] + "\n" + soru.aciklama;
        }

        devamButonu.gameObject.SetActive(true);
    }

    private void DevamEt()
    {
        soruIndex++;
        if (soruIndex < sorular.Length)
        {
            SoruyuGoster();
            return;
        }

        if (OyunYonetici.info != null)
        {
            OyunYonetici.info.SahneyeGec(sonrakiSahne);
        }
        else
        {
            SceneManager.LoadScene(sonrakiSahne);
        }
    }

    private TextMeshProUGUI CreateText(Transform parent, string name, Vector2 position, Vector2 size, float fontSize, string text)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.enableAutoSizing = true;
        tmp.fontSizeMin = 18f;
        tmp.fontSizeMax = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(0.08f, 0.23f, 0.31f);
        return tmp;
    }

    private Button CreateButton(Transform parent, string name, Vector2 position, Vector2 size, string text, UnityEngine.Events.UnityAction action)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        Image image = obj.GetComponent<Image>();
        image.color = new Color(0.20f, 0.58f, 0.38f, 1f);

        Button button = obj.GetComponent<Button>();
        button.onClick.AddListener(action);

        TextMeshProUGUI label = CreateText(obj.transform, "Yazi", Vector2.zero, size - new Vector2(30f, 12f), 25f, text);
        label.color = Color.white;
        label.fontStyle = FontStyles.Bold;
        return button;
    }
}
