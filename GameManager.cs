using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Script;

    // Allocating memory for other scripts
    [HideInInspector] public SummonManager Summon;
    [HideInInspector] public PlayerStatus Player;
    [HideInInspector] public EnemyPool Pool;
    private TextManager Text;
    private CameraRotation[] CamRot;
    
    private GameObject[] EnemiesObject;         // Allocating memory for enemies' gameObject
    [Range(1, 50)] public int Level;            // Current level
    public int PlayerHealth;                    // Player's current health
    public int EnemiesLeft;                     // Number of enemies left
    private int FeatureOnProgress;              // Number of progressing feature
    private bool IsBusy;                        // Telling wether this is busy progressing feature


    private void Awake()
    { Script = this; }

    private void Start()
    {
        ObjectsAssignment();
        ToNextLevel();
    }

    private void Update()
    {
        // If the button is pressed, deactivating all enemies on screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnemiesObject = GameObject.FindGameObjectsWithTag("Enemy"); // Getting all enemies
            foreach (GameObject EnemyObject in EnemiesObject)           // Deactivating all enemies
                EnemyObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Summon.ForceStop();
            ClearScreen(true);
            EnemiesObject = GameObject.FindGameObjectsWithTag("Boss");  // Getting all Stun gameObject
            foreach (GameObject EnemyObject in EnemiesObject)           // Disabling all Stun gotten
                EnemyObject.SetActive(false);
            Level--;
            ToNextLevel();
        }
    }


    // Function to count down the number of enemy on screen and determine wether move to next level-------------------
    public void EnemyNumberDecrease()
    {
        EnemiesLeft--;          // Counting down the number of enemies on screen

        if (EnemiesLeft == 0)   // If there is no enemy left,
            ToNextLevel();      // ... moving to next level
    }
    

    // Function to clear the screen from enemy object-----------------------------------------------------------------
    private void ClearScreen(bool BombIncluded)
    {
        EnemiesObject = GameObject.FindGameObjectsWithTag("Enemy"); // Getting all Enemy gameObject
        foreach (GameObject EnemyObject in EnemiesObject)           // Disabling all Enemy gotten
            EnemyObject.SetActive(false);
        EnemiesObject = GameObject.FindGameObjectsWithTag("Kid");   // Getting all Kid gameObject
        foreach (GameObject EnemyObject in EnemiesObject)           // Disbling all Kid gotten
            EnemyObject.SetActive(false);
        EnemiesObject = GameObject.FindGameObjectsWithTag("Stun");  // Getting all Stun gameObject
        foreach (GameObject EnemyObject in EnemiesObject)           // Disabling all Stun gotten
            EnemyObject.SetActive(false);
        if (BombIncluded)
        {
            EnemiesObject = GameObject.FindGameObjectsWithTag("Bomb");  // Getting all Stun gameObject
            foreach (GameObject EnemyObject in EnemiesObject)           // Disabling all Stun gotten
                EnemyObject.SetActive(false);
        }
    }


    // Function to move to next level---------------------------------------------------------------------------------
    public void ToNextLevel()
    {
        EnemiesLeft = 100;                  // Disallowing the next stage to start promptly
        Level++;                            // Increasing the level
        Summon.ForceStop();                 // Forcing SummonManager acitivity
        /* Make the GUI pop-up here (with animation if possible) */
        Text.PrintStage();
        ClearScreen(false);                 // Clearing the screen from any unintended enemies (for debugging reason)
        LevelFeatureActivator();            // Initializing level featur
        StartCoroutine(StageStarter());     // Starting next stage
    }


    // Coroutine to check and activate special feature----------------------------------------------------------------
    private void LevelFeatureActivator()
    {
        // Separating the player
        if (Level >= 21 && !Player.HasSeparated)
        {
            FeatureOnProgress++;
            IsBusy = true;
            Player.PlayerSeparation();
        }
        
        // Rotating the camera
        if (Level > 20)
        {
            for (int i = 0; i < 2; i++)
            {
                // If it is the special level, resetting speed, else, calculating it based on level
                if (Level % 5 == 0) CamRot[i].ResetSpeed();
                else CamRot[i].SetSpeed(Level);

                // Changing both camera's speed
                CamRot[i].ChangeSpeed();
            }
        }
    }
    public void FeatureActivityIsDone()
    {
        FeatureOnProgress--;
        if (FeatureOnProgress == 0)
            IsBusy = false;
    }

    // Function which invoke Enemy's summoning function based on current level----------------------------------------
    private IEnumerator StageStarter()
    {
        while (IsBusy)
            yield return null;

        Summon.Summoning(Level);
    }


    // Function to end the game---------------------------------------------------------------------------------------
    public void GameOver()
    {
        Summon.ForceStop();     // Forcing any activity on SummonManager to stop
        StopAllCoroutines();    // Stoppoing all possibly active coroutine
        EnemiesLeft = 1000;     // Setting enemies left as pesudo-infinite
        Level = 0;              // Setting no level may be called
        ClearScreen(true);      // Clearing screen from any objects (Boss excluded)
    }


    // Function to assign all necesssary objects----------------------------------------------------------------------
    private void ObjectsAssignment()
    {
        // Establishing communication with other scripts
        Summon = SummonManager.Script;
        Player = PlayerStatus.Script;
        Pool = EnemyPool.Script;
        Text = TextManager.Script;
        CamRot = new CameraRotation[2];
        CamRot[0] = GameObject.Find("World Origin").GetComponent<CameraRotation>();
        CamRot[1] = GameObject.Find("Inversed World Origin").GetComponent<CameraRotation>();

        // Assigning all necessry objects for other scripts
        Summon.ObjectsAssignment();
        Player.ObjectsAssignment();
        Text.ObjectsAssignment();

        // Setting necessary parameters
        PlayerHealth = 3;
        Level--;
    }
}
