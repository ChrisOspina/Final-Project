using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<CollectibleObject> collectibles;
    public AudioMixer mixer;
    public GameObject pausePanel;
    public GameObject portal;
    public TMP_Text infoText;
    public TMP_Text healthText;
    AudioSource src;

    public string currentLevel;
    public string NextLevel;
    public int health;

    float volume_master = 0;
    float volume_music = 0;
    float volume_sfx = 0;
    float volume_ambient = 0;

    bool mute = false;
    bool pause = false;

    public AudioClip pickupsound;
    public AudioClip dieSound;

    void SaveSettings()
    {
        mixer.GetFloat("volume_master", out volume_master);
        mixer.GetFloat("volume_music", out volume_music);
        mixer.GetFloat("volume_sfx", out volume_sfx);
        mixer.GetFloat("volume_ambient", out volume_ambient);
    }

    void RestoreSettings()
    {
        mixer.SetFloat("volume_master", volume_master);
        mixer.SetFloat("volume_music", volume_music);
        mixer.SetFloat("volume_sfx", volume_sfx);
        mixer.SetFloat("volume_ambient", volume_ambient);
    }


    // Start is called before the first frame update
    void Start()
    {
        SaveSettings();
        GameData.currentLevel = currentLevel;
        src = GetComponent<AudioSource>();
        if (currentLevel != "Tutorial")
        {
            GameData.health = health;
            healthText.SetText("Health: " + GameData.health.ToString());
        }
            

        if (currentLevel == "Tutorial")
        {
            infoText.SetText("Information: Welcome to your summer job at the Seidenberg anthropology lab. For your interview " +
                " find all the missing test tubes");
        }
        else if(currentLevel == "Egypt")
        {
            infoText.SetText("Information: Welcome to Ancient Egypt. To find your way out collect all the vases but" +
                "beware of Pharoah's curse!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mute == false)
            {
                mute = true;
                mixer.SetFloat("volume_master", -80);
            }
            else
            {
                mute = false;
                RestoreSettings();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause == false)
            {
                pausePanel.SetActive(true);
                pause = true;
                Time.timeScale = 0;

                mixer.SetFloat("volume_music", -50);
                mixer.SetFloat("volume_ambient", -80);
                mixer.SetFloat("volume_sfx", -80);
            }
            else
            {
                pause = false;
                pausePanel.SetActive(false);
                RestoreSettings();
                Time.timeScale = 1;
            }
        }
        
    }

    private bool Checkallcollected()
    {
        foreach (CollectibleObject obj in collectibles)
        {
            if (!obj.IsCollected())
            {
                return false;
            }
        }

        return true;
    }

    void Die()
    {
        src.PlayOneShot(dieSound);
        GameData.health -= 10;
        healthText.SetText("Health: " + GameData.health.ToString());
        infoText.SetText("Information: Be careful! messing with time could lead to serious consequences!");
        if (GameData.health == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            src.PlayOneShot(pickupsound);
            if (currentLevel == "Tutorial")
                infoText.SetText("Information: You collected a test tube");
            else if (currentLevel =="Egypt")
                infoText.SetText("Information: You collected a vase. The Ancient Egyptians used this as a fine work of art");
            bool allcollected = Checkallcollected();

            if (allcollected == true)
            {
                portal.SetActive(true);

                if (currentLevel == "Tutorial")
                    infoText.SetText("Information: You collected all the test tubes now the portal is open to begin your real work");
                else {
                    infoText.SetText("Information: The portal is now open");
                }
            }

        }
        else if (other.CompareTag("Portal"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(NextLevel);
        }

        else if (other.CompareTag("ExtraLife"))
        {
            Destroy(other.gameObject);
            GameData.health += 10;
            healthText.SetText("Health: " + GameData.health.ToString());
            infoText.SetText("Information: It's always important to stay hydrated. Gives you free energy!");

        }
        else if (other.CompareTag("AI"))
        {
            Die();
        }
    }


}
