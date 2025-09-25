using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    public bool isAcquired = false;
    public AudioClip launchClip;
    void OnEnable()
    {
        isAcquired = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (launchClip != null)
            {
                SoundManager.Instance.PlaySfx(launchClip);
            }
            isAcquired = true;
            gameObject.SetActive(false);
            return;
        }
    }
}
