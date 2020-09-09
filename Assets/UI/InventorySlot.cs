using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryPanel parentInventoryPanel;
    public Item item;

    float distanceOnDragStart;
    Vector3 startPosition;
    Quaternion startRotation;

    public void OnBeginDrag(PointerEventData eventData) {
        distanceOnDragStart = Vector3.Distance(Camera.main.transform.position, transform.position);
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void OnDrag(PointerEventData eventData) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 newPosition = ray.GetPoint(distanceOnDragStart);
        transform.position = newPosition;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
