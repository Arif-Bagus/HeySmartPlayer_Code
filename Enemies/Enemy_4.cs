using System.Collections;
using UnityEngine;

public class Enemy_4 : Enemies
{
    private Transform Symbol;           // Symbol of this-gameobject
    private Transform SymbolMask;       // The symbol's mask
    private Transform ThisParent;       // This transform's parent
    private SpriteRenderer SymbolSpr;   // The symbol's spriterenderer
    private ParticleSystem.EmissionModule Exhaust; // Jet's exhaust
    private float SymbolDir;            // Direction of symbol
    private float MaxThrustForce;       // Thrust force
    private int ThrustStep;             // Thrust total-step
    private bool IsThrsutingDone;       // Telling wether thrusting has done
    

    protected override void OnEnable()
    {
        Preparation();                  // Preparing all necessary variables

        StartCoroutine(ThrustPreparation()); // Starting the thrust preparation
        base.OnEnable();                // Calling base' command
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsThrsutingDone)
            StopCoroutine(Adjuster);

        // Preparation and destroying self when colliding with "player"
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            Game.PlayerHealth++;
            BeInactive();
        }

        if (IsThrsutingDone && gameObject.activeSelf)
            StartCoroutine(Adjuster);
    }


    // Coroutines to thrust the self-gameobject-----------------------------------------------------------------------
    private IEnumerator ThrustPreparation()
    {
        yield return new WaitForSeconds(1);

        float InitDir;          // Symbol's initial direction
        float FinalDir;         // Symbol's (and thruster) final direction
        float T;                // T_parameter for interpolating
        int StepMax;            // Symbol rotation step
        Color SymbolColor;      // Symbol's color

        // Setting necessary parameters
        FinalDir = DirectionCalculation();
        InitDir = FinalDir + Random.Range(-180, 180);
        SymbolColor = new Color(1, 1, 1, 0);
        StepMax = 45;

        // Showing the arrow-symbol gradually
        transform.SetParent(null);
        for (float i = 1; i <= StepMax; i++)
        {
            T = i / StepMax;
            SymbolDir = Mathf.LerpAngle(InitDir, FinalDir, -1.311f * T * T + 2.29f * T);
            Symbol.rotation = Quaternion.Euler(0, 0, SymbolDir);

            SymbolColor.a = T;
            SymbolSpr.color = SymbolColor;

            yield return null;
        }
        transform.SetParent(ThisParent);

        // Start thrusting
        yield return Thrusting();
    }
    private IEnumerator Thrusting()
    {
        Vector2 MaskPos;            // Position of arrow-symbol's mask
        float ThrustForce;          // Thrust force
        Vector2 Direction;          // Direction of force

        MaskPos = Vector2.zero;     // Initiallizing mask's position

        // Getting the direction in cartesian format
        Direction.x = - Mathf.Sin(SymbolDir * Mathf.Deg2Rad);
        Direction.y = Mathf.Cos(SymbolDir * Mathf.Deg2Rad);

        // Thrusting the enemy-object
        for (float i = 1; i <= ThrustStep; i++)
        {
            ThrustForce = Mathf.Lerp(MaxThrustForce, 0, i / ThrustStep);
            ThisRb.AddForce(ThrustForce * Direction, ForceMode2D.Force);
            Exhaust.rateOverTime = ThrustStep - i;

            MaskPos.y = Mathf.Lerp(0, 0.88f, i / ThrustStep);
            SymbolMask.localPosition = MaskPos;

            yield return null;
        }
        IsThrsutingDone = true;
    }


    private float DirectionCalculation()
    {
        int Offset1;
        int Offset2 = 0;

        // Obtaining parent's position and its tangent
        float x = transform.position.x;
        float y = transform.position.y;
        float y_x = y / x;

        // Randomizing first angle offset
        Offset1 = Random.Range(-10, 11);

        // Setting second angle offset based on its x-axis
        if (transform.position.x < 0) Offset2 = -90;
        if (transform.position.x > 0) Offset2 = 90;

        // Creating, calculating, and returning the Enemy_2's rotation's Vector3
        float ZRot = Mathf.Atan(y_x) * Mathf.Rad2Deg;
        return ZRot + Offset1 + Offset2;
    }


    // Function to prepare and set object inactive--------------------------------------------------------------------
    protected override void BeInactive()
    {
        transform.SetParent(ThisParent);
        base.BeInactive();
    }


    // Function to prepare initial parameter of each summoning--------------------------------------------------------
    private void Preparation()
    {
        // Getting generic parameters
        ThrustStep = Data1;
        MaxThrustForce = Data2;

        // Setting necessary parameters
        IsThrsutingDone = false;
        Exhaust.rateOverTime = 0;
        SymbolSpr.color = new Color(1, 1, 1, 0);
        SymbolMask.localPosition = Vector2.zero;
    }


    // Function to assign all necessary objects------------------------------------------------------------------
    protected override void ObjectsAssignment()
    {
        // Calling base' command
        base.ObjectsAssignment();

        // Getting all necessary objects
        Symbol = transform.GetChild(0);
        SymbolMask = Symbol.GetChild(0);
        SymbolSpr = Symbol.gameObject.GetComponent<SpriteRenderer>();
        Exhaust = Symbol.Find("Exhaust").gameObject.GetComponent<ParticleSystem>().emission;
        ThisParent = transform.parent;
    }
}
