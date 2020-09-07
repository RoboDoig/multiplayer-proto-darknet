using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class Player : MonoBehaviour
{
    [SerializeField]
    float moveDistance = 0.05f;

    public UnityClient Client;
    Vector3 lastPosition;
    Quaternion lastRotation;

    void Awake() {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update() {
        if (Vector3.Distance(lastPosition, transform.position) > moveDistance && Client != null) {
            Message movMessage = Message.Create(Tags.MovePlayerTag,
                new NetworkMessages.MovementMessage(transform.position, transform.rotation));

            Client.SendMessage(movMessage, SendMode.Unreliable);

            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }
    }
}
