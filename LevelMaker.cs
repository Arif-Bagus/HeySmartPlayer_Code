using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelData", order = 10)]
public class LevelMaker : ScriptableObject
{
    public bool IsBossLevel = false;    // Telling wether the level is Boss or normal one
    public GameObject BossObject;       // Boss-object may be passed to SummonManager
    public int NumberOfWave = 0;        // Number of waves in level
    public Data[] Enemies;              // Datas of summoned enemies
    public int[] EnemiesToPool;         // Enemy's pool data

    [System.Serializable]
    public struct Data
    {
        public EnemyType TypeOfEnemies;
        public int NumberOfEnemies;
        public int Impulse;
        public float Delay;
        public bool OverrideInitialDelay;
        public float InitialDelay;
        public int AdditionalStat1;
        public float AdditionalStat2;

        public Data(int TypeOfEnemies, int NumberOfEnemies, int Impulse, float Delay, int AdditionalStat1, float AdditionalStat2)
        {
            this.TypeOfEnemies = (EnemyType)TypeOfEnemies;
            this.NumberOfEnemies = NumberOfEnemies;
            this.Impulse = Impulse;
            this.Delay = Delay;
            OverrideInitialDelay = false;
            InitialDelay = 0;
            this.AdditionalStat1 = AdditionalStat1;
            this.AdditionalStat2 = AdditionalStat2;
        }

        public Data(int TypeOfEnemies, int NumberOfEnemies, int Impulse, float Delay, float InitialDelay, int AdditionalStat1, float AdditionalStat2)
        {
            this.TypeOfEnemies = (EnemyType)TypeOfEnemies;
            this.NumberOfEnemies = NumberOfEnemies;
            this.Impulse = Impulse;
            this.Delay = Delay;
            OverrideInitialDelay = true;
            this.InitialDelay = InitialDelay;
            this.AdditionalStat1 = AdditionalStat1;
            this.AdditionalStat2 = AdditionalStat2;
        }
    }

    public enum EnemyType : int { Enemy1 = 1, Enemy2, Enemy3, Enemy4};
    public Dictionary<EnemyType, string> AddStat1Label = new Dictionary<EnemyType, string>()
    {
        { EnemyType.Enemy1, "Health" },
        { EnemyType.Enemy2, "Number of Kids" },
        { EnemyType.Enemy3, "Stun Number" },
        { EnemyType.Enemy4, "Thrust Step" },
    };
    public Dictionary<EnemyType, string> AddStat2Label = new Dictionary<EnemyType, string>()
    {
        { EnemyType.Enemy3, "Stun Delay" },
        { EnemyType.Enemy4, "Max Force" },
    };
}