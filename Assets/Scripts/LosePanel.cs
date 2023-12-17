using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            dead = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        else if(Input.GetKeyDown(KeyCode.M))
        {
            dead = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
          
        }

        if (GameData.health == 0)
            dead = true;

        if (dead== true){
            Time.timeScale = 0;
        }
        if (dead == false)
        {
            Time.timeScale = 1;
        }
    }

   



}
