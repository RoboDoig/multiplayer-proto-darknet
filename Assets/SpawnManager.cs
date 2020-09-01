using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    void Awake() {
        if (client == null) {
            Debug.Log("Client unassigned in SpawnManager");
            Application.Quit();
        }

        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {

            ushort id = reader.ReadUInt16();

            Debug.Log(id);

        }
    }
}
