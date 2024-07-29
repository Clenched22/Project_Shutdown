using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
//using Unity.PlasticSCM.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelController : MonoBehaviour
{
    public List<EnemySpawnInformation> Level1Enemies;
    public List<EnemySpawnInformation> Level2Enemies;
    public List<EnemySpawnInformation> Level3Enemies;
    public List<EnemySpawnInformation> Level4Enemies;
    public List<EnemySpawnInformation> Level1EnemiesRestart;
    public List<EnemySpawnInformation> Level2EnemiesRestart;
    public List<EnemySpawnInformation> Level3EnemiesRestart;
    public List<EnemySpawnInformation> Level4EnemiesRestart;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject TutorialPanel;
    [SerializeField] GameObject PlayerRef;
    [SerializeField] GameObject HealthPanel;
    [SerializeField] TMP_Text HealthText;
    [SerializeField] GameObject LevelChangerPanel;
    [SerializeField] TMP_Text GetItemText;
    [SerializeField] string GetScrewDriverText;
    [SerializeField] string GetKeyCardText;
    [SerializeField] string GetWireCutterText;
    [SerializeField] float PushbackForce;
    [SerializeField] String TimerPrefix;
    [SerializeField] TMP_Text TimerText;
    [SerializeField] GameObject TutorialTextPanel;
    [SerializeField] TMP_Text TutorialTextRef;
    [SerializeField] string TutorialText1;
    [SerializeField] string TutorialText2;
    [SerializeField] string TutorialText3;
    [SerializeField] string TutorialText4;
    [SerializeField] string TutorialText5;
    [SerializeField] string TutorialText6;
    [SerializeField] string TutorialText7;
    [SerializeField] string TutorialText8;
    private float WhichStringToShow;
    public bool TutorialActive;
    private bool TimerActive;
    public float TimerAmount;
    public float CurrentTime;
    private bool LevelChanger;
    public bool ScrewDriverAcquired;
    public bool KeyCardAcquired;
    public bool WireCutterAcquired;
    public bool PistolAcquired;
    public bool ARAcquired;
    public bool SniperAcquired;
    public float MaxHealth;
    public float HealthCarriedBetweenLevels;
    public bool Paused;
    private bool Tutorial;
    private int SceneIndex;
    private bool Pauseable;
    public bool MainBGPlaying;
    public bool Win;


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
        TimerActive = false;
        LevelChangerPanel.SetActive(false);
        SceneIndex = SceneManager.GetActiveScene().buildIndex;
        Paused = false;
        Tutorial = false;
        HealthPanel.SetActive(false);
        Pauseable = false;
        TutorialTextPanel.SetActive(false);
        TutorialActive = false;
        PistolAcquired = true;
        ARAcquired = false;
        SniperAcquired = false;
        FindObjectOfType<AudioManager>().Play("MainBG");
        MainBGPlaying = true;
        Level1EnemiesRestart = Level1Enemies;
        Level2EnemiesRestart= Level2Enemies;
        Level3EnemiesRestart = Level3Enemies;
        Level4EnemiesRestart = Level4Enemies;
    }

    public void ActualStart()
    {
        StartCoroutine(ActualStartDelay());
    }

    IEnumerator ActualStartDelay()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        TutorialTextPanel.SetActive(true);
        TutorialActive = true;
        WhichStringToShow = 1;
        SpawnLevel1Enemies();
        TutorialStringSelector();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && TutorialActive) { WhichStringToShow++; TutorialStringSelector(); }

        if (TimerActive)
        {
            CurrentTime -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(CurrentTime);
            if (CurrentTime < 60)
            {
                TimerText.text = TimerPrefix + "0" + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
            }
            else
            {
                TimerText.text = TimerPrefix + time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString();
            }
        }
        if (CurrentTime <= 0) { Win = false; GameOver(); }


        SceneIndex = SceneManager.GetActiveScene().buildIndex;

        var playerScript = FindObjectOfType<PlayerScript>();

        if (playerScript != null)
        {
            if (ScrewDriverAcquired != true)
            { ScrewDriverAcquired = playerScript.ScrewDriver; }
            if (KeyCardAcquired != true)
            { KeyCardAcquired = playerScript.KeyCard; }
            if (WireCutterAcquired != true)
            { WireCutterAcquired = playerScript.WireCutter; }
            if (ARAcquired != true)
            { ARAcquired = playerScript.ARAccquired; }
            if (SniperAcquired != true)
            { SniperAcquired = playerScript.SniperAccquired; }
        }

        if (SceneIndex == 0 | SceneIndex == 5) { Pauseable = false; }
         else { Pauseable = true; }
        if (SceneIndex == 0 | SceneIndex == 5 | Paused == true) { HealthPanel.SetActive(false); }
        else { HealthPanel.SetActive(true); }
        if (Input.GetKeyDown(KeyCode.Escape) && Pauseable == true && TutorialActive == false)
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
                GameObject enemy = Instantiate(Level1Enemies[i].EnemyPrefab, Level1Enemies[i].SpawnPosition, Level1Enemies[i].SpawnRotation);
                Level1Enemies[i].SpawnIndex = i;
                Level1Enemies[i].LevelIndex = 1;
                Level1Enemies[i].Death = false;

                Regular regularComp = enemy.GetComponent<Regular>();
                if ( regularComp != null) 
                {
                    enemy.GetComponent<Regular>().ESI = Level1Enemies[i];
                    continue;
                }

                Boss bossComp = enemy.GetComponent<Boss>();
                if (bossComp != null) 
                { 
                    enemy.GetComponent<Boss>().ESI = Level1Enemies[i];
                    continue;
                }

                Projectile projectile = enemy.GetComponent<Projectile>();
                if (projectile != null)
                {
                    enemy.GetComponent<Projectile>().ESI = Level1Enemies[i];
                    continue;
                }

                Objects objectComp = enemy.GetComponent<Objects>();
                if ( objectComp != null) 
                { 
                    enemy.GetComponent<Objects>().ESI = Level1Enemies[i];
                    continue;
                }
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
                Level2Enemies[i].SpawnIndex = i;
                Level2Enemies[i].LevelIndex = 2;
                Level2Enemies[i].Death = false;

                Regular regularComp = enemy.GetComponent<Regular>();
                if (regularComp != null)
                {
                    enemy.GetComponent<Regular>().ESI = Level2Enemies[i];
                    continue;
                }

                Boss bossComp = enemy.GetComponent<Boss>();
                if (bossComp != null)
                {
                    enemy.GetComponent<Boss>().ESI = Level2Enemies[i];
                    continue;
                }

                Projectile projectile = enemy.GetComponent<Projectile>();
                if (projectile != null)
                {
                    enemy.GetComponent<Projectile>().ESI = Level2Enemies[i];
                    continue;
                }

                Objects objectComp = enemy.GetComponent<Objects>();
                if (objectComp != null)
                {
                    enemy.GetComponent<Objects>().ESI = Level2Enemies[i];
                    continue;
                }
            }
        }
    }

    public void SpawnLevel3Enemies()
    {
        for (int i = 0; i < Level3Enemies.Count; i++)
        {
            if (Level3Enemies[i].Death == false)
            {
                GameObject enemy = Instantiate(Level3Enemies[i].EnemyPrefab, Level3Enemies[i].SpawnPosition, Level3Enemies[i].SpawnRotation);
                Level3Enemies[i].SpawnIndex = i;
                Level3Enemies[i].LevelIndex = 3;
                Level3Enemies[i].Death = false;

                Regular regularComp = enemy.GetComponent<Regular>();
                if (regularComp != null)
                {
                    enemy.GetComponent<Regular>().ESI = Level3Enemies[i];
                    continue;
                }

                Boss bossComp = enemy.GetComponent<Boss>();
                if (bossComp != null)
                {
                    enemy.GetComponent<Boss>().ESI = Level3Enemies[i];
                    continue;
                }

                Projectile projectile = enemy.GetComponent<Projectile>();
                if (projectile != null)
                {
                    enemy.GetComponent<Projectile>().ESI = Level3Enemies[i];
                    continue;
                }

                Objects objectComp = enemy.GetComponent<Objects>();
                if (objectComp != null)
                {
                    enemy.GetComponent<Objects>().ESI = Level3Enemies[i];
                    continue;
                }
            }
        }
    }

    public void SpawnLevel4Enemies()
    {
        for (int i = 0; i < Level4Enemies.Count; i++)
        {
            if (Level4Enemies[i].Death == false)
            {
                GameObject enemy = Instantiate(Level4Enemies[i].EnemyPrefab, Level4Enemies[i].SpawnPosition, Level4Enemies[i].SpawnRotation);
                Level4Enemies[i].SpawnIndex = i;
                Level4Enemies[i].LevelIndex = 4;
                Level4Enemies[i].Death = false;

                Regular regularComp = enemy.GetComponent<Regular>();
                if (regularComp != null)
                {
                    enemy.GetComponent<Regular>().ESI = Level4Enemies[i];
                    continue;
                }

                Boss bossComp = enemy.GetComponent<Boss>();
                if (bossComp != null)
                {
                    enemy.GetComponent<Boss>().ESI = Level4Enemies[i];
                    continue;
                }

                Projectile projectile = enemy.GetComponent<Projectile>();
                if (projectile != null)
                {
                    enemy.GetComponent<Projectile>().ESI = Level4Enemies[i];
                    continue;
                }

                Objects objectComp = enemy.GetComponent<Objects>();
                if (objectComp != null)
                {
                    enemy.GetComponent<Objects>().ESI = Level4Enemies[i];
                    continue;
                }
            }
        }
    }

    public void EnemyDeathIndexReset(int Level, int Index)
    {
        switch (Level)
        {
            case 1:
                Level1Enemies[Index].Death = true;
                break;

            case 2:
                Level2Enemies[Index].Death = true;
                break;
            case 3:
                Level3Enemies[Index].Death = true;
                break;
            case 4:
                Level4Enemies[Index].Death = true;
                break;
        }
    }

    public void LoadLevel1()
    {

        LevelChangerPanel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Elevator");
        SceneManager.LoadScene(1);
        StartCoroutine(SpawnDelay(1));
        TimerActive = true;
        if (MainBGPlaying != true)
        {
            FindObjectOfType<AudioManager>().Play("MainBG");
            FindObjectOfType<AudioManager>().Stop("BossBG");
        }
    }

    public void LoadLevel2()
    {
        if (ScrewDriverAcquired == true)
        {
            LevelChangerPanel.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Elevator");
            SceneManager.LoadScene(2);
            StartCoroutine(SpawnDelay(2));
            TimerActive = true;
            if (MainBGPlaying != true)
            {
                FindObjectOfType<AudioManager>().Play("MainBG");
                FindObjectOfType<AudioManager>().Stop("BossBG");
            }
        }
        else { GetItemText.enabled = true; GetItemText.text = GetScrewDriverText; }
    }

    public void LoadLevel3()
    {
        if (KeyCardAcquired == true)
        {
            LevelChangerPanel.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Elevator");
            SceneManager.LoadScene(3);
            StartCoroutine(SpawnDelay(3));
            TimerActive = true;
            if (MainBGPlaying != true)
            {
                FindObjectOfType<AudioManager>().Play("MainBG");
                FindObjectOfType<AudioManager>().Stop("BossBG");
            }
        }
        else { GetItemText.enabled = true; GetItemText.text = GetKeyCardText; }
    }

    public void LoadLevel4()
    {
        if (WireCutterAcquired == true)
        {
            LevelChangerPanel.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Elevator");
            SceneManager.LoadScene(4);
            StartCoroutine(SpawnDelay(4));
            TimerActive = true;
            FindObjectOfType<AudioManager>().Stop(("MainBG"));
            FindObjectOfType<AudioManager>().Play("BossBG");
        }
        else { GetItemText.enabled = true; GetItemText.text = GetWireCutterText; }
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

    protected void TutorialStringSelector()
    {
        Time.timeScale = 0;
        switch (WhichStringToShow)
        {
            case 1:
                TutorialTextRef.text = TutorialText1;
                break;
            case 2:
                TutorialTextRef.text = TutorialText2;
                break;
            case 3:
                TutorialTextRef.text = TutorialText3;
                break;
            case 4:
                TutorialTextRef.text = TutorialText4;
                break;
            case 5:
                TutorialTextRef.text = TutorialText5;
                break;
            case 6:
                TutorialTextRef.text = TutorialText6;
                break;
            case 7:
                TutorialTextRef.text = TutorialText7;
                break;
            case 8:
                TutorialTextRef.text = TutorialText8;
                break;
            case 9:
                Time.timeScale = 1;
                TutorialActive = false;
                TutorialTextPanel.SetActive(false);
                TimerActive = true;
                HealthPanel.SetActive(true);
                Pauseable = true;
                break;
        }
    }

    public void GameOver()
    {
        TimerActive = false;
        ResetEnemyLists();
        if (Win == true)
        {
            SceneManager.LoadScene("End screen");
        }
        else 
        {
            SceneManager.LoadScene("End screen");
            FindObjectOfType<EndScreen>().Win = false;
            FindObjectOfType<EndScreen>().TimeTookText = null;
        }
    }

    private void ResetEnemyLists()
    {
        for (int i = 0; i < Level1EnemiesRestart.Count; i++)
        {
            Level1Enemies[i].Death = false;
        }        
        for (int i = 0; i < Level2EnemiesRestart.Count; i++)
        {
            Level2Enemies[i].Death = false;
        }
        for (int i = 0; i < Level3EnemiesRestart.Count; i++)
        {
            Level3Enemies[i].Death = false;
        }
        for (int i = 0; i < Level4EnemiesRestart.Count; i++)
        {
            Level4Enemies[i].Death = false;
        }
        ScrewDriverAcquired = false;
        KeyCardAcquired = false;
        WireCutterAcquired = false;
        ARAcquired = false;
        SniperAcquired = false;
        HealthCarriedBetweenLevels = MaxHealth;
        CurrentTime = TimerAmount;
    }

    public void LevelChangerActive()
    {
        LevelChangerPanel.SetActive(true);
        GetItemText.enabled = false;
    }    

    public void LevelChangerInActive()
    {
        GetItemText.enabled = false;
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
        SceneManager.LoadScene("Main menu");
        ResetEnemyLists();
        HealthCarriedBetweenLevels = MaxHealth;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator SpawnDelay(int level)
    {
        yield return new WaitForSeconds(0.25f);
        switch (level)
            {
                case 1:
                    SpawnLevel1Enemies();
                    break;
                case 2:
                    SpawnLevel2Enemies();
                    break;
                case 3:
                    SpawnLevel3Enemies();
                    break;
                case 4:
                    SpawnLevel4Enemies();
                    break;
            }
        FindObjectOfType<PlayerScript>().Damageable = true;
    }
}
