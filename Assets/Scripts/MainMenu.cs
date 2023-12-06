using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Button startbtn;
    public Button selectbtn;
    // Start is called before the first frame update
    void Start()
    {
        Button btn1 = startbtn.GetComponent<Button>();
        Button btn2 = selectbtn.GetComponent<Button>();

        btn1.onClick.AddListener(StartOnClick);
        btn2.onClick.AddListener(SelectOnClick);
    }

    void StartOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    void SelectOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelect");
    }
}
