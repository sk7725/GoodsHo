using System.Collections.Generic;
using UnityEngine;

public static class ShapeSmoothing {
    static List<Vector2> tmp = new();

    // Subdivide the edges of the shape
    public static void SubdivideEdges(List<Vector2> points, int subdivisions) {
        tmp.Clear();

        for (int i = 0; i < points.Count; i++) {
            Vector2 p1 = points[i];
            Vector2 p2 = points[(i + 1) % points.Count];

            tmp.Add(p1); // Add the original point

            for (int j = 1; j < subdivisions; j++) {
                float t = (float)j / subdivisions;
                Vector2 interpolatedPoint = Vector2.Lerp(p1, p2, t);
                tmp.Add(interpolatedPoint);
            }
        }

        points.Clear();
        points.AddRange(tmp);
    }

    public static void SmoothCorners(List<Vector2> points, float smoothingFactor, int windowSize) {
        tmp.Clear();

        if (windowSize < 3 || windowSize >= points.Count) {
            throw new System.ArgumentException("Window size must be at least 3 and less than the number of points.");
        }

        for (int i = 0; i < points.Count; i++) {
            Vector2 smoothedPoint = points[i];

            for (int j = 1; j <= windowSize / 2; j++) {
                int prevIndex = (i - j + points.Count) % points.Count;
                int nextIndex = (i + j) % points.Count;

                smoothedPoint += smoothingFactor * (points[prevIndex] + points[nextIndex] - 2 * smoothedPoint);
            }

            tmp.Add(smoothedPoint);
        }

        points.Clear();
        points.AddRange(tmp);
    }

}
