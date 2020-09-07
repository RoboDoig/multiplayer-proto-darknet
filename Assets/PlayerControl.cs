using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Interactable interactTarget;
    Player player;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();
    }

    void Update() {
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
    }
}
