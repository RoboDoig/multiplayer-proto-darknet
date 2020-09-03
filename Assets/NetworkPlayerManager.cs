using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift.Client.Unity;
using DarkRift.Client;
using DarkRift;

public class NetworkPlayerManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    Dictionary<ushort, NetworkPlayer> networkPlayers = new Dictionary<ushort, NetworkPlayer>();

    public void Awake() {
        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        using (Message message = e.GetMessage() as Message) {
            if (message.Tag == Tags.MovePlayerTag) {
                using (DarkRiftReader reader = message.GetReader()) {
                    ushort id = reader.ReadUInt16();

                    Vector3 newPosition = new Vector3(reader.ReadSingle(), 1f, reader.ReadSingle());
                    Quaternion newRotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                    if (networkPlayers.ContainsKey(id)) {
                        networkPlayers[id].SetMovePosition(newPosition);
                        networkPlayers[id].SetRotation(newRotation);
                    }
                }
            }
        }
    }

    public void Add(ushort id, NetworkPlayer player) {
        networkPlayers.Add(id, player);
    }

    public void DestroyPlayer(ushort id) {
        NetworkPlayer p = networkPlayers[id];
        Destroy(p.gameObject);
        networkPlayers.Remove(id);
    }
}
