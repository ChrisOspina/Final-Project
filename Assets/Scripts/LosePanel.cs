using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    public bool dead;

    private void Awake()
    {
        dead = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        else if(Input.GetKeyDown(KeyCode.M))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        if (GameData.health == 0)
            dead = true;

        if (dead== true){
            Time.timeScale = 0;
        }
    }



}
