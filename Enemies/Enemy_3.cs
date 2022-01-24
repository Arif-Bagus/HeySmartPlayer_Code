using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_3 : Enemies
{
    // Allocating memory for other scripts
    private EnemyPool Pool;

    private Transform[] ShootPoint;         // Transform which stun may inherited from
    private GameObject TheStun;             // TheStun game-object
    private List<int> SummonPos;            // List of selected point-index for stun summoning
    private int StunNumber;                 // Number of stun summoned at once
    private int StunSummon;                 // Shoot number position randomizer
    private float ShootDelay;               // Time delay between each shoot
    private float ShootPosThreshold = 8;    // Area size where shooting is allowed 
    private string PointPosString;          // Point gameObject's name
    private Vector2 PointPos;               // Point gameObject's vector2
    private Quaternion PointRot;            // Point gameObject's rotation (in Quaternion)

    protected override void OnEnable()
    {
        Preparation();                      // Preparing all necessary variable
        base.OnEnable();                    // Calling base' command
        if (gameObject.activeSelf)          // Starting stun shooting
            StartCoroutine(StartShoot());
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Stopping the velocity-adjuster
        StopCoroutine(Adjuster);

        // Disabling self-gameobject
        if (collision.gameObject.CompareTag("Player"))
        {
            Game.PlayerHealth++;
            BeInactive();
            {
                /* Player's health must be increased before the enemy's destruction.
                   Once the enemy destroyed, it will move out from scene as well as from the 'sensor' area.
                   Exiting the 'sensor' area triggering the method below and reducing player's health.
                   Conclusion: Player's health increase is necessary to negate health reduce
                */
            } // Explanation
        }

        // Adjusting after-collision velocity
        if (gameObject.activeSelf)
            StartCoroutine(Adjuster);
    }


    // Function to prepare necessary variables---------------------------------------------------------------------
    private void Preparation()
    {
        // Getting generic parameters
        StunNumber = Data1;
        ShootDelay = Data2;
        
        // Getting each stun summoning's transform
        SummonPos = new List<int> { 0, 1, 2, 3, 4, 5 };
        ShootPoint = new Transform[StunNumber];
        for (int i = 1; i <= StunNumber; i++)
        {
            StunSummon = SummonPos[Random.Range(0, SummonPos.Count)];
            SummonPos.Remove(StunSummon);

            PointPosString = "Point " + StunSummon;
            ShootPoint[i - 1] = gameObject.transform.GetChild(0).Find(PointPosString);
        }
    }


    // Coroutine and function to startstop the repetitive stun summoning-------------------------------------------
    private IEnumerator StartShoot()
    {
    Start:
        // Brief delay before first shoot
        yield return new WaitForSeconds(ShootDelay / 2);

        // If self-gameobject is inside the threshold area, shooting the stun
        if (gameObject.transform.position.sqrMagnitude <= ShootPosThreshold)
        {
            DoShoot();
            yield return new WaitForSeconds(ShootDelay / 2);
        }

        goto Start;
    }
    private void DoShoot()
    {
        for (int i = 1; i <= StunNumber; i++)
        {
            PointPos = ShootPoint[i-1].position;// Getting point's position
            PointRot = ShootPoint[i-1].rotation;// Getting point's rotation
            SummonStun(PointPos, PointRot);     // Summoning the stun
        }
        
        {
            // This has been changed into function.
            // In case something wrong happened, revert this back ...
            // into coroutine.
        } // IMPORTANT!!!
    }


    // Function to Summon stun-------------------------------------------------------------------------------------
    private void SummonStun(Vector3 Position, Quaternion Rotation)
    {
        TheStun = Pool.GetPooledObject(6);      // Getting the stun from its pool

        // If the stun is successfully obtained ...
        if (TheStun != null)
        {
            TheStun.transform.position = Position;  // Setting its position
            TheStun.transform.rotation = Rotation;  // Setting its rotation
            TheStun.SetActive(true);                // Activating it
        }
    }


    // Function to assign all necessary objects--------------------------------------------------------------------
    protected override void ObjectsAssignment()
    {
        // Calling base' command
        base.ObjectsAssignment();

        // Establishing communication with other scripts
        Pool = EnemyPool.Script;
    }
}
