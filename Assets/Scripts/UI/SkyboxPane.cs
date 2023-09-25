using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class SkyboxPane : MonoBehaviour {
    [SerializeField] private SkyboxButton buttonPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private SkyboxList list;
    [SerializeField] private LocalizedStringTable localizationTable;

    private void Start() {
        LocalizationSettings.SelectedLocaleChanged += LocaleChanged;
        Build();
    }

    public void Build() {
        var table = localizationTable.GetTable();
        ClearChildren(content);

        for (int i = 0; i < list.data.Length; i++) {
            var s = table.GetEntry(list.data[i].name);
            Instantiate(buttonPrefab, content).Set(list.data[i], s == null ? list.data[i].name : s.GetLocalizedString());
        }
    }

    private static void ClearChildren(Transform o) {
        int n = o.childCount;
        if (n <= 0) return;
        for (int i = n - 1; i >= 0; i--) {
            GameObject.Destroy(o.GetChild(i).gameObject);
        }
    }

    private void LocaleChanged(Locale locale) {
        if(!Application.isPlaying) return;
        Build();
    }
}
