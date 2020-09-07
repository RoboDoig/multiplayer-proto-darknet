using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject defaultContainer;

    // Start is called before the first frame update
    void Start()
    {
        // SpawnItem("item.resource.wood", 2, new Vector3(0f, 0f, 0f));
        // SpawnItem("item.tool.hammer", 1, new Vector3(-5f, 0f, 5f));
    }

    void SpawnItem(string item, int amount, Vector3 placePosition) {
        GameObject obj = Instantiate(defaultContainer, placePosition, Quaternion.identity) as GameObject;

        obj.GetComponent<ItemContainer>().contents.Add(new Item(ItemManager.allItems[item], amount));
    }
}
