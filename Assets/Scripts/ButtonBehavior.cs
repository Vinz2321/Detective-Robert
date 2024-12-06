using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonBehavior : MonoBehaviour
{
   public void LoadScene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
