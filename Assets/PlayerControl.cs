using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    InventoryPanel inventoryPanel;
    Interactable interactTarget;
    Player player;
    public delegate void UpdateAction();
    UpdateAction updateAction;

    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();
        inventoryPanel = GetComponentInChildren<InventoryPanel>();
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
            inventoryPanel.UpdateItems(GetComponent<Inventory>());
            inventoryPanel.ToggleView();
            updateAction = InventoryControl;
        }
    }

    void InventoryControl() {
        if (Input.GetKeyDown(KeyCode.I)) {          
            inventoryPanel.ToggleView();
            updateAction = DefaultControl;
        }
    }

    void Update() {
        updateAction();
    }
}
