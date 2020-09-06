using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "WorldItem", menuName = "ScriptableObjects/WorldItemDefinition", order = 1)]
public class ItemDefinition : ScriptableObject
{
    public string nameID;
    public Sprite icon;
    public enum BaseType {Resource, Consumable, Tool, Weapon, Stat}
    public int maxStack;
    public float value;
    public float weight;
    public List<Item> craftRecipe;
}
