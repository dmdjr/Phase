using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EndingTriggerController : MonoBehaviour
{
    public Tilemap currentStageTilemap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {

            GameManager.Instance.ChangeState(GameManager.GameState.Clear);
            UIManager.Instance.StartEnding(currentStageTilemap);

        }
    }
}
