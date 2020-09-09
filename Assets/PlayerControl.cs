using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    Collider col;
    WorldSpaceUI ui;
    Interactable interactTarget;
    Player player;
    List<WorldSpaceUI> openUI = new List<WorldSpaceUI>();
    public delegate void UpdateAction();
    UpdateAction updateAction;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        col = GetComponent<Collider>();
        player = GetComponent<Player>();
        ui = GetComponentInChildren<WorldSpaceUI>();
        updateAction = DefaultControl;
    }

    void DefaultControl() {
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
            navMeshAgent.SetDestination(transform.position);
            ui.UpdateInventoryItems();

            // Show players's inventory
            ui.ToggleInventoryView();

            // Show any nearby inventories
            openUI.Clear();
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach(Collider colliderOther in colliders) {
                if (colliderOther != col) {
                    WorldSpaceUI objectUI = colliderOther.transform.GetComponentInChildren<WorldSpaceUI>();
                    if (objectUI != null) {
                        objectUI.ToggleInventoryView();
                        objectUI.UpdateInventoryItems();
                        openUI.Add(objectUI);
                    }
                }
            }

            updateAction = InventoryControl;
        }
    }

    void InventoryControl() {
        if (Input.GetKeyDown(KeyCode.I)) {          
            ui.ToggleInventoryView();

            foreach (WorldSpaceUI objectUI in openUI) {
                objectUI.ToggleInventoryView();
            }

            updateAction = DefaultControl;
        }
    }

    void Update() {
        updateAction();
    }
}
