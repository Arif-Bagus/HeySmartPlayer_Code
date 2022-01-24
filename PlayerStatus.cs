using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Script;

    private GameManager Game;

    [HideInInspector] public bool IsPlayer1Controllable;
    [HideInInspector] public bool IsPlayer2Controllable;
    [HideInInspector] public bool HasSeparated; // Determining wether the player's platform has been separated
    private GameObject Player1;                 // Platform 1's gameobject
    private GameObject Player2;                 // Platform 2's gameobject
    private GameObject PlayerCompass2;          // Platform 2's compass' gameobject
    

    private void Awake()
    { Script = this; }


    public void PlayerSeparation()
    { StartCoroutine(PlayerSeparationEnum()); }
    private IEnumerator PlayerSeparationEnum()
    {
        // Getting both compasses
        Transform Compass1 = GameObject.Find("Player Compass 1").transform;
        Transform Compass2 = GameObject.Find("Player Compass 2").transform;

        // Disabling the player from controlling either controller
        IsPlayer1Controllable = false;
        IsPlayer2Controllable = false;

        // Changing both player's parent (useful to prevent offset during separation)
        Compass1.parent = GameObject.Find("World Origin").transform;
        Compass2.parent = GameObject.Find("World Origin").transform;

        // Changing the control of platform 2
        PlayerCompass2.GetComponent<Player_MouseControlled>().enabled = false;
        PlayerCompass2.GetComponent<Player_KeyboardControlled>().enabled = true;

        // Enabling the platform 2 collider
        Player2.GetComponent<Collider2D>().enabled = true;
        Player2.GetComponent<PlatformEffector2D>().enabled = true;

        // Separating the platform 1 and 2 slowly
        for (int i = 1; i <= 60; i++)
        {
            Player1.transform.localPosition += new Vector3(0, 0.004f, 0);
            Player2.transform.localPosition -= new Vector3(0, 0.004f, 0);
            yield return new WaitForEndOfFrame();
        }

        // Reverting player 1's parent back to normal
        Compass1.parent = null;

        // Enabling the player from controlling both controller
        // and setting as has been separated
        IsPlayer1Controllable = true;
        IsPlayer2Controllable = true;
        HasSeparated = true;

        Game.FeatureActivityIsDone();
    }

    public void ObjectsAssignment()
    {
        // Establishing communication network with other scripts
        Game = GameManager.Script;

        // Getting all necessary objects
        Player1 = GameObject.Find("Player 1");
        Player2 = GameObject.Find("Player 2");
        PlayerCompass2 = GameObject.Find("Player Compass 2");

        // Setting all necessary parameters
        HasSeparated = false;
        IsPlayer1Controllable = true;
        IsPlayer2Controllable = true;
    }
}
