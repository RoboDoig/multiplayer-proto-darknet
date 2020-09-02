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

    [SerializeField]
    NetworkPlayerManager networkPlayerManager;

    [SerializeField]
    GameObject playerPrefab;

    void Awake() {
        if (client == null) {
            Debug.Log("Client unassigned in SpawnManager");
            Application.Quit();
        }

        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            if (message.Tag == Tags.TestMessageTag) {
                using (DarkRiftReader reader = message.GetReader()) {
                ushort id = reader.ReadUInt16();
                Debug.Log(id);
        }
            } else if (message.Tag == Tags.SpawnPlayerTag) {
                SpawnPlayer(sender, e);
            } else if (message.Tag == Tags.DespawnPlayerTag) {
                DespawnPlayer(sender, e);
            }
        }
    }

    void SpawnPlayer(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {
            while (reader.Position < reader.Length) {
                ushort id = reader.ReadUInt16();
                Vector3 position = new Vector3(reader.ReadSingle(), 1f, reader.ReadSingle());

                GameObject obj;
                obj = Instantiate(playerPrefab, position, Quaternion.identity) as GameObject;

                networkPlayerManager.Add(id, obj.GetComponent<NetworkPlayer>());
            }
        }
    }

    void DespawnPlayer(object sender, MessageReceivedEventArgs e) {
        using (Message message  = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader()) {
            networkPlayerManager.DestroyPlayer(reader.ReadUInt16());
        }
    }
}
