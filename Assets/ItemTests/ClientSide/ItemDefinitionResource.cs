using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldItem", menuName = "ScriptableObjects/ItemDefinitionResource", order = 1)]
public class ItemDefinitionResource : ItemDefinition
{
    public readonly BaseType baseType = BaseType.Resource;
}
