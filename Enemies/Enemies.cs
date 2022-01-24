using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemies : MonoBehaviour
{
    // Allocating memory for other scripts
    protected GameManager Game;

    public int Impulse;             // Impulse done once game-object enabled
    public int Data1;               // Extra data 1 which will be interpreted by child class
    public float Data2;             // Extra data 2 which will be interpreted by child class
    protected Rigidbody2D ThisRb;   // Self-rigidbody
    protected IEnumerator Adjuster; // Velocity-adjuster holder

    protected void Awake() => ObjectsAssignment();
    protected virtual void OnEnable() => ThisRb.AddRelativeForce(new Vector2(0, Impulse), ForceMode2D.Impulse);

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        Game.PlayerHealth--;        // Reducing player's health
        Game.EnemyNumberDecrease(); // Counting down the number of enemy(es) on screen
        BeInactive();               // Doing this-deactivation
    }


    // Function to prepare and set the enemies-object inactive------------------------------------------------------
    protected virtual void BeInactive()
    {
        StopAllCoroutines();        // Stopping all possibly active coroutine
        gameObject.SetActive(false);// Deactivating the Enemy
    }


    // Coroutine to adjust the enemy-object's velocity--------------------------------------------------------------
    protected IEnumerator VelocityAdjuster()
    {
        int i = 0;
        float LowerVelocityLimit = .6f * Impulse;           // Lower velocity threshold
        float UpperVelocityLimit = .8f * Impulse;           // Upper velocity threshold
        Vector2 Direction = ThisRb.velocity.normalized;     // The direction of move

        // Increasing velocity if the after-collision velocity is too low
        while (ThisRb.velocity.sqrMagnitude < LowerVelocityLimit)
        {
            i++;
            ThisRb.velocity += 0.01f * Direction;
            yield return new WaitForEndOfFrame();

            // Once this loop operated 100 times, destroy if the the velocity is too low
            if (i == 100)
                if (ThisRb.velocity.sqrMagnitude < .7f * LowerVelocityLimit)
                    BeInactive();
        }

        // Decreasing velocity if the after-collision velocity is too high
        while (ThisRb.velocity.sqrMagnitude > UpperVelocityLimit)
        {
            ThisRb.velocity -= 0.01f * Direction;
            yield return new WaitForEndOfFrame();
        }
    }


    // Function to assign all necessary objects---------------------------------------------------------------------
    protected virtual void ObjectsAssignment()
    {
        Game = GameManager.Script;
        ThisRb = gameObject.GetComponent<Rigidbody2D>();
        Adjuster = VelocityAdjuster();
    }
}
