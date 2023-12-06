using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    public Button Retrybtn;
    public Button Menubtn;
    private void Start()
    {
        Button btn1 = Retrybtn.GetComponent<Button>();
        Button btn2 = Menubtn.GetComponent<Button>();

        btn1.onClick.AddListener(retryOnClick);
        btn2.onClick.AddListener(menuOnClick);
    }

    void retryOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(GameData.currentLevel);
    }

    void menuOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
