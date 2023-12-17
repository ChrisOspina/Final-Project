using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown || Input.GetButtonDown("Jump"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
        }
    }
}
