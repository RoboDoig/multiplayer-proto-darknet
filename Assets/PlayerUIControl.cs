using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{

    bool isActive = false;

    void Start() {
        gameObject.SetActive(isActive);
    }
    
    public void ToggleView() {
        if (isActive) {
            isActive = false;
        } else {
            isActive = true;
        }

        gameObject.SetActive(isActive);
    }

}
