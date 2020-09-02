using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;

public class NetworkPlayer : MonoBehaviour
{
    public void SetMovePosition(Vector3 position) {
        transform.position = position;
    }
}
