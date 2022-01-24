using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Enemy_1 : Enemies
{
    private SpriteResolver Symbol;  // Symbol resolver
    private int Health;             // Health before destructible

    protected override void OnEnable()
    {
        Health = Data1;             // Setting the health from Data1
        Symbol.SetCategoryAndLabel  // Getting the chosen symbol
            ("Symbol", Health.ToString());
        base.OnEnable();            // Calling the base' command
    }
    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Stopping the velocity-adjuster
        StopCoroutine(Adjuster);

        // Reducing self-health (or even disabling self-gameobject) when colliding with "player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Health--;
            if (Health >= 0)
                Symbol.SetCategoryAndLabel("Symbol", Health.ToString());

            if (Health <= 0)
            {
                BeInactive();
                Game.PlayerHealth++;
                {
                    /* Player's health must be increased before the enemy's destruction.
                       Once the enemy destroyed, it will move out from scene as well as from the 'sensor' area.
                       Exiting the 'sensor' area triggering the method below and reducing player's health.
                       Conclusion: Player's health increase is necessary to negate health reduce
                    */
                } // Explanation
            }
        }

        // Adjusting after-collision velocity
        if (gameObject.activeSelf)
            StartCoroutine(Adjuster);
    }


    // Function to assign all necessary objects
    protected override void ObjectsAssignment()
    {
        // Calling the base' command
        base.ObjectsAssignment();

        // Getting all necessary objects
        Symbol = transform.GetChild(0).gameObject.GetComponent<SpriteResolver>();
    }
}
