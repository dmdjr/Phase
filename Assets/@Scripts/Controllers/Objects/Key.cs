using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    public bool isAcquired = false;

    void OnEnable()
    {
        isAcquired = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isAcquired = true;
            gameObject.SetActive(false);
            return;
        }
    }
}
