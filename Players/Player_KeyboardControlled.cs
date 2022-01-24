using UnityEngine;

public class Player_KeyboardControlled : MonoBehaviour
{
    private GameManager Game;       // Allocating memory for GameManager script
    private float HorizontalValue;  // Value of "Horizontal" axis

    private void Start()
    { ObjectsAssignment(); }

    private void Update()
    {
        HorizontalValue = Input.GetAxis("Horizontal");  // Getting the x-axis value
        if (Game.Player.IsPlayer2Controllable)          // Rotating based on input and if allowed
            transform.Rotate(0, 0, 200 * HorizontalValue * Time.fixedDeltaTime);
    }


    // Function to assign all necessary objects------------------------------------------------------------------
    private void ObjectsAssignment()
    { Game = GameManager.Script; }
}
