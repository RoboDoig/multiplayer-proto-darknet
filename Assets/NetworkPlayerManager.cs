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

    public void Add(ushort id, NetworkPlayer player) {
        networkPlayers.Add(id, player);
    }

    public void DestroyPlayer(ushort id) {
        NetworkPlayer p = networkPlayers[id];
        Destroy(p.gameObject);
        networkPlayers.Remove(id);
    }
}
