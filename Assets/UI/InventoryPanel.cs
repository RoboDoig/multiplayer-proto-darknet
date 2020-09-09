using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, IDropHandler
{
    bool isActive = false;
    public ItemContainer itemContainer;
    public GameObject inventorySlot;

    void Start() {
        gameObject.SetActive(isActive);
    }

    public void ToggleView() {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void OnDrop(PointerEventData e) {
        Debug.Log(GetComponentInParent<ItemContainer>().gameObject);

        // Item we want to transfer
        Item itemToTransfer = e.pointerDrag.GetComponent<InventorySlot>().item;

        // From Inventory
        ItemContainer fromContainer = e.pointerDrag.GetComponentInParent<ItemContainer>();

        // To Inventory
        ItemContainer toContainer = GetComponentInParent<ItemContainer>();

        // Request Transfer
        if (fromContainer != toContainer)
            fromContainer.TransferItem(itemToTransfer, toContainer);
    }

    public void ClearItems() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void UpdateItems() {
        ClearItems();

        GameObject obj;
        foreach (Item item in itemContainer.contents) {
            if (item.amount > 0) {
                obj = Instantiate(inventorySlot);
                obj.transform.SetParent(transform, false);

                obj.GetComponent<Image>().sprite = item.itemDefinition.icon;
                obj.GetComponent<InventorySlot>().parentInventoryPanel = this;
                obj.GetComponent<InventorySlot>().item = item;
            }
        }
    }
}
