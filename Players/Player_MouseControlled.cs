using UnityEngine;

public class Player_MouseControlled : MonoBehaviour
{
    // Allocating memory for other scripts
    private PlayerStatus Player;

    private Transform InversedWorldOrigin;  // Allocating memory for InversedWorldOirign transform
    private Vector3 MousePositionWorld;     // Mouse position vector in world origin
    private Vector3 MousePositionLocal;     // Mouse position vector in local origin
    private int Width;                      // Screen-width
    private int Height;                     // Screen-height

    private void Start()
    { ObjectsAssignment(); }

    private void Update()
    {
        {
            // Goal: The player object is not rotating as camera rotates
            // The mouse (0,0) position must be 'centered' as the default position is at lower left corner.
            // Mouse position vector must be localized so that it is rotating as camera rotates,
            // resulting the player's pseudo-idle. The localization must be done on object which rotate
            // inversely with camera rotation, which is the "InversedWorldOrigin". If not, the player
            // will rotate wrongly.
        } // Explanation

        // Calculating mouse position with (0,0) as center and localizing it
        MousePositionWorld = Input.mousePosition - 0.5f * new Vector3(Width, Height, 0);
        MousePositionLocal = InversedWorldOrigin.InverseTransformPoint(MousePositionWorld);

        // Orienting the 'up' vector as mouse local position
        if (Player.IsPlayer1Controllable)
            transform.up = - MousePositionLocal;                                                    
    }

    private void ObjectsAssignment()
    {
        // Establishing communication network with other scripts
        Player = PlayerStatus.Script;

        // Getting all necessary objects
        InversedWorldOrigin = GameObject.Find("Inversed World Origin").GetComponent<Transform>();
        Width = Screen.width;
        Height = Screen.height;
    }
}
