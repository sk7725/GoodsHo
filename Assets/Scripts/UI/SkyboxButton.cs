using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkyboxButton : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI label;

    private SkyboxData skybox;

    void Start() {
        GetComponent<Button>().onClick.AddListener(Clicked);
    }

    public void Set(SkyboxData skybox, string name) {
        this.skybox = skybox;
        label.text = name;
    }

    void Clicked() {
        RenderSettings.skybox = skybox.material;
        Light light = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>();
        light.colorTemperature = skybox.lightTemperature;
        light.color = skybox.lightColor;

        ReflectionProbe probe = GameObject.FindGameObjectWithTag("MainReflectionProbe").GetComponent<ReflectionProbe>();
        probe.RenderProbe();
    }
}
