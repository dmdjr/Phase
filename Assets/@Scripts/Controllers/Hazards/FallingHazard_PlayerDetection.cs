using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingHazard_PlayerDetection : MonoBehaviour
{
    public GameObject parent;
    private FallingHazard fallingHazard;
    void Awake()
    {
        fallingHazard = gameObject.transform.parent.GetComponent<FallingHazard>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fallingHazard.OnPlayerDetected();
        }
    }

    // void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //     {
    //         parent.GetComponent<
    //     }
    // }
}
