using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelController : MonoBehaviour
{
    public List<EnemySpawnInformation> Level1Enemies;
    public List<EnemySpawnInformation> Level2Enemies;    
    public List<EnemySpawnInformation> Level1EnemiesRestart;
    public List<EnemySpawnInformation> Level2EnemiesRestart;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject PlayerRef;
    [SerializeField] GameObject HealthPanel;
    [SerializeField] TMP_Text HealthText;
    [SerializeField] GameObject LevelChangerPanel;
    [SerializeField] TMP_Text GetItem1Text;
    [SerializeField] float PushbackForce;
    [SerializeField] String TimerPrefix;
    [SerializeField] TMP_Text TimerText;
    private bool TimerActive;
    public float TimerAmount;
    public float CurrentTime;
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

        for (int i = 0; i < Level1Enemies.Count; i++)
        {
            Level1EnemiesRestart.Add(Level1Enemies[i]);
        }
        for (int i = 0; i < Level2Enemies.Count; i++)
        {
            Level2EnemiesRestart.Add(Level2Enemies[i]);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = TimerAmount;
        TimerActive = true;
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
        if (Input.GetKeyDown(KeyCode.Space)) { SpawnLevel2Enemies(); }

        if (TimerActive)
        {
            CurrentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(CurrentTime);
            TimerText.text = TimerPrefix + time.Minutes.ToString() + ":" + time.Seconds.ToString();
        }
        if (CurrentTime <= 0) { GameOver(false); }


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
    {//HELP Attemping to make it so Each Enemy Type understands the info it should have on hand. So it can set itself to -
     //be marked as dead upon destruction, without removing itself from the List for spawning. 
     //Currently It fails to know if any of the componets are null or not. And seems to maybe pass over the Death if statement.
     //But Info does appear to be pushed correctly into the Enemy, just not backwards on death.
     //Biggest problem is within the spawn and respawn when reloading the level.
        for (int i = 0; i < Level1Enemies.Count; i++)
        {
            if (Level1Enemies[i].Death == false)
            {
                GameObject enemy = Instantiate(Level1Enemies[i].EnemyPrefab, Level1Enemies[i].SpawnPosition, Level1Enemies[i].SpawnRotation);
                Level1Enemies[i].SpawnIndex = i;
                Level1Enemies[i].LevelIndex = 1;
                Level1Enemies[i].Death = false;
                if (enemy.GetComponent<Regular>().ESI != null) { enemy.GetComponent<Regular>().ESI = Level1Enemies[i]; continue; }
                if (enemy.GetComponent<Boss>().ESI != null) { enemy.GetComponent<Boss>().ESI = Level1Enemies[i]; }
                if (enemy.GetComponent<Objects>().ESI != null) { enemy.GetComponent<Objects>().ESI = Level1Enemies[i]; }
            }
        }
    }

   public void SpawnLevel2Enemies()
    {
        for (int i = 0; i < Level2Enemies.Count; i++)
        {
            if (Level2Enemies[i].Death == false)
            {
                GameObject enemy = Instantiate(Level2Enemies[i].EnemyPrefab, Level2Enemies[i].SpawnPosition, Level2Enemies[i].SpawnRotation);
                enemy.GetComponent<Regular>().ESI = Level2Enemies[i];
                enemy.GetComponent<Boss>().ESI = Level2Enemies[i];
                enemy.GetComponent<Objects>().ESI = Level2Enemies[i];
                Level2Enemies[i].SpawnIndex = i;
                Level2Enemies[i].LevelIndex = 2;
                Level2Enemies[i].Death = false;
            }
        }
    }

    public void EnemyDeathIndexReset(int Level, int Index)
    {
        if (Level == 1)
        {
            Level1Enemies[Index].Death = true;
        }
        else if (Level == 2) { Level2Enemies[Index].Death = true; }
    }

    public void LoadLevel1()
    {

        LevelChangerPanel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Elevator");
        SceneManager.LoadScene(0);
        StartCoroutine(SpawnDelay(0));
        TimerActive = true;
    }

    public void LoadLevel2()
    {
        if (Item1Acquired == true)
        {
            LevelChangerPanel.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Elevator");
            SceneManager.LoadScene(1);
            StartCoroutine(SpawnDelay(1));
            TimerActive = true;
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

    public void GameOver(bool Win)
    {
        Debug.Log(Win);
        if (Win == true)
        {
            SceneManager.LoadScene(2);
        }
        else 
        {
            SceneManager.LoadScene(2);
        }
        TimerActive = false;
        TimerText.text = "";
        ResetEnemyLists();
    }

    private void ResetEnemyLists()
    {
        for (int i = 0; i < Level1EnemiesRestart.Count; i++)
        {
            Level1Enemies.Add(Level1EnemiesRestart[i]);
        }        
        for (int i = 0; i < Level2EnemiesRestart.Count; i++)
        {
            Level2Enemies.Add(Level2EnemiesRestart[i]);
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
        if (Tutorial) { Tutorial = false; TutorialPanel.SetActive(false); PausePanel.SetActive(true); }
        else { Tutorial = true; TutorialPanel.SetActive(true); PausePanel.SetActive(false); }
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

    IEnumerator SpawnDelay(int Level)
    {
        yield return new WaitForSeconds(0.25f);
        if (Level == 0) { SpawnLevel1Enemies(); } else if (Level == 1) { SpawnLevel2Enemies(); }
        FindObjectOfType<PlayerScript>().Damageable = true;
    }
}
