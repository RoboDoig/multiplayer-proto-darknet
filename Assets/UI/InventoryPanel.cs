using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour, IDropHandler
{
    bool isActive = false;
    public GameObject inventorySlot;

    void Start() {
        gameObject.SetActive(isActive);
    }

    public void ToggleView() {
        isActive = !isActive;
        gameObject.SetActive(isActive);
    }

    public void OnDrop(PointerEventData e) {


        Debug.Log(e.pointerDrag.GetComponent<InventorySlot>().item.itemDefinition.nameID);
        Debug.Log(e.pointerDrag.GetComponent<InventorySlot>().item.amount);

        // From Inventory
        Debug.Log(e.pointerDrag.GetComponentInParent<ItemContainer>());

        // To Inventory
        Debug.Log(GetComponentsInParent<ItemContainer>());
    }

    public void ClearItems() {
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void UpdateItems(ItemContainer itemContainer) {
        ClearItems();

        GameObject obj;
        foreach (Item item in itemContainer.contents) {
            obj = Instantiate(inventorySlot);
            obj.transform.SetParent(transform, false);

            obj.GetComponent<Image>().sprite = item.itemDefinition.icon;
            obj.GetComponent<InventorySlot>().parentInventoryPanel = this;
            obj.GetComponent<InventorySlot>().item = item;
        }
    }
}
