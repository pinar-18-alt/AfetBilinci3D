using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CantaListesiUI : MonoBehaviour
{
    [System.Serializable]
    public class ListeSatiri
    {
        public string esyaId;
        public TextMeshProUGUI yazi;
    }

    public List<ListeSatiri> satirlar = new List<ListeSatiri>();
    public Color bekliyorRengi = new Color(0.18f, 0.22f, 0.25f);
    public Color tamamRengi = new Color(0.16f, 0.55f, 0.30f);

    private readonly HashSet<string> toplananlar = new HashSet<string>();

    private void Start()
    {
        ListeyiYenile();
    }

    public void EsyaToplandi(string esyaId)
    {
        if (string.IsNullOrWhiteSpace(esyaId))
        {
            return;
        }

        toplananlar.Add(esyaId);
        ListeyiYenile();
    }

    private void ListeyiYenile()
    {
        foreach (ListeSatiri satir in satirlar)
        {
            if (satir == null || satir.yazi == null)
            {
                continue;
            }

            bool tamam = toplananlar.Contains(satir.esyaId);
            string temizMetin = satir.yazi.text;
            if (temizMetin.StartsWith("[ ] ") || temizMetin.StartsWith("[x] "))
            {
                temizMetin = temizMetin.Substring(4);
            }

            satir.yazi.text = (tamam ? "[x] " : "[ ] ") + temizMetin;
            satir.yazi.color = tamam ? tamamRengi : bekliyorRengi;
        }
    }
}
