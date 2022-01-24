using UnityEngine;

public class The_kid : Enemies
{
    protected override void OnEnable()
    {
        Game.EnemiesLeft++;             // Increasing the number of enemies
        base.OnEnable();                // Calling base' command
    }

    
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Game.PlayerHealth++;            // Increasing player's health
        gameObject.SetActive(false);    // Deactivating the gameObject

        {
            // Player's health must be increased before the enemy's disabling.
            // Once the enemy is disabled, it will move out from scene as well as from the 'sensor' area.
            // Exiting the 'sensor' area triggering the function below and reducing player's health.
            // Conclusion: Player's health increase is necessary to negate health reduce
        } // Explanation
    }
}
