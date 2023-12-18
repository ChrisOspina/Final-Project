using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LosePanel : MonoBehaviour
{
    public bool dead;
    public AudioMixer mixer;

    //float volume_master = 0;

    private void Awake()
    {
        dead = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            dead = false;
            GameData.health = 200;
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        else if(Input.GetKeyDown(KeyCode.M))
        {
            dead = false;
            GameData.health = 200;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            mixer.SetFloat("volume_master", 0);

          
        }

        if (GameData.health == 0)
            dead = true;

        if (dead== true){
            Time.timeScale = 0;
        }
    }

   



}
