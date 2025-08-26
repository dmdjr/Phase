using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crackedTile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MissileHazard>() != null)
        {
            gameObject.SetActive(false);
        }
    }
}
