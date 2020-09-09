using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    Camera viewCamera;

    void Start() {
        viewCamera = Camera.main;
    }

    void Update() {
        transform.LookAt(viewCamera.transform);
    }
}
