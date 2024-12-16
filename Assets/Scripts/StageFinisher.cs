using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageFinisher : MonoBehaviour

{
    public string SceneTitle;
    public float DelayTime;
    void OnTriggerEnter2D (Collider2D other)
    {
    if(other.CompareTag("Player"))
    {
       StartCoroutine(EndGame());
    }
    }
   IEnumerator EndGame(){
    yield return new  WaitForSeconds(DelayTime);

    SceneManager.LoadScene(SceneTitle);
   }
}
