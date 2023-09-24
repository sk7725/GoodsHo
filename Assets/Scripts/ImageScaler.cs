using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ImageScaler : MonoBehaviour {
    public MeshRenderer mrenderer;
    public Transform body;

    void Start() {
        SetScale();
    }

    private void Update() {
        if (!Application.isPlaying && mrenderer != null && body != null) SetScale();
    }

    public void SetScale() {
        Texture2D t = (Texture2D)mrenderer.sharedMaterial.mainTexture;
        body.localScale = new Vector3(1, t.height / (float)t.width, 1);
        body.localPosition = Vector3.up * transform.localScale.y / 2f;
    }
}
