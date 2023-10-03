using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AcrylSettings", menuName = "GoodsHo/Acryl Gen Settings")]
public class Outliner : ScriptableObject
{
    [Header("Contour Tracing")]
    [Tooltip("How much difference in pixels in a straight line is considered a gap. This can help smooth out the outline a bit.")]
    [Min(1)] public uint gapLength = 3;
    [Tooltip("Product for optimizing the outline based on angle. 1 means no optimization. This value should be kept pretty high if you want to maintain round shapes. Note that some points (e.g. outer angles) are never optimized.")]
    [Range(0f, 1f)] public float product = 0.99f;
    public float tolerance = 0.02f;

    [Header("Gaussian Outline")]
    public float alphaCutoff = 0.1f;

    public int outlineIterations = 3;
    public int downscaleFactor = 10;
    public int outlinePadding = 5;

    [Header("Smoothing")]
    public int subdivisions = 1;
    public float smoothingFactor = 0.2f;
    public int smoothingWindow = 5;
    public float finalTolerance = 0.005f;

    [Header("Mesh Generation")]
    public float thickness = 0.02f;
    [Range(0f, 1f)] public float bevel = 0.08f;
}