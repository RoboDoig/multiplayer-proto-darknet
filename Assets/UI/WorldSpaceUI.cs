using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{
    Camera viewCamera;
    public ItemContainer itemContainer;
    public InventoryPanel inventoryPanel;

    void Awake() {
        viewCamera = Camera.main;
        GetComponent<Canvas>().worldCamera = viewCamera;
    }

    void Update() {
        transform.LookAt(viewCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    public void UpdateInventoryItems() {
        inventoryPanel.UpdateItems(itemContainer);
    }

    public void ToggleInventoryView() {
        inventoryPanel.ToggleView();
    }
}
