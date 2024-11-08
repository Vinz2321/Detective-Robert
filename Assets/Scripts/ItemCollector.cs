using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
   public Image CollectedItemImage;
   private void OnTriggerEnter2D(Collider2D other)
   {
     Debug.Log("Collided with: " + other.name);
    if (other.CompareTag("Evidence"))
    {
        collectItem(other.gameObject);

        Sprite itemSprite = other.GetComponent<SpriteRenderer>().sprite;
        CollectedItemImage.sprite = itemSprite;
        CollectedItemImage.enabled = true;
    }
   }
   private void Start()
   {
        CollectedItemImage.enabled = false;
   }

   private void collectItem(GameObject Item)
   {
      Debug.Log("Item collected");

      Destroy(Item);
   }
}
