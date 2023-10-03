using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AcrylShaper : MonoBehaviour {
    public bool isBody = true;

    [Header("Sources")]
    public Texture2D sourceImage;
    public Outliner outliner;
    public ImageScaler imageScaler;

    [Header("Front Acryl")]
    public MeshFilter filter;
    public MeshRenderer mrenderer;

    [Header("Back Acryl")]
    public MeshFilter filterBack;
    public MeshRenderer mrendererBack;

    private bool generating = false;

    public bool Generating { get { return generating; } }

    List<Vector2> path = new();
    List<Vector2> points = new();

    void Awake() {
        generating = false;
    }

    Texture2D originalImage;

    public void Shape() {
        if (generating) return;
        generating = true;
        originalImage = sourceImage;
        mrenderer.enabled = false;
        mrendererBack.enabled = false;
        path.Clear();
        points.Clear();

        imageScaler.SetImage(sourceImage);

        //Texture2D outlined = TextureOutlineGenerator.Generate(source, outliner.outlinePadding, outliner.outlineIterations, outliner.downscaleFactor);
        //tempRenderer.texture = outlined;
        //GetPath(outlined);

        TextureOutlineGenerator.GenerateAsync(sourceImage, outliner.outlinePadding, outliner.outlineIterations, outliner.downscaleFactor, AcrylManager.main.loadingLabel, AcrylManager.main.loadingBar, AfterTextureGeneration, this);
    }

    void AfterTextureGeneration(Texture2D outlined) {
        AcrylManager.main.loadingLabel.Set("getpath");
        AcrylManager.main.loadingBar.fillAmount = 0;
        //tempRenderer.texture = outlined;
        GetPath(outlined);

        if (isBody) {
            float miny = 0f;
            for (int i = 0; i < points.Count; i++) {
                if (miny > points[i].y) miny = points[i].y;
            }
            float l = -miny;

            transform.localPosition = Vector3.forward * 0.0025f + Vector3.up * l;
            imageScaler.transform.localPosition = Vector3.up * l;
        }

        //todo is it always cw?
        points.Reverse();

        Destroy(outlined);
        for (int i = 0; i < outliner.subdivisions; i++) {
            ShapeSmoothing.SubdivideEdges(points, 2);
            ShapeSmoothing.SmoothCorners(points, outliner.smoothingFactor, outliner.smoothingWindow);
        }

        //simplify mesh
        path.Clear();
        path.AddRange(points);
        LineUtility.Simplify(path, outliner.finalTolerance, points);

        MeshGenerator.GenerateAsync(points, outliner.thickness, outliner.bevel, AcrylManager.main.loadingLabel, AcrylManager.main.loadingBar, AfterMeshGeneration, this);
    }

    void AfterMeshGeneration(Mesh mesh) {
        AcrylManager.main.loadingLabel.Set("done");
        AcrylManager.main.loadingBar.fillAmount = 1;
        filter.mesh = mesh;
        mrenderer.enabled = true;
        filterBack.mesh = mesh;
        mrendererBack.enabled = true;

        generating = false;
    }

    private void GetPath(Texture2D source) {
        var boundaryTracer = new ContourTracer();
        boundaryTracer.Trace(source, new Vector2(0.5f, 0.5f), 100, outliner.gapLength, outliner.product, outliner.alphaCutoff);

        //get the first path only
        boundaryTracer.GetPath(0, ref path);
        LineUtility.Simplify(path, outliner.tolerance, points);

        float scale = 100f / (source.width - 2 * outliner.outlinePadding);
        for (int i = 0; i < points.Count; i++) {
            points[i] *= scale;
        }
    }

#if UNITY_EDITOR
    List<Vector3> gizmo_tmp = new(), gizmo_tmp2 = new();

    private void OnDrawGizmosSelected() {
        if (!Application.isPlaying || points.Count < 3) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + (Vector3)points[0], transform.position + (Vector3)points[1]);
        Gizmos.color = Color.yellow;
        for (int i = 1; i < points.Count - 1; i++) {
            Gizmos.DrawLine(transform.position + (Vector3)points[i], transform.position + (Vector3)points[i + 1]);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + (Vector3)points[points.Count - 1], transform.position + (Vector3)points[0]);

        int n = filter.sharedMesh.vertexCount;
        gizmo_tmp.Clear();
        gizmo_tmp2.Clear();
        filter.sharedMesh.GetVertices(gizmo_tmp);
        filter.sharedMesh.GetNormals(gizmo_tmp2);

        Gizmos.color = Color.cyan;
        for(int i = 0; i < n; i++) {
            Vector3 v = gizmo_tmp[i];
            Vector3 norm = gizmo_tmp2[i];
            Gizmos.DrawLine(transform.position + v, transform.position + v + norm * 0.05f);
        }
    }
#endif
}
