using UnityEngine;

public class SymbolRotation : MonoBehaviour
{
    public int MinSpeed;        // Rotation minimum speed
    public int MaxSpeed;        // Rotation maximum speed
    private int Direction;      // Direction of rotation
    private int RotSpeed;       // Rotation set speed
    private Vector3 Rotation;   // Rotation per step

    private void OnEnable()
    {
        // Randomize the initial rotation
        transform.Rotate(Vector3.forward * Random.Range(0, 360));

        // Setting parameters for rotation
        Direction = Random.Range(0, 2) * 2 - 1;
        RotSpeed = Direction * Random.Range(MinSpeed, MaxSpeed);
        Rotation = Vector3.forward * RotSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    { transform.Rotate(Rotation); }
}
