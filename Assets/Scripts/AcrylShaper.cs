using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcrylShaper : MonoBehaviour {
    [Header("Sources")]
    public Texture2D sourceImage;
    public Outliner outliner;
    public Transform imageOffset;

    [Header("Front Acryl")]
    public MeshFilter filter;
    public MeshRenderer mrenderer;

    [Header("Back Acryl")]
    public MeshFilter filterBack;
    public MeshRenderer mrendererBack;

    [Header("UI")]
    public Image loadingBar;
    public TextMeshProUGUI loadingLabel;
    //public RawImage tempRenderer;

    List<Vector2> path = new List<Vector2>();
    List<Vector2> points = new List<Vector2>();

    void Start() {
        Shape(sourceImage);
    }

    private void Update() {
        //todo temp
        if (Input.GetKeyDown(KeyCode.Space)) {
            Shape(sourceImage);
        }
    }

    public void Shape(Texture2D source) {
        mrenderer.enabled = false;
        mrendererBack.enabled = false;
        path.Clear();
        points.Clear();

        //Texture2D outlined = TextureOutlineGenerator.Generate(source, outliner.outlinePadding, outliner.outlineIterations, outliner.downscaleFactor);
        //tempRenderer.texture = outlined;
        //GetPath(outlined);

        TextureOutlineGenerator.GenerateAsync(source, outliner.outlinePadding, outliner.outlineIterations, outliner.downscaleFactor, loadingLabel, loadingBar, AfterTextureGeneration, this);
    }

    void AfterTextureGeneration(Texture2D outlined) {
        //tempRenderer.texture = outlined;
        GetPath(outlined);
        transform.position = imageOffset.position;

        //todo is it always cw?
        points.Reverse();

        Destroy(outlined);
        MeshGenerator.GenerateAsync(points, 0.05f, loadingLabel, loadingBar, AfterMeshGeneration, this);
    }

    void AfterMeshGeneration(Mesh mesh) {
        filter.mesh = mesh;
        mrenderer.enabled = true;
        filterBack.mesh = mesh;
        mrendererBack.enabled = true;
    }

    private void GetPath(Texture2D source) {
        var boundaryTracer = new ContourTracer();
        boundaryTracer.Trace(source, new Vector2(0.5f, 0.5f), 100, outliner.gapLength, outliner.product, outliner.alphaCutoff);

        //get the first path only
        boundaryTracer.GetPath(0, ref path);
        LineUtility.Simplify(path, outliner.tolerance, points);

        float scale = 100f / source.width;
        for (int i = 0; i < points.Count; i++) {
            points[i] *= scale;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (!Application.isPlaying || points.Count < 3) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + (Vector3)points[0], transform.position + (Vector3)points[1]);
        Gizmos.color = Color.yellow;
        for (int i = 1; i < points.Count - 1; i++) {
            Gizmos.DrawLine(transform.position + (Vector3)points[i], transform.position + (Vector3)points[i + 1]);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + (Vector3)points[points.Count - 1], transform.position + (Vector3)points[0]);
    }
#endif
}
