using UnityEngine;

public class The_stun : MonoBehaviour
{
    protected Transform WorldOrigin;        // WorldOrigin hierarchy
    protected Transform ThePool;            // TheStunPool hierarchy
    private Rigidbody2D ThisRb;             // Self-rigidbody
    protected bool AllowTriggerExit;        // Telling wether self-gameobject destroyed upon exiting trigger2D
    private int Impulse;                    // Impulse upon enabled
    

    protected void Awake() => ObjectsAssignment();
    protected void OnEnable()
    {
        if (gameObject.activeSelf)
        {
            transform.parent = WorldOrigin;
            OnEnableAction();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    { BeInactive(); }

    public void OnTriggerExit2D(Collider2D collision)
    { if (gameObject.activeSelf && !AllowTriggerExit) BeInactive(); }


    // Activity called by OnEnable (override-able)-----------------------------------------------------------------
    protected virtual void OnEnableAction()
    { ThisRb.AddRelativeForce(new Vector2(0, Impulse), ForceMode2D.Impulse); }


    // Function to prepare the deactivation-------------------------------------------------------------------------
    protected void BeInactive()
    {
        transform.parent = ThePool;         // Reverting back into the pool
        StopAllCoroutines();
        gameObject.SetActive(false);        // Deactivating gameObject
    }


    // Function to assign all necessary object----------------------------------------------------------------------
    protected virtual void ObjectsAssignment()
    {
        // Getting all necessary objects
        WorldOrigin = GameObject.Find("World Origin").transform;
        ThePool = GameObject.Find("The Stun Pool").transform;
        ThisRb = gameObject.GetComponent<Rigidbody2D>();

        // Setting all necessary parameters
        AllowTriggerExit = false;
        Impulse = 8;
    }
}
