using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class LocalizedTMP : MonoBehaviour {
    public TextMeshProUGUI label;
    public LocalizedStringTable table;

    public void Set(string key) {
        var k = table.GetTable().GetEntry(key);
        if(k == null) {
            Debug.LogWarning($"Missing key {key}");
            label.text = key;
        }
        label.text = k.GetLocalizedString();
    }

    public void Format(string key, params string[] values) {
        label.text = string.Format(table.GetTable().GetEntry(key).GetLocalizedString(), values);
    }

    public void Clear() {
        label.text = string.Empty;
    }
}
