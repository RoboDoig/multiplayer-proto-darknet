using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client.Unity;
using DarkRift.Client;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField]
    UnityClient client;

    [SerializeField]
    GameObject defaultContainer;

    void Awake() {
        client.MessageReceived += MessageReceived;
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e) {

    }
}
