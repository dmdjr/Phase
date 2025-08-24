using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}

    public enum GameState { Ready, Playing, Paused, GameOver, Clear }
    public GameState State { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.Ready:
                // 초기화
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
            case GameState.GameOver:
                // UIManager.ShowGameOver();
                break;
            case GameState.Clear:
                // UIManager.ShowClear();
                break;
        }
    }
}