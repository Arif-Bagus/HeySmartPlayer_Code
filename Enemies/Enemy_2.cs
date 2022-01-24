using System.Collections;
using UnityEngine;

public class Enemy_2 : Enemies
{
    // Allocating memory for other scripts
    private EnemyPool Pool;

    public Vector2[] PossibleKidImagePos;       // All possible position for kids image
    private Transform[] KidImages;              // All kid images exist
    private SpriteRenderer[] KidImageSprite;    // Kid sprite hold in KidImages
    public GameObject TheKids;                  // The kid may be summoned
    private int KidsNumber;                     // Number of kids might be summoned

    protected override void OnEnable()
    {
        KidsNumber = Data1;     // Setting the number of kids
        SetKidSymbol();         // ... and its symbol

        base.OnEnable();        // Calling the base' command
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Stopping the velocity-adjuster
        StopCoroutine(Adjuster);

        // Summoning the kids and disabling self-gameobject
        if (collision.gameObject.CompareTag("Player"))
        {
            int Angle;          // Kids' main summoning angle
            int AngleOffset;    // Angle offset for randomness

            AngleOffset = Random.Range(-20, 21);

            // Summoning the kids
            for (int i = 1; i <= KidsNumber; i++)
            {
                Angle = KidsAngle(i, KidsNumber);
                SummonKids(transform.position, KidsRotation(Angle + AngleOffset));
            }

            // Preparation of disabling self-gameobject
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


    // Functions to show the kid-symbol images------------------------------------------------------------------
    private void SetKidSymbol()
    {
        // Disabling all kid-symbol
        foreach (SpriteRenderer KidImage in KidImageSprite)
            KidImage.enabled = false;

        // Preparing and enabling the necessary kid-symbol
        for (int i = 1; i <= KidsNumber; i++)
        {
            KidImages[i - 1].localPosition = GetPosition();
            KidImageSprite[i - 1].enabled = true;
        }
    }
    private Vector2 GetPosition()
    {
        // Randomizing selected position and returning it
        int Index = Random.Range(0, PossibleKidImagePos.Length);
        return PossibleKidImagePos[Index];
    }


    // Function to calculate kids' angle-------------------------------------------------------------------------
    private int KidsAngle(int KidNumber, int TotalKids)
    {
        int InitialAngle;       // Base angle value
        int IncreasingAngle;    // Increasing angle value
        int Angle;              // Angle of summoned kids'

        // Determining the angle parameter value based on number of kids
        switch (TotalKids)
        {
            case 1: InitialAngle = 0; IncreasingAngle = 0; break;
            case 2: InitialAngle = -60; IncreasingAngle = 40; break;
            case 3: InitialAngle = -60; IncreasingAngle = 30; break;
            default: InitialAngle = -50; IncreasingAngle = 20; break;
        }

        // Calculating and returning the angle
        Angle = InitialAngle + KidNumber * IncreasingAngle;
        return Angle;
    }


    // Specify TheKids rotation to face to (0,0) with offset-----------------------------------------------------
    private Quaternion KidsRotation(int Offset)
    {
        int Offset2 = 0;

        // Obtaining parent's position and its tangent
        float x = transform.position.x;
        float y = transform.position.y;
        float y_x = y / x;

        // Setting second angle offset based on its x-axis
        if (transform.position.x < 0) Offset2 = -90;
        if (transform.position.x > 0) Offset2 = 90;

        // Creating, calculating, and returning the Enemy_2's rotation's Vector3
        float ZRot = Mathf.Atan(y_x) * Mathf.Rad2Deg;
        Vector3 RotationEuler = new Vector3 (0, 0, ZRot + Offset + Offset2);
        Quaternion RotationQuat = Quaternion.Euler(RotationEuler);
        return RotationQuat;
    }


    // Function to Summon kids-----------------------------------------------------------------------------------
    private void SummonKids(Vector3 Position, Quaternion Rotation)
    {
        TheKids = Pool.GetPooledObject(5);      // Getting the kid from its pool

        // If the kid is successfully obtained ...
        if (TheKids != null)
        {
            TheKids.GetComponent<The_kid>().Impulse = Mathf.RoundToInt(Impulse * .75f);

            TheKids.transform.position = Position;  // Setting its position
            TheKids.transform.rotation = Rotation;  // Setting its rotation
            TheKids.SetActive(true);                // Activating it
        }
    }


    // Function to assign all necessary objects------------------------------------------------------------------
    protected override void ObjectsAssignment()
    {
        // Calling base' command
        base.ObjectsAssignment();

        // Establishing communication with other scripts
        Pool = EnemyPool.Script;

        // Setting all necessary parameters
        int Number = 5;
        KidImages = new Transform[Number];
        KidImageSprite = new SpriteRenderer[Number];

        // Getting all necessary objects
        for (int i = 1; i <= Number; i++)
        {
            KidImages[i - 1] = transform.GetChild(0).GetChild(i - 1);
            KidImageSprite[i - 1] = KidImages[i - 1].gameObject.GetComponent<SpriteRenderer>();
        }
    }
}
