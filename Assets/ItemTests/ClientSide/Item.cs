using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public ItemDefinition itemDefinition;
    public int amount;

    public Item (ItemDefinition _itemDefinition, int _amount) {
        itemDefinition = _itemDefinition;
        amount = _amount;
    }
}
