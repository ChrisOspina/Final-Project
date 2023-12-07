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
    public GameObject playerWeapon;
    public GameObject portal;
    public TMP_Text infoText;
    public TMP_Text healthText;
    AudioSource src;

    public int currentLevel;
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
    public AudioClip eatSound;
    public AudioClip drinkSound;


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
        src = GetComponent<AudioSource>();
        if (currentLevel > 0)
        {
            GameData.health = health;
            healthText.SetText("Health: " + GameData.health.ToString());
        }
            

        if (currentLevel == 0)
        {
            infoText.SetText("Information: Welcome to your summer job at the Seidenberg anthropology lab. For your interview " +
                " find all the missing test tubes");
        }
        else if(currentLevel == 1)
        {
            infoText.SetText("Information: Welcome to Ancient Egypt. To find your way out collect all the vases but" +
                "beware of Pharoah's curse!");
        }
        else if(currentLevel == 2)
        {
            infoText.SetText("Information: How often do you think of the Roman Empire? We sent you now to the Great Fire of Rome" +
               " where the Emperor's soldiers are on the hunt for traitors. Collect all the coins to escape. Also watch out for flames");
        }
    }

    private void Awake()
    {
        if(currentLevel >1)
            playerWeapon.SetActive(false);
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
    void Burn()
    {
        src.PlayOneShot(dieSound);
        GameData.health -= 10;
        healthText.SetText("Health: " + GameData.health.ToString());
        infoText.SetText("Information: Yow! that must burn. Don't play with fire!");
        if (GameData.health == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
        }
    }
    void Eat()
    {
        src.PlayOneShot(eatSound);
        GameData.health += 20;
        healthText.SetText("Health: " + GameData.health.ToString());
        infoText.SetText("Information: Eat up for free energy!");
    }
    void Drink()
    {
        src.PlayOneShot(drinkSound);
        GameData.health += 10;
        healthText.SetText("Health: " + GameData.health.ToString());
        infoText.SetText("Information: It's always important to stay hydrated. Gives you free energy!");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            src.PlayOneShot(pickupsound);
            if (currentLevel == 0)
                infoText.SetText("Information: You collected a test tube");
            else if (currentLevel ==1)
                infoText.SetText("Information: You collected a vase. The Ancient Egyptians used this as a fine work of art");
            else if(currentLevel == 2)
                infoText.SetText("Information: You collected a coin. The Romans first used money as we know it today.");
            bool allcollected = Checkallcollected();

            if (allcollected == true)
            {
                portal.SetActive(true);

                if (currentLevel == 0)
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

        else if (other.CompareTag("Drink"))
        {
            Destroy(other.gameObject);
            Drink();
        }

        else if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            Eat();
        }

        else if (other.CompareTag("PickupSword"))
        {
            infoText.SetText("Information: You picked up a weapon. This will protect you");
            Destroy(other.gameObject);
            playerWeapon.SetActive(true);
        }
        else if (other.CompareTag("AI"))
        {
            Die();
        }
        else if (other.CompareTag("Fire"))
        {
            Burn();
        }
    }


}
