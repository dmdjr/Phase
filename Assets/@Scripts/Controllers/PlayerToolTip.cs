using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolTip : MonoBehaviour
{
    public Transform player;

    public int activeUntilStage = 2;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                if (canvasGroup != null) canvasGroup.alpha = 0f;
            }
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || player == null || canvasGroup == null)
        {
            return;
        }

        int currentStage = GameManager.Instance.currentStageNum;
        GameManager.GameState currentState = GameManager.Instance.State;

        if (currentStage <= activeUntilStage && currentState != GameManager.GameState.GameOver)
        {
            canvasGroup.alpha = 1f;


            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

            rectTransform.position = screenPos;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
}
