using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    public InventoryPanel parentInventoryPanel;

    float distanceOnDragStart;
    Vector3 startPosition;
    Quaternion startRotation;

    public void OnBeginDrag() {
        distanceOnDragStart = Vector3.Distance(Camera.main.transform.position, transform.position);
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void DuringDrag() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 newPosition = ray.GetPoint(distanceOnDragStart);
        transform.position = newPosition;
        transform.LookAt(Camera.main.transform);
    }

    public void EndDrag() {
        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}
