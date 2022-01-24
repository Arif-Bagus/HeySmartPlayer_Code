using System.Collections;
using UnityEngine;

public class P1CollisionAction : MonoBehaviour
{
    // Allocating memory for other scripts
    private PlayerStatus Player;

    private Transform TheCompass;   // Platform 1's compass
    private Transform WorldOrigin;  // World origin

    private void Start()
    { ObjectsAssignment(); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stun"))
        {
            StopAllCoroutines();                    // Stopping all coroutine
            StartCoroutine(PlayerDeactivator());    // De-controlling Player 1
        }
    }


    // Coroutine to de-control Player 1-------------------------------------------------------------------------
    private IEnumerator PlayerDeactivator()
    {
        Player.IsPlayer1Controllable = false;   // Disallowing Platform 1 to be controlled
        TheCompass.parent = WorldOrigin;        // Changing the parent (so that the player rotating as World Origin)
        yield return new WaitForSeconds(1);     // Delay time of de-control
        TheCompass.parent = null;               // Removing the parent
        Player.IsPlayer1Controllable = true;    // Allowing the Platform 1 to be controlled
    }


    // Function to assign all necessary objects-----------------------------------------------------------------
    private void ObjectsAssignment()
    {
        // Establishing communication network with other scripts
        Player = PlayerStatus.Script;

        // Getting all necessary objects
        TheCompass = transform.parent;
        WorldOrigin = GameObject.Find("World Origin").GetComponent<Transform>();
    }
}
