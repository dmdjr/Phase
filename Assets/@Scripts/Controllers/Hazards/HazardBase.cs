using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HazardBase : MonoBehaviour
{
    public abstract void OnPlayerEnter(PlayerController player);

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
            OnPlayerEnter(player);
    }
}
