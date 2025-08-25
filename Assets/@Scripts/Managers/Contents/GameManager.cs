using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Paused, GameOver, Clear }
    public GameState State { get; private set; }

    private int currentStage = 1;
    private int lastStage = 10;
    private Transform normalWorld;
    private List<GameObject> stages = new List<GameObject>();

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
        // Noemal_World의 Stage# 리스트 저장, 현재 스테이지(Stage1)만 활성화
        normalWorld = GameObject.Find("Normal_World").GetComponent<Transform>();
        if (normalWorld == null)
        {
            Debug.Log($"Can't find {normalWorld.name}");
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
        if (currentStage != lastStage)
        {
            GameObject prevStageObj = stages[currentStage - 1];
            currentStage++;
            GameObject currentStageObj = stages[currentStage - 1];
            currentStageObj.SetActive(true);
            prevStageObj.SetActive(false);
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