using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SettingsHolder : MonoBehaviour {

    public abstract void Apply();
    public abstract void BuildUI(Transform table);
}
