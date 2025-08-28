using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Paused, GameOver, Clear }
    public GameState State { get; private set; }

    private int currentStageNum = 1;
    private int lastStage = 10;
    private Transform normalWorld;
    private List<GameObject> stages = new List<GameObject>();
    private GameObject currentStage;

    private Transform respawnPoint;
    // private float respawnWaitingTime = 3.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitStage();
    }

    private void InitStage()
    {
        // Noemal_World의 Stage# 리스트 저장, Stage1만 활성화
        normalWorld = GameObject.Find("Objects").GetComponent<Transform>();
        if (normalWorld == null)
        {
            Debug.Log($"Can't find Objects");
        }
        stages.Clear();
        foreach (Transform stage in normalWorld)
        {
            if (stage.name.StartsWith("Stage"))
            {
                stages.Add(stage.gameObject);
                if (stage.name == "Stage1")
                {
                    stage.gameObject.SetActive(true);
                    currentStage = stage.gameObject;
                }
                else
                {
                    stage.gameObject.SetActive(false);
                }
            }
        }
    }

    public void IncreaseStage()
    {
        if (currentStageNum != lastStage)
        {
            GameObject prevStage = stages[currentStageNum - 1];
            currentStageNum++;
            currentStage = stages[currentStageNum - 1];
            currentStage.SetActive(true);
            prevStage.SetActive(false);
        }
    }

    public void PlayerDie(PlayerController player)
    {
        // 리스폰 위치 찾기
        respawnPoint = currentStage.GetComponent<Transform>().Find("RespawnPoint");
        if (!player || !respawnPoint) return;
        player.Respawn(respawnPoint);
        // 맵 상태 초기화
        ResetObjects(currentStage.transform);
    }

    private void ResetObjects(Transform currentStage)
    {
        foreach (Transform child in currentStage.transform)
        {
            child.gameObject.SetActive(false);
        }

        // 이후 다시 활성화
        foreach (Transform child in currentStage.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    // public void ChangeState(GameState newState)
    // {
    //     State = newState;
    //     switch (State)
    //     {
    //         case GameState.Ready:
    //             // 초기화
    //             break;
    //         case GameState.Playing:
    //             Time.timeScale = 1f;
    //             break;
    //         case GameState.Paused:
    //             Time.timeScale = 0f;
    //             break;
    //         case GameState.GameOver:
    //             // UIManager.ShowGameOver();
    //             break;
    //         case GameState.Clear:
    //             // UIManager.ShowClear();
    //             break;
    //     }
    // }
}