using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class NetworkObjectManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    [SerializeField]
    GameObject objectPrefab;

    void Awake() {
        client.MessageReceived += MessageReceived;
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit)) {
                Debug.Log("Request Spawn Object");
                RequestSpawnObject(hit.point);
            }
        }
    }

    void RequestSpawnObject(Vector3 position) {
        using (DarkRiftWriter writer = DarkRiftWriter.Create()) {
            writer.Write(position.x);
            writer.Write(position.y);
            writer.Write(position.z);

            using (Message message = Message.Create(Tags.PlaceObjectTag, writer))
                client.SendMessage(message, SendMode.Reliable);
        }
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            if (message.Tag == Tags.PlaceObjectTag) {
                using (DarkRiftReader reader = message.GetReader()) {
                    Vector3 objSpawnPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Instantiate(objectPrefab, objSpawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
