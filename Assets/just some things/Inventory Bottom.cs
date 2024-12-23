using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBottom : MonoBehaviour
{
    public List<string> items = new List<string>();

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("Added to inventory: " + itemName);
    }
}


