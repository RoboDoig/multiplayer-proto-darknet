using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "ScriptableObjects/ItemDefinitionTool", order = 1)]
public class ItemDefinitionTool : ItemDefinition
{
    public readonly BaseType baseType = BaseType.Tool;
    public float durability = 100f;
}
