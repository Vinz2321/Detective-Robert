using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
   private void OnTriggerEnter2D(Collider2D other)
   {
     Debug.Log("Collided with: " + other.name);
    if (other.CompareTag("Collectible"))
    {
        collectItem(other.gameObject);
    }
   }

   private void collectItem(GameObject Item)
   {
      Debug.Log("Item collected");

      Destroy(Item);
   }
}
