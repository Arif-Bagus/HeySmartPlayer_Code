using System.Collections;
using UnityEngine;

public class The_stun_aiming : The_stun
{
    private Vector2 Front;
    private Vector2 Bezier1;
    private Vector2 Bezier2;
    private Vector2 Bezier3;
    private Vector2 Bezier4;
    public Transform AimTarget;
    private float Speed;
    private const float SpeedMod = 0.05f;

    // Activity called by OnEnable (override-able)-----------------------------------------------------------------
    protected override void OnEnableAction()
    {
        AllowTriggerExit = true;
        StartAiming();
    }


    // Functions and coroutine to manage aiming--------------------------------------------------------------------
    private void StartAiming() => StartCoroutine(MovingForward(true, Speed));
    private IEnumerator MovingForward(bool Beginning, float Speed)
    {
        int StepMax;
        Vector2 OldPosition;
        float Distance;

        Front = transform.up;
        StepMax = 75 * Beginning.CompareTo(false) - 1000 * Beginning.CompareTo(true);

        if (!Beginning) AllowTriggerExit = false;

        for (int i = 1; i <= StepMax; i++)
        {
            Speed += -SpeedMod * Beginning.CompareTo(false) - SpeedMod * Beginning.CompareTo(true);
            gameObject.transform.Translate(Front * Speed * Time.deltaTime, Space.World);
            yield return null;
        }

        if (Beginning)
        {
            OldPosition = transform.position;
            transform.Translate(Front * Speed * Time.deltaTime, Space.World);
            yield return null;
            Distance = Vector2.Distance(transform.position, OldPosition);
            
            yield return Turning(Distance);
        }
    }
    private IEnumerator Turning(float LastMovingDistance)
    {
        Vector2 OldPosition = Vector2.zero;
        Vector2 Position = Vector2.zero;
        float i = 0;
        float i_step;
        float Distance;

        Bezier1 = transform.position;
        Bezier2 = (Vector2)transform.position + 5 * Front;
        Bezier3 = Bezier2;
        Bezier4 = Bezier3 + 5 * ((Vector2)AimTarget.position - Bezier3).normalized;

        i_step = IStepCal(LastMovingDistance);

        while (i <= 1)
        {
            i += i_step;

            OldPosition = transform.position;
            Position = BezierCalculation(i);
            transform.position = Position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Position - OldPosition);
            Debug.DrawLine(Position, OldPosition, Color.green, 5);
            
            yield return null;
        }

        Distance = Vector2.Distance(Position, OldPosition);

        yield return MovingForward(false, Distance / Time.deltaTime);
    }
    private Vector2 BezierCalculation(float i)
    {
        return (1 - i) * (1 - i) * (1 - i) * Bezier1 +
            3 * (1 - i) * (1 - i) * i * Bezier2 +
            3 * (1 - i) * i * i * Bezier3 +
            i * i * i * Bezier4;
    }
    private float IStepCal(float LastMovingDistance)
    {
        float i_base = 0.01f;
        Vector2 Pos1 = BezierCalculation(0);
        Vector2 Pos2 = BezierCalculation(i_base);
        float Distance = Vector2.Distance(Pos1, Pos2);

        return i_base * LastMovingDistance / Distance;
    }

    
    // Function to assign all necessary objects--------------------------------------------------------------------
    protected override void ObjectsAssignment()
    {
        // Getting all necessary objects
        WorldOrigin = GameObject.Find("World Origin").transform;
        ThePool = transform.parent;

        Speed = 17;
    }
}
