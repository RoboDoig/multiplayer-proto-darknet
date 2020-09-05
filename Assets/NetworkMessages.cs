using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift;

public class NetworkMessages : MonoBehaviour
{
    public class MovementMessage : IDarkRiftSerializable
    {
        Vector3 position;
        Quaternion rotation;
        public MovementMessage(Vector3 _position, Quaternion _rotation) {
            position = _position;
            rotation = _rotation;
        }
        public void Deserialize(DeserializeEvent e)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(position.x);
            e.Writer.Write(position.z);
            e.Writer.Write(rotation.x);
            e.Writer.Write(rotation.y);
            e.Writer.Write(rotation.z);
            e.Writer.Write(rotation.w);
        }
    }
}
