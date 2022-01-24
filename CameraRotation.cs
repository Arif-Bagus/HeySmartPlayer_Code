using System.Collections;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] bool IsFlipped;// Determining if the rotation is flipped
    private int Direction;          // Setting the direction
    private float FlipValue;        // Valuing the flip state
    private float RotSpeedBase;     // Base rotation speed
    private float RotSpeedInc;      // Rotation speed increment
    private float RotSpeed;         // Rotation speed
    private float InitSpeed;        // Initial speed for interpolation
    private float TargetSpeed;      // Target speed for interpolation

    private void Start()
    {
        if (IsFlipped) FlipValue = -1;  // If set true, the rotation is reversed
        else FlipValue = 1;             // else, the rotation is normal
        RotSpeedInc = .75f;             // Setting the increasing rotation speed
        RotSpeedBase = 10f;             // Setting the base rotation speed
        RotSpeed = 0;                   // Beginning the rotation with zero speed
    }

    private void Update()
    { gameObject.transform.Rotate(Vector3.forward, RotSpeed * FlipValue * Time.deltaTime); }
    

    // Function to set or reset the rotation speed-----------------------------------------------------------------
    public void SetSpeed(int Level)
    {
        if (Level % 2 == 0) Direction = 1;  // At even level, the direction is clockwise
        else Direction = -1;                // At odd level, the direction is counter-clockwise
        TargetSpeed = (RotSpeedBase + RotSpeedInc * (Level - 20)) * Direction; // Calculating the speed
    }
    public void ResetSpeed()
    { TargetSpeed = 0; }

    // Function and coroutine to change rotation speed or direction smoothly---------------------------------------
    public void ChangeSpeed()
    { StartCoroutine(ChangeSpeedEnum()); }
    private IEnumerator ChangeSpeedEnum()
    {
        InitSpeed = RotSpeed;   // Getting the current speed
        
        // Changing the rotation speed smoothly based on interpolation
        for (float i = 0; i < 1; i += .1f)
        {
            RotSpeed = Mathf.Lerp(InitSpeed, TargetSpeed, i);
            yield return new WaitForEndOfFrame();
        }
    }
}
