using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkRift.Client.Unity;
using DarkRift;

[Serializable]
public class Item: IDarkRiftSerializable
{
    public ItemDefinition itemDefinition;
    public int amount;

    public Item (ItemDefinition _itemDefinition, int _amount) {
        itemDefinition = _itemDefinition;
        amount = _amount;
    }

    // Network code
    public void OnItemChanged(UnityClient client) {

    }

    public void Serialize(SerializeEvent e)
    {
        throw new NotImplementedException();
    }

    public void Deserialize(DeserializeEvent e)
    {
        throw new NotImplementedException();
    }
}
