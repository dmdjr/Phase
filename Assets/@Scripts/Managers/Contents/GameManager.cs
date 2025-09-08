using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Paused, GameOver, Clear }
    public GameState State { get; private set; }
    public int currentStageNum = 1; // Stage# 오브젝트의 이름이 1부터 시작
    public int lastStage = 10;
    [SerializeField] private GameObject[] playerDeathFragments;
    [SerializeField] private float respawnDelay = 2f;
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

    private void Start()
    {
        respawnPoint = currentStage.transform.Find("RespawnPoint");
        GameObject player = GameObject.Find("Player");
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }
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
                if (stage.name == "Stage" + currentStageNum.ToString())
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
        SkillController skillController = player.GetComponent<SkillController>();
        if (skillController != null)
        {
            skillController.RevertInversion();
        }
        if (player.gameObject.activeSelf)
        {
            StartCoroutine(PlayerDieCoroutine(player));
        }
        /*// 리스폰 위치 찾기
        respawnPoint = currentStage.GetComponent<Transform>().Find("RespawnPoint");
        if (!player || !respawnPoint) return;
        player.Respawn(respawnPoint);
        // 맵 상태 초기화
        ResetObjects(currentStage.transform);*/
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

    private IEnumerator PlayerDieCoroutine(PlayerController player)
    {
        Vector3 deathPosition = player.transform.position;
        Transform circleEffects = player.transform.parent.Find("CircleEffects");
        player.gameObject.SetActive(false);
        if (circleEffects != null)
        {
            circleEffects.gameObject.SetActive(false);
        }
        if (playerDeathFragments.Length > 0)
        {
            foreach (GameObject fragmentPrefab in playerDeathFragments)
            {
                GameObject fragment = Instantiate(fragmentPrefab, deathPosition, Quaternion.identity);
                Rigidbody2D rb = fragment.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                    rb.AddForce(direction * 200f);
                    float randomTorque = Random.Range(-30f, 30f);
                    rb.AddTorque(randomTorque);
                }
            }
        }
        // 4. 지정된 시간(respawnDelay)만큼 대기
        yield return new WaitForSeconds(respawnDelay);

        // 5. 맵 상태 초기화
        ResetObjects(currentStage.transform);

        // 6. 리스폰 위치 찾기 및 플레이어 위치 이동
        respawnPoint = currentStage.transform.Find("RespawnPoint");
        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }

        // 7. 플레이어 오브젝트 다시 활성화
        player.gameObject.SetActive(true);
        if (circleEffects != null)
        {
            circleEffects.gameObject.SetActive(true);
        }
        SkillController skillController = player.GetComponent<SkillController>();
        if (skillController != null)
        {
            skillController.ResetSkillState();
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