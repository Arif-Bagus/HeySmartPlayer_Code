using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    public static SummonManager Script;

    // Allocating memory for other scripts
    private GameManager Game;
    private EnemyPool Pool;
    private LevelMaker LevelData;
    private Enemies Enemies;

    private GameObject EnemyObject;     // Object will be summoned
    private Transform SummonSpot;       // Enemy-object summoning position
    private Transform SummonCompass;    // Compass controlling SummonSpot
    private Transform WorldOrigin;      // World-origin hierarchy

    private void Awake() => Script = this;


    // Function and coroutine to summon enemy or boss based on current level------------------------------------------------
    public void Summoning(int Level)
    {
        // Getting the LevelData in Resource
        LevelData = Resources.Load<LevelMaker>("Levels/Level " + Level);

        // Changing the pool and start summoning
        if (LevelData != null)
        {
            Game.Pool.ChangePool(LevelData.EnemiesToPool);

            if (LevelData.IsBossLevel)
                SummonBoss(LevelData);
            if (!LevelData.IsBossLevel)
                StartCoroutine(SummonEnemyWaves(LevelData));
        }
        else Debug.Log("No level data found");
    }
    private void SummonBoss(LevelMaker LevelData)
    {
        GameObject BossObject;      // Declaring Boss gameObject

        Game.EnemiesLeft = 1000;    // Setting enemies left as semi-infinite

        // Instatntiating the Boss
        BossObject = Instantiate(LevelData.BossObject);

        // Changing Boss' rotation to match the 'World Origin' and change the parent
        BossObject.transform.rotation = WorldOrigin.transform.rotation;
        BossObject.transform.SetParent(WorldOrigin.transform);
    }
    private IEnumerator SummonEnemyWaves(LevelMaker LevelData)
    {
        int Waves;                  // Number of waves
        int EnemiesInLevel;         // Total enemies within level
        int EnemiesInWave;          // Total enemies within wave
        int EnemyType;              // Enemy-type within wave
        int Impulse;                // Impulse of enemies within wave
        int Data1;                  // Enemy's data1 within wave
        float Data2;                // Enemy's data2 within wave
        WaitForSeconds Wait;        // Wait over one delay
        WaitForSeconds InitWait;    // Wait over one initial-delay
        LevelMaker.Data[] Enemies;  // Enemy's generic data

        Waves = LevelData.NumberOfWave;     // Getting the number of enemy waves
        EnemiesInLevel = 0;                 // Presetting the total number of enemies
        Enemies = LevelData.Enemies;        // Copying enemy's generic data

        // Setting current level's enemies left
        foreach (LevelMaker.Data Enemy in Enemies)
            EnemiesInLevel += Enemy.NumberOfEnemies;
        Game.EnemiesLeft = EnemiesInLevel;

        // For every waves ...
        for (int i = 1; i <= Waves; i++)
        {
            // Preparing some parameters for efficiency
            Wait = new WaitForSeconds(Enemies[i - 1].Delay);
            InitWait = new WaitForSeconds(Enemies[i - 1].InitialDelay);
            EnemiesInWave = Enemies[i - 1].NumberOfEnemies;
            EnemyType = (int)Enemies[i - 1].TypeOfEnemies;
            Impulse = Enemies[i - 1].Impulse;
            Data1 = Enemies[i - 1].AdditionalStat1;
            Data2 = Enemies[i - 1].AdditionalStat2;

            // Summoning first enemy in wave (and its delay)
            if (Enemies[i - 1].OverrideInitialDelay)
                yield return InitWait;
            else
                yield return Wait;
            SummonEnemy(EnemyType, Impulse, Data1, Data2);

            // summon enemies (but the first one) in wave (and its delay)
            for (int j = 2; j <= EnemiesInWave; j++)
            {
                yield return Wait;
                SummonEnemy(EnemyType, Impulse, Data1, Data2);
            }
        }
    }


    // Function to rotate the summon compass randomly-----------------------------------------------------------------------
    private void CompassRotRandomizer()
    {
        float z = Random.Range(0f, 360f);
        SummonCompass.rotation = Quaternion.Euler(new Vector3(0, 0, z));
    }


    // Function to summon enemy from its pool-------------------------------------------------------------------------------
    public void SummonEnemy(int i, int Impulse, int Data1, float Data2)
    {
        // Getting the chosen enemyObject from its pool
        EnemyObject = Pool.GetPooledObject(i);

        // If the enemyObject is successfully obtained ..
        if (EnemyObject != null)
        {
            // Passing generic data to enemies summoned
            Enemies = EnemyObject.GetComponent<Enemies>();
            Enemies.Impulse = Impulse;
            Enemies.Data1 = Data1;
            Enemies.Data2 = Data2;

            // Setting enemy's initial position and rotation
            CompassRotRandomizer();
            EnemyObject.transform.position = SummonSpot.position;
            EnemyObject.transform.rotation = SummonSpot.rotation;

            // Activating the enemyObject
            EnemyObject.SetActive(true);
        }
        else Game.EnemiesLeft--;
    }


    // Function to stop all coroutine---------------------------------------------------------------------------------------
    public void ForceStop()
    { StopAllCoroutines(); }


    // Function to assign all necessary objects-----------------------------------------------------------------------------
    public void ObjectsAssignment()
    {
        // Establishing network communication with other scripts
        Game = GameManager.Script;
        Pool = EnemyPool.Script;

        // Getting all necessary objects
        WorldOrigin = GameObject.Find("World Origin").GetComponent<Transform>();
        SummonCompass = GameObject.Find("Summon Compass").GetComponent<Transform>();
        SummonSpot = GameObject.Find("Summon Spot").GetComponent<Transform>();
    }
}
