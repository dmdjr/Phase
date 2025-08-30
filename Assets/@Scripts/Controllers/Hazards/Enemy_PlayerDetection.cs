using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_PlayerDetection : MonoBehaviour
{
    private Enemy enemy;
    void Awake()
    {
        enemy = gameObject.transform.parent.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.isPlayerDetected = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.isPlayerDetected = false;
        }
    }
}
