using System.Collections;
using UnityEngine;

public class P2CollisionAction : MonoBehaviour
{
    private PlayerStatus Player;

    private void Start()
    { ObjectsAssignment(); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Stun"))
        {
            StopAllCoroutines();                    // Stopping all coroutine
            StartCoroutine(PlayerDeactivator());    // De-controlling Player 2
        }
    }


    // Coroutine to de-control Player 2---------------------------------------------------------------------------
    private IEnumerator PlayerDeactivator()
    {
        Player.IsPlayer2Controllable = false;   // Disallowing Player 2 to be controlled
        yield return new WaitForSeconds(1);     // Delay time of de-control
        Player.IsPlayer2Controllable = true;    // Allowing Player 2 to be controlled
    }


    // Function to assign all necessary objects-------------------------------------------------------------------
    private void ObjectsAssignment()
    { Player = PlayerStatus.Script; }
}
