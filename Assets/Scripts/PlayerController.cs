using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public List<CollectibleObject> collectibles;
    public AudioMixer mixer;
    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject portal;

    public TMP_Text infoText;
    public TMP_Text healthText;
    public TMP_Text countText;

    AudioSource src;

    public int currentLevel;
    public string NextLevel;
    public int health;
    public int collectCount;
    public int collectTotal;

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
    public AudioClip gameOver;

    public ParticleSystem poison;
    public ParticleSystem burn;


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
        if (poison)
        {
            poison = poison.GetComponent<ParticleSystem>();
            poison.Pause();
        }
           
        if (burn)
        {
            burn = burn.GetComponent<ParticleSystem>();
            burn.Pause();
        }
            

        if (currentLevel > 0)
        {
            GameData.health = health;
            healthText.SetText("Health: " + GameData.health.ToString());
        }  

        if (currentLevel == 0)
        {
            infoText.SetText("<mark>Welcome to your summer job at Seidenberg Labs. Find all the missing test tubes and avoid our robot.</mark>");
        }
        else if(currentLevel == 1)
        {
            infoText.SetText("<mark>Welcome to Ancient Egypt. To find your way out collect all the vases but" +
                "beware of Pharoah's curse!</mark>");
        }
        else if(currentLevel == 2)
        {
            infoText.SetText("<mark>How often do you think of the Roman Empire? We sent you now to the Great Fire of Rome" +
               " where the Emperor's soldiers are on the hunt for traitors. Collect all the coins to escape. Also watch out for flames</mark>");
        }
        else if (currentLevel == 3)
        {
            infoText.SetText("<mark>Welcometh to the Middle Ages. To progresseth findeth the legendary Excalibur but beware of skeletons from King Arthur's Dungeon</mark>");
        }
    }

    private void Awake()
    {
        collectCount = 0;
        collectTotal = collectibles.Count;
        countText.SetText(collectCount.ToString() + " of " + collectTotal.ToString() + " items.");
        portal.SetActive(false);
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
        if(currentLevel==3)
            infoText.SetText("<mark>Beest careful! messing with time couldst leadeth to s'rious consequences!</mark>");
        else
            infoText.SetText("<mark>Be careful! messing with time could lead to serious consequences!</mark>");
        if (GameData.health == 0)
        {
            losePanel.SetActive(true);
            src.PlayOneShot(gameOver);
        }
    }
    void Burn()
    {
        
        StartCoroutine(ApplyBurnOverTime());
        infoText.SetText("<mark>Yow! that must burn. Don't play with fire!</mark>");
        if (GameData.health == 0)
        {
            losePanel.SetActive(true);
            src.PlayOneShot(gameOver);
        }
        burn.Pause();
    }
    void CollectItem()
    {
        collectCount += 1;
        countText.SetText(collectCount.ToString() + " of " + collectTotal.ToString() + " items.");

        if (currentLevel == 0)
            infoText.SetText("<mark>You collected a test tube</mark>");
        else if (currentLevel == 1)
            infoText.SetText("<mark>You collected a vase. The Ancient Egyptians used this as a fine work of art.</mark>");
        else if (currentLevel == 2)
            infoText.SetText("<mark>You collected a coin. The Romans first used money as we know it today.</mark>");
        bool allcollected = Checkallcollected();

        if (allcollected == true)
        {
            portal.SetActive(true);

            if (currentLevel == 0)
                infoText.SetText("<mark>You collected all the test tubes now the portal is open to begin your real work.</mark>");
            else if (currentLevel == 3)
                infoText.SetText("Thou hath found the legendary sw'rd of King Arthur. Just what we needeth f'r researcheth anon the p'rtal is open by the Church");
            else
            {
                infoText.SetText("<mark>The portal is now open.</mark>");
            }
        }
    }
    void Eat()
    {
        src.PlayOneShot(eatSound);
        if (currentLevel == 0)
            infoText.SetText("<mark>Be sure to also be on the lookout for foods throughout your journey. They give you energy.</mark>");
        else if (currentLevel == 3)
        {
            infoText.SetText("Consume up f'r free en'rgy!");
            GameData.health += 20;
            healthText.SetText("Health: " + GameData.health.ToString());
        }
        else
        {
            infoText.SetText("<mark>Eat up for free energy!</mark>");
            GameData.health += 20;
            healthText.SetText("Health: " + GameData.health.ToString());
        }   
    }

    void Drink()
    {
        src.PlayOneShot(drinkSound);
        
        if (currentLevel == 0)
            infoText.SetText("<mark>This is an energy drink. You will find some more throughout your journey</mark>");
        else if(currentLevel == 3)
        {
            infoText.SetText("<mark>'Tis at each moment imp'rtant to stayeth hydrat'd.  Giveth thou free en'rgy!</mark>");
            GameData.health += 10;
            healthText.SetText("Health: " + GameData.health.ToString());
        }

        else
        {
            infoText.SetText("<mark>It's always important to stay hydrated. Gives you free energy!</mark>");
            GameData.health += 10;
            healthText.SetText("Health: " + GameData.health.ToString());
        }
    }

    void PoisonDrink()
    {
        src.PlayOneShot(drinkSound);
        infoText.SetText("Beest careful some drinks may actually beest poisonous");
        StartCoroutine(ApplyPoisonOverTime());
        poison.Pause();
    }

    IEnumerator ApplyPoisonOverTime()
    {
        for (int i = 0; i < 5; i++)
        {
            GameData.health -= 5;
            src.PlayOneShot(dieSound);
            healthText.SetText("Health: " + GameData.health.ToString());

            poison.Play();
           // Debug.Log(poison.isPlaying);

            // Wait for one second before the next iteration
            yield return new WaitForSeconds(1.0f);
        }
        poison.Stop();
    }

    IEnumerator ApplyBurnOverTime()
    {
        for (int i = 0; i < 3; i++)
        {
            GameData.health -= 10;
            src.PlayOneShot(dieSound);
            healthText.SetText("Health: " + GameData.health.ToString());

            burn.Play();

            // Wait for one second before the next iteration
            yield return new WaitForSeconds(1.0f);
        }
        burn.Stop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Destroy(other.gameObject);
            src.PlayOneShot(pickupsound);
            CollectItem();
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

        else if (other.CompareTag("PoisonDrink"))
        {
            Destroy(other.gameObject);
            PoisonDrink();
        }
        else if (other.CompareTag("AI"))
        {
            if (currentLevel == 0)
                infoText.SetText("<mark>Beware of our test robot</mark>");
            else
                Die();
        }
        else if (other.CompareTag("Fire"))
        {
            Burn();
        }
        else if (other.CompareTag("priest"))
        {
            if (GameData.health < 200)
            {
                GameData.health = 200;
                healthText.SetText("Health: " + GameData.health.ToString());
            }
            infoText.SetText("This is a catholic preacheth'r.  God blesseth thy soul");
        }
    }


}
