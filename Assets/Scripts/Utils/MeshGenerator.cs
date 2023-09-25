using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class MeshGenerator {
    public static void GenerateAsync(List<Vector2> source, float thickness, LocalizedTMP label, Image loadingBar, System.Action<Mesh> endAction, MonoBehaviour caller) {
        caller.StartCoroutine(IGenerateAsync(source, thickness, label, loadingBar, endAction, caller));
    }

    static List<Vector3> vertices = new(), tmpv = new();
    static List<int> indices = new(), tmpi = new();

    static IEnumerator IGenerateAsync(List<Vector2> vertices2D, float thickness, LocalizedTMP label, Image loadingBar, System.Action<Mesh> endAction, MonoBehaviour caller) {
        label.Set("mesh.triangulate");
        loadingBar.fillAmount = 0f;

        indices.Clear();
        vertices.Clear();
        yield return null;

        //generate front indices
        var tt = new Triangulator(vertices2D.ToArray());
        int[] frontIndices = tt.Triangulate();

        /*Debug.Log(frontIndices.Length);
        for (int i = 0; i < frontIndices.Length; i += 3) {
            Debug.Log($"{frontIndices[i]} {frontIndices[i + 1]} {frontIndices[i + 2]}");
        }*/

        //add front face
        label.Set("mesh.front");
        yield return null;

        foreach (var v in vertices2D) {
            vertices.Add(new Vector3(v.x, v.y, -thickness));
        }
        indices.AddRange(frontIndices);

        //add back face
        label.Set("mesh.back");
        yield return null;

        int offset = vertices.Count;
        foreach (var v in vertices2D) {
            vertices.Add(new Vector3(v.x, v.y, 0));
        }

        tmpi.Clear();
        tmpi.AddRange(frontIndices);
        tmpi.Reverse();
        foreach (var i in tmpi) {
            indices.Add(i + offset);
        }

        //add side edges
        label.Set("mesh.sides");
        yield return null;

        //first duplicate vertices
        tmpv.Clear();
        tmpv.AddRange(vertices);
        vertices.AddRange(tmpv);

        //cylinder edges
        for (int i = offset * 2; i < offset * 3 - 1; i++) {
            int v1 = i;
            int v2 = i + 1;
            int v3 = i + 1 + offset;
            int v4 = i + offset;

            indices.Add(v1);
            indices.Add(v3);
            indices.Add(v2);

            indices.Add(v1);
            indices.Add(v4);
            indices.Add(v3);
        }

        //generate mesh
        label.Set("mesh.assign");
        yield return null;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        //align normals
        //todo add second layer of normals??
        /*
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < offset; i++) {
            normals[i] = Vector3.forward;
        }
        for (int i = offset; i < offset << 1; i++) {
            normals[i] = -Vector3.forward;
        }
        mesh.normals = normals;*/

        label.Clear();
        loadingBar.fillAmount = 1f;
        endAction.Invoke(mesh);
    }
}
