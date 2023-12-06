using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Button selecttutorial;
    public Button selectegypt;
    // Start is called before the first frame update
    void Start()
    {
        Button btn1 = selecttutorial.GetComponent<Button>();
        Button btn2 = selectegypt.GetComponent<Button>();

        btn1.onClick.AddListener(LoadTutorialOnClick);
        btn2.onClick.AddListener(LoadEgyptOnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
    void LoadTutorialOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    void LoadEgyptOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Egypt");
    }

    
}
