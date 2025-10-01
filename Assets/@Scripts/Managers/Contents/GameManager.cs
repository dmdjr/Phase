using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Ready, Playing, Paused, GameOver, Clear }
    private bool isPaused = false;
    public GameState State { get; private set; }
    public int currentStageNum = 1; // Stage# 오브젝트의 이름이 1부터 시작
    public int skillGrade = 0;
    public int clearCnt = 0;

    private SkillController skillController;

    public int lastStage = 40;
    [SerializeField] private GameObject[] playerDeathFragments;
    [SerializeField] private float respawnDelay = 2f;
    private Transform normalWorld;
    private List<GameObject> stages = new List<GameObject>();
    private GameObject currentStage;

    private Transform respawnPoint;
    // private float respawnWaitingTime = 3.0f;

    public AudioClip bgmClip1; // 스테이지 1~15, 35~40용
    public AudioClip bgmClip2; // 스테이지 16~34용
    public AudioClip dieClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // SaveData loaded = SaveManager.Instance.Load();
        // if (loaded != null)
        // {
        //     currentStageNum = loaded.currentStage;
        // }

        // InitStage();
        State = GameState.Ready;
    }

    public void Init()
    {
        InitStage();
        ChangeState(GameState.Playing);

        UpdateBgmForStage(currentStageNum);

        respawnPoint = currentStage.transform.Find("RespawnPoint");
        GameObject player = GameObject.Find("Player");

        if (player != null)
        {
            skillController = player.GetComponent<SkillController>();
        }
        if (skillController != null)
        {
            if (currentStageNum == 5) skillGrade = 0;
            else if (currentStageNum == 10) skillGrade = 1;
            else if (currentStageNum == 15) skillGrade = 2;

            switch (skillGrade)
            {
                case 1:
                    skillController.enabled = true;
                    UIManager.Instance.UnlockSkill();
                    break;
                case 2:
                    skillController.enabled = true;
                    skillController.releasePointMoveSpeed = 10f;
                    skillController.circleShrinkSpeed = 1f;
                    skillController.circleGrowSpeed = 0.7f;
                    skillController.finalDashForce = 15f;
                    skillController.UpdateCircleSize(new Vector3(3f, 3f, 2f));
                    break;
                case 3:
                    skillController.enabled = true;
                    skillController.releasePointMoveSpeed = 15f;
                    skillController.circleShrinkSpeed = 2f;
                    skillController.circleGrowSpeed = 0.5f;
                    skillController.finalDashForce = 20f;
                    skillController.UpdateCircleSize(new Vector3(5f, 5f, 2f));
                    break;
            }
        }

        if (respawnPoint != null)
        {
            player.transform.position = respawnPoint.position;
        }

        if (currentStageNum == 1)
        {
            player.transform.position = new Vector3(-17f, 37f, 0);
        }
    }

    private void Update()
    {
        if (State != GameState.Ready && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                ChangeState(GameState.Paused);
                isPaused = true;
            }
            else
            {
                ChangeState(GameState.Playing);
                isPaused = false;
            }
        }
    }

    private void InitStage()
    {
        // Save the Stage# lists of Objects(GameObject), and activate current stage 
        normalWorld = GameObject.Find("Objects").GetComponent<Transform>();
        if (normalWorld == null)
        {
            Debug.Log($"Can't find Objects");
            return;
        }
        stages.Clear();
        foreach (Transform stage in normalWorld)
        {
            if (stage.name.StartsWith("Stage"))
            {
                stages.Add(stage.gameObject);
                Tilemap stageTilemap = stage.GetComponentInChildren<Tilemap>();
                if (stageTilemap != null)
                {
                    stageTilemap.color = Color.white;
                }
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
        if (currentStageNum < lastStage)
        {
            GameObject prevStage = stages[currentStageNum - 1];
            currentStageNum++;
            currentStage = stages[currentStageNum - 1];
            currentStage.SetActive(true);
            prevStage.SetActive(false);

            CheckForSkillDegradation(currentStageNum);
            UpdateBgmForStage(currentStageNum);
            /*if (currentStageNum >= lastStage)
            {
                ChangeState(GameState.Clear);
                UIManager.Instance.StartEnding();
                return; // 더 이상 스테이지를 증가시키지 않고 종료함
            }*/
            // auto save
            // SaveData save = new SaveData();
            // save.currentStage = currentStageNum;
            // SaveManager.Instance.Save(save);
        }
    }

    public void PlayerDie(PlayerController player)
    {
        SoundManager.Instance.PlaySfx(dieClip);
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
    private void UpdateBgmForStage(int stageNum)
    {
        AudioClip targetBgm;
        float fadeDuration;

        if (stageNum >= 16 && stageNum <= 34)
        {
            targetBgm = bgmClip2;
            fadeDuration = 50.0f;
        }
        else
        {
            // 그 외 모든 스테이지 (1~15, 35~40)는 BGM 1
            targetBgm = bgmClip1;
            fadeDuration = 1.5f;
        }

        if (targetBgm != null)
        {
            SoundManager.Instance.PlayBgm(targetBgm, fadeDuration);

        }
    }
    private void ResetObjects(Transform currentStage)
    {
        Dictionary<GameObject, bool> originalStates = new Dictionary<GameObject, bool>();
        foreach (Transform child in currentStage.transform)
        {
            originalStates.Add(child.gameObject, child.gameObject.activeSelf);
            child.gameObject.SetActive(false);
        }

        foreach (Transform child in currentStage.transform)
        {
            if (originalStates[child.gameObject]) 
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator PlayerDieCoroutine(PlayerController player)
    {

        ChangeState(GameState.GameOver);

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
        Key[] keysInStage = currentStage.GetComponentsInChildren<Key>(true);
        foreach (Key key in keysInStage)
        {
            key.gameObject.SetActive(true); 
        }
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

        if (skillController != null && skillController.isActiveAndEnabled)
        {
            skillController.ResetSkillState();
        }

        ChangeState(GameState.Playing);
    }

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.Ready:
                // main menu
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                UIManager.Instance.HidePausePopup();
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                UIManager.Instance.ShowPausePopup();
                break;
            case GameState.GameOver:
                // UIManager.ShowGameOver();
                break;
            case GameState.Clear:
                clearCnt++;
                SaveManager.Instance.ResetSave();
                UIManager.Instance.Init();
                break;
        }
    }
    private void CheckForSkillDegradation(int newStage)
    {
        if (skillController == null || !skillController.isActiveAndEnabled)
        {
            return;
        }

        switch (newStage)
        {
            case 35:
                skillController.circleShrinkSpeed = 0.7f;
                skillController.circleGrowSpeed = 0.3f;
                skillController.releasePointMoveSpeed = 5f;
                skillController.finalDashForce = 10f;
                skillController.UpdateCircleSize(new Vector3(2f, 2f, 1f));
                break;

            case 36:
                skillController.UpdateCircleSize(new Vector3(1.5f, 1.5f, 1f));
                break;

            case 37:
                skillController.UpdateCircleSize(new Vector3(1.3f, 1.3f, 1f));
                break;

            case 38:
                skillController.UpdateCircleSize(new Vector3(1.2f, 1.2f, 1f));
                break;

            case 39:
                skillController.enabled = false;
                break;
        }
    }
    public void RestartGame()
    {
        Debug.Log("게임 재시작");
        if (stages.Count > 0)
        {
            foreach (GameObject stage in stages)
            {
                ResetObjects(stage.transform);
            }
        }
        currentStageNum = 1;
        skillGrade = 0;

        Init();

        Camera.main.GetComponent<CameraController>().Init();

        UIManager.Instance.ShowMainMenu();

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            skillController = player.GetComponent<SkillController>();
            if (skillController != null)
            {
                skillController.enabled = false;

                skillController.releasePointMoveSpeed = 5f;
                skillController.circleShrinkSpeed = 0.7f;
                skillController.circleGrowSpeed = 0.3f;
                skillController.finalDashForce = 10f;
                skillController.UpdateCircleSize(new Vector3(2.5f, 2.5f, 1f)); 
            }
        }
    }
}