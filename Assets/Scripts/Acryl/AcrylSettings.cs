using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcrylSettings : SettingsHolder {
    [Header("References")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform stand;
    [SerializeField] private Transform defaultStand, customStand;

    [Header("Settings")]
    public Texture2D bodyImage;
    public Texture2D standImage;

    public bool useDefaultStand = true;

    [Header("Model")]
    public float thickness = 0.04f;
    public float bevel = 0.15f;

    [Header("Scale")]
    public float bodyScale = 1f;
    public float standScale = 1f;

    [Header("Outline")]
    public bool useGaussian = false;
    public int downscaleFactor = 8;

    [Header("Dilation Outline")]
    public int outlineRadius = 16;
    public float dilationAlphaCutoff = 0.2f;

    [Header("Gaussian Outline")]
    public int outlineIterations = 3;
    public float alphaCutoff = 0.1f;

    public override void Apply() {
        AcrylManager.main.body.sourceImage = bodyImage;
        AcrylManager.main.useDefaultStand = useDefaultStand;

        AcrylManager.main.outliner.thickness = thickness / 2f;
        body.transform.localPosition = Vector3.up * thickness;
        body.transform.localScale = new Vector3(bodyScale, bodyScale, 1f);
        if (useDefaultStand) {
            defaultStand.gameObject.SetActive(true);
            customStand.gameObject.SetActive(false);
            defaultStand.transform.localScale = new Vector3(defaultStand.transform.localScale.x, thickness / 2f, defaultStand.transform.localScale.z);
            defaultStand.transform.localPosition = Vector3.up * (thickness / 2f);
            stand.transform.localScale = new Vector3(standScale / 2f, 1f, standScale / 2f);
        }
        else {
            defaultStand.gameObject.SetActive(false);
            customStand.gameObject.SetActive(true);
            customStand.transform.localPosition = Vector3.up * (thickness / 2f);
            AcrylManager.main.stand.sourceImage = standImage;

            float actualStandScale = standScale * (standImage.width / bodyImage.width);
            stand.transform.localScale = new Vector3(actualStandScale, 1f, actualStandScale);
        }

        AcrylManager.main.outliner.bevel = bevel;
        AcrylManager.main.outliner.useGaussian = useGaussian;
        AcrylManager.main.outliner.downscaleFactor = downscaleFactor;

        AcrylManager.main.outliner.alphaCutoffDilation = dilationAlphaCutoff;
        AcrylManager.main.outliner.dilationRadius = outlineRadius;

        AcrylManager.main.outliner.outlineIterations = outlineIterations;
        AcrylManager.main.outliner.alphaCutoffGaussian = alphaCutoff;
    }

    public override void BuildUI(Transform table) {
        //todo
    }
}
