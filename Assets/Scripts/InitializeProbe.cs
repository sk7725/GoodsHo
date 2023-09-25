using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeProbe : MonoBehaviour
{
    [SerializeField] private ReflectionProbe probe;
    void Awake()
    {
        probe.RenderProbe();
    }
}
