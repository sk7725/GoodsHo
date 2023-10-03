using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScaler : MonoBehaviour {
    public MeshRenderer mrenderer;
    public MeshRenderer mrendererBack;
    public Transform body;

    private Material mat, mat1;

    private void Awake() {
        mat = mrenderer.material;
        mat1 = mrendererBack.material;
    }

    public void SetImage(Texture2D source) {
        mat.mainTexture = source;
        mat1.mainTexture = source;
        SetScale();
    }

    public void SetScale() {
        Texture2D t = (Texture2D)mat.mainTexture;
        body.localScale = new Vector3(1, t.height / (float)t.width, 1);
        body.localPosition = Vector3.up * transform.localScale.y / 2f;
    }
}
