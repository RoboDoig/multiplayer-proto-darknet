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
        if (Vector3.Distance(lastPosition, transform.position) > moveDistance) {
            using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
                writer.Write(transform.position.x);
                writer.Write(transform.position.z);
                writer.Write(transform.rotation.x);
                writer.Write(transform.rotation.y);
                writer.Write(transform.rotation.z);
                writer.Write(transform.rotation.w);

                using (Message message = Message.Create(Tags.MovePlayerTag, writer))
                    Client.SendMessage(message, SendMode.Unreliable);
            }
            lastPosition = transform.position;
            lastRotation = transform.rotation;
        }
    }
}
