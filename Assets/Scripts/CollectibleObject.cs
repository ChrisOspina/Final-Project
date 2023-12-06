using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    private bool isCollected = false;

    public void Collect()
    {
        isCollected = true;
    }

    public bool IsCollected()
    {
        return isCollected;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}


