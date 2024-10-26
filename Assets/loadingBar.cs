using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class loadingBar : MonoBehaviour
{

    public Image loadBar;
    public float loadingCount = 0f;
    public string sceneName;

    
    void OnWake(){
        loadBar = GetComponent<Image>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Load();
    }

    public void Load(){
       loadBar.fillAmount += loadingCount/1f;

       if (loadBar.fillAmount >= 1f)
        {
            
            SceneManager.LoadScene(sceneName);
        }
    }
}
