using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraPivoter : MonoBehaviour {
    [Header("Input Settings")]
    [SerializeField] private float panSensitivity = 1f;

    private bool panning;
    private Vector3 lastMousePos;

    void Start() {
        panning = false;
    }

    void Update() {
        if (panning) {
            if (Input.GetMouseButton(0)) {
                Vector3 delta = Input.mousePosition - lastMousePos;
                transform.Rotate(new Vector3(0, panSensitivity * delta.x, 0), Space.World);

                lastMousePos = Input.mousePosition;
            }
            else {
                panning = false;
            }
        }
        else if (Input.GetMouseButtonDown(0) && CanPanStart()) {
            panning = true;
            lastMousePos = Input.mousePosition;
        }
    }

    private bool CanPanStart() {
        return !EventSystem.current.IsPointerOverGameObject();
    }
}
