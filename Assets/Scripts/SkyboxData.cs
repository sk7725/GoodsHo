using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkyboxData", menuName = "GoodsHo/Skybox Data")]
public class SkyboxData : ScriptableObject {
    public Material material;
    public Color lightColor = Color.white;
    public float lightTemperature = 5000;

    public string displayName = "Skybox";
}