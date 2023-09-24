using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public static class PolygonTriangulation {
    static float CCWby2D(Vector3 a, Vector3 b, Vector3 c) {
        Vector3 p = b - a;
        Vector3 q = c - b;

        return Vector3.Cross(p, q).y;
    }

    static float getAreaOfTriangle(Vector3 dot1, Vector3 dot2, Vector3 dot3) {
        Vector3 a = dot2 - dot1;
        Vector3 b = dot3 - dot1;
        Vector3 cross = Vector3.Cross(a, b);

        return cross.magnitude / 2.0f;
    }

    static bool checkTriangleInPoint(Vector3 dot1, Vector3 dot2, Vector3 dot3, Vector3 checkPoint) {
        if (dot1 == checkPoint) return false;
        if (dot2 == checkPoint) return false;
        if (dot3 == checkPoint) return false;

        float area = getAreaOfTriangle(dot1, dot2, dot3);
        float dot12 = getAreaOfTriangle(dot1, dot2, checkPoint);
        float dot23 = getAreaOfTriangle(dot2, dot3, checkPoint);
        float dot31 = getAreaOfTriangle(dot3, dot1, checkPoint);

        return (dot12 + dot23 + dot31) <= area + 0.1f;
    }

    static bool CrossCheckAll(List<Vector3> list, int index) {
        Vector3 a = list[index];
        Vector3 b = list[index + 1];
        Vector3 c = list[index + 2];

        for (int i = index + 3; i < list.Count; i++) {
            if (checkTriangleInPoint(a, b, c, list[i]) == true) return true;
        }

        return false;
    }

    static List<Vector3> copy = new List<Vector3>();
    static List<int> indices = new List<int>();

    public static int[] Generate(List<Vector3> vertices) {
        indices.Clear();
        copy.Clear();
        copy.AddRange(vertices);

        int numOfTriangle = vertices.Count - 2;
        for (int i = 0; i < numOfTriangle; i++) {
            for (int k = 0; k < copy.Count - 2; k++) {
                bool ccw = (CCWby2D(copy[k], copy[k + 1], copy[k + 2]) > 0);
                bool cross = CrossCheckAll(copy, k);

                if (ccw == true && cross == false) {
                    indices.Add(k);
                    indices.Add(k + 1);
                    indices.Add(k + 2);
                    //makeTriangle(triangles[i + 1], copy[k], copy[k + 1], copy[k + 2]);
                    copy.RemoveAt(k + 1);

                    break;
                }
            }
        }

        return indices.ToArray();
    }
}
*/