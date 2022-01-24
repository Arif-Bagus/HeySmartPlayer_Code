using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Script;

    [HideInInspector]
    public int[] ObjectStock;           // The number of each pooled enemy types

    private List<GameObject>[] Pool;    // Stock of enemies of each type
    public GameObject[] ObjectToPool;   // Enemy types to be pooled

    private GameObject FreeObject;      // Multi-purpose gameObject

    private void Awake()
    {
        Script = this;
        Pool = new List<GameObject>[ObjectToPool.Length];   // Declaring the Pool array of list
        for (int i = 1; i <= ObjectToPool.Length; i++)      // Declaring Pool list ...
            Pool[i - 1] = new List<GameObject>(0);          // on each array
        ObjectStock = new int[ObjectToPool.Length];         // Declaring the ObjectCount array
    }


    // Function to change the stock number of each enemy types's pool-------------------------------------------------
    public void ChangePool(int[] PoolObject)
    {
        int TargetStock;       // Number of object shold be ready
        int CurrentStock;      // Current number of object

        // Evaluating stock of all enemy types
        for (int i = 1; i <= ObjectToPool.Length; i++)
        {
            TargetStock = PoolObject[i - 1];                // Getting the target stock
            CurrentStock = ObjectStock[i - 1];              // Getting the current stock

            // If the current stock is not match with the target, add or remove some
            if (CurrentStock != TargetStock)
            {
                if (CurrentStock < TargetStock)
                    AddObject(i - 1, TargetStock - CurrentStock);
                else
                    RemoveObject(i - 1, CurrentStock - TargetStock);
            }

            ObjectStock[i - 1] = TargetStock;               // Saving the current stock
        }
    }


    // Function to add object to pool---------------------------------------------------------------------------------
    private void AddObject(int EnemyType, int NumberToAdd)
    {
        for (int i = 1; i <= NumberToAdd; i++)
        {
            FreeObject = Instantiate(ObjectToPool[EnemyType]); // Getting a copy of enemy type
            FreeObject.SetActive(false);                    // Deactivating the copy
            Pool[EnemyType].Add(FreeObject);                // Adding the copy to pool list
            FreeObject.transform.SetParent                  // Changing the copy's parent hierarchy 
                (transform.GetChild(EnemyType));
        }
    }


    // Function to remove object from pool----------------------------------------------------------------------------
    private void RemoveObject(int EnemyType, int NumberToRemove)
    {
        for (int i = 1; i <= NumberToRemove; i++)
        {
            FreeObject = Pool[EnemyType][0];    // Getting the first object in corresponding list
            Pool[EnemyType].RemoveAt(0);        // Removing it from list ...
            Destroy(FreeObject);                // and destroying it
        }
    }


    // Function to get a copy from pool-------------------------------------------------------------------------------
    public GameObject GetPooledObject(int EnemyType)
    {
        // Correction of EnemyType int-value with array indexing
        EnemyType -= 1;

        // Checking all object in a pool ...
        for (int i = 1; i <= Pool[EnemyType].Count; i++)
        {
            // If there is an unused copy, return it
            if (!Pool[EnemyType][i - 1].activeInHierarchy)
                return Pool[EnemyType][i - 1];
        }

        // Returning null if all copy is currently in use
        return null;
    }
}
