using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrylSettings : SettingsHolder {
    [Header("References")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform stand;
    [SerializeField] private Transform defaultStand;

    [Header("Settings")]
    public Texture2D bodyImage;
    public Texture2D standImage;

    public bool useDefaultStand = true;

    public float thickness = 0.04f;
    public float bevel = 0.15f;

    public float bodyScale = 1f;
    public float standScale = 0.5f;

    public int outlineIterations = 3;
    public int downscaleFactor = 8;
    public float alphaCutoff = 0.1f;

    public override void Apply() {
        AcrylManager.main.body.sourceImage = bodyImage;
        AcrylManager.main.useDefaultStand = useDefaultStand;

        AcrylManager.main.outliner.thickness = thickness / 2f;
        body.transform.localPosition = Vector3.up * thickness;
        body.transform.localScale = new Vector3(bodyScale, bodyScale, 1f);
        stand.transform.localScale = new Vector3(standScale, 1f, standScale);
        if (useDefaultStand) {
            defaultStand.transform.localScale = new Vector3(defaultStand.transform.localScale.x, thickness / 2f, defaultStand.transform.localScale.z);
            defaultStand.transform.localPosition = Vector3.up * (thickness / 2f);
        }
        else {
            AcrylManager.main.stand.sourceImage = standImage;
        }

        AcrylManager.main.outliner.bevel = bevel;
        AcrylManager.main.outliner.outlineIterations = outlineIterations;
        AcrylManager.main.outliner.downscaleFactor = downscaleFactor;
        AcrylManager.main.outliner.alphaCutoff = alphaCutoff;
    }

    public override void BuildUI(Transform table) {
        //todo
    }
}
