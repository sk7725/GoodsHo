using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcrylManager : MonoBehaviour {
    public static AcrylManager main;

    [Header("Shapers")]
    public AcrylShaper body;
    public AcrylShaper stand;
    public GameObject defaultStand;
    public Outliner outliner;
    public AcrylSettings settings;

    [Header("UI")]
    public Image loadingBar;
    public LocalizedTMP loadingLabel;
    public Image loadingBackground;

    [NonSerialized] public bool useDefaultStand = true;

    private bool generating = false;
    public bool Generating => generating;

    private void Awake() {
        main = this;
        loadingBackground.gameObject.SetActive(false);
    }
    void Start() {
        Generate();//todo temp
    }

    private void Update() {
        //todo temp
        if (Input.GetKeyDown(KeyCode.Space)) {
            Generate();
        }
    }

    public void Generate() {
        if (generating) return;
        generating = true;
        settings.Apply();
        StartCoroutine(IGenerate());
    }

    IEnumerator IGenerate() {
        loadingBackground.gameObject.SetActive(true);
        body.Shape();
        yield return new WaitWhile(() => body.Generating);

        if (!useDefaultStand) {
            stand.Shape();
            yield return new WaitWhile(() => stand.Generating);
        }

        generating = false;
        loadingBackground.gameObject.SetActive(false);
    }
}
