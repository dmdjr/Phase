using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PausePopup;

    public GameObject skillGuideSprite; // Skill UnLock시 뜨는 문구
    private bool isSkillUnlocked = false; 
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        MainMenu.SetActive(true);

        if (skillGuideSprite != null)
        {
            skillGuideSprite.SetActive(false);
        }
    }
    private void Update()
    {
        if (!isSkillUnlocked || GameManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.currentStageNum == 5)
        {
            skillGuideSprite.SetActive(true);
        }
        else
        {
            skillGuideSprite.SetActive(false);
        }
    }
    public void UnlockSkill()
    {
        isSkillUnlocked = true;
    }
    public void ShowPausePopup()
    {
        PausePopup.SetActive(true);
    }

    public void HidePausePopup()
    {
        PausePopup.SetActive(false);
    }

    public void OnClickNewGame()
    {
        MainMenu.SetActive(false);
        GameManager.Instance.currentStageNum = 1;

        GameManager.Instance.Init();
        Camera.main.GetComponent<CameraController>().Init();
    }

    public void OnClickLoadGame()
    {
        MainMenu.SetActive(false);
        SaveData loaded = SaveManager.Instance.Load();
        if (loaded != null)
        {
            GameManager.Instance.currentStageNum = loaded.currentStage;
        }

        GameManager.Instance.Init();
        Camera.main.GetComponent<CameraController>().Init();
    }

    public void OnClickSave()
    {
        SaveData save = new SaveData();
        save.currentStage = GameManager.Instance.currentStageNum;
        SaveManager.Instance.Save(save);
    }

    public void OnClickExit()
    {
        Application.Quit(); // 실제 빌드에서 게임 종료
    }

    public void OnClickClose()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
    }
}
