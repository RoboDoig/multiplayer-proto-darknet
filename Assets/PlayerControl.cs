using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    PlayerUIControl playerUIControl;
    Interactable interactTarget;
    Player player;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();
        playerUIControl = GetComponentInChildren<PlayerUIControl>();
    }

    void Update() {
        // Movement control
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                navMeshAgent.SetDestination(hit.point);
                if (hit.transform.GetComponent<Interactable>()) {
                    interactTarget = hit.transform.GetComponent<Interactable>();
                }
            }
        } 

        if (interactTarget != null) {
            if (Vector3.Distance(transform.position, interactTarget.transform.position) < 3f) {
                interactTarget.OnInteract(player);
                interactTarget = null;
            }
        }

        // UI Control
        if (Input.GetKeyDown(KeyCode.I)) {
            playerUIControl.ToggleView();
        }
    }
}
