using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    Camera viewCamera;

    void Awake() {
        viewCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = viewCamera;
    }

    void Update() {
        transform.LookAt(viewCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
