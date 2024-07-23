using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<EnemySpawnInformation> Level1Enemies;
    public List<EnemySpawnInformation> Level2Enemies;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject PlayerRef;
    [SerializeField] GameObject HealthPanel;
    [SerializeField] TMP_Text HealthText;
    [SerializeField] GameObject LevelChangerPanel;
    [SerializeField] TMP_Text GetItem1Text;
    private bool LevelChanger;
    public bool Item1Acquired;
    public int MaxHealth;
    public int HealthCarriedBetweenLevels;
    private bool Paused;
    private bool Tutorial;
    private int SceneIndex;
    private bool Pauseable;

    private void Awake()
    {
        LevelController[] objs = FindObjectsOfType<LevelController>();

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        HealthCarriedBetweenLevels = MaxHealth;

        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        LevelChangerPanel.SetActive(false);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        Paused = false;
        Tutorial = false;
        HealthPanel.SetActive(true);
        Pauseable = true;
        SpawnLevel1Enemies();
    }

    // Update is called once per frame
    void Update()
    {
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerRef = FindObjectOfType<PlayerScript>().gameObject;
        Item1Acquired = PlayerRef.GetComponent<PlayerScript>().Item1Equipped;
        //if (SceneIndex == 0 | SceneIndex == 4) { Pauseable = false; }
       // else { Pauseable = true; }
        //if (SceneIndex == 0 | SceneIndex == 4 | Paused == true) { HealthPanel.SetActive(false); }
        //else { HealthPanel.SetActive(true); }
        if (Input.GetKeyDown(KeyCode.Escape) && Pauseable == true)
        {
            if (Paused == true)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        HealthText.text = HealthCarriedBetweenLevels.ToString();
    }

    public void SpawnLevel1Enemies()
    {
        for (int i = 0; i < Level1Enemies.Count; i++)
        {
            if (Level1Enemies[i].Death == false)
            { 
                Instantiate(Level1Enemies[i].EnemyPrefab, Level1Enemies[i].SpawnPosition, Level1Enemies[i].SpawnRotation);
                Level1Enemies[i].SpawnIndex = i;
                Level1Enemies[i].LevelIndex = 1;
                Level1Enemies[i].Death = false;
            }
        }
    }

   public void SpawnLevel2Enemies()
    {
        for (int i = 0; i < Level2Enemies.Count; i++)
        {
            if (Level2Enemies[i].Death == false)
            {
                Instantiate(Level2Enemies[i].EnemyPrefab, Level2Enemies[i].SpawnPosition, Level2Enemies[i].SpawnRotation);
                Level2Enemies[i].SpawnIndex = i;
                Level2Enemies[i].LevelIndex = 2;
                Level2Enemies[i].Death = false;
            }
        }
    }

    public void LoadLevel1()
    {
        LevelChangerPanel.SetActive(false);
        SceneManager.LoadScene(1);
        FindObjectOfType<AudioManager>().Play("Elevator");
        SceneManager.LoadScene(0);
    }

    public void LoadLevel2()
    {
        if (Item1Acquired == true)
        {
            GetItem1Text.enabled = false;
            LevelChangerPanel.SetActive(false);
            SceneManager.LoadScene(1);
        }
        else { GetItem1Text.enabled = true; }
    }

    public void EnemyDeath(int Level, int Index)
    {
        if (Level == 1)
        {
            Level1Enemies[Index].Death = true;
        }
        else if (Level == 2)
        {
            Level2Enemies[Index].Death= true;
        }
    }

    public void LevelChangerActive()
    {
            LevelChangerPanel.SetActive(true);
            GetItem1Text.enabled = false;
    }    

    public void LevelChangerInActive()
    {
        GetItem1Text.enabled = false;
        LevelChangerPanel.SetActive(false);
    }

    public void Pause()
    {
        HealthPanel.SetActive(false);
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        Paused = true;
    }

    public void Resume()
    {
        PausePanel.SetActive(false);
        HealthPanel.SetActive(true);
        Time.timeScale = 1;
        Paused = false;
    }
    public void TutorialTab()
    {
        if (Tutorial) { Tutorial = false; TutorialPanel.SetActive(false); }
        else { Tutorial = true; TutorialPanel.SetActive(true); }
    }

    public void MainMenu()
    {
        Resume();
        SceneManager.LoadScene("MainMenu");
        HealthCarriedBetweenLevels = MaxHealth;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
