using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LoadButton;
    public GameObject PausePopup;
    public GameObject EndingLogo;

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
        Init();
    }

    public void Init()
    {
        SaveData loaded = SaveManager.Instance.Load();
        if (loaded == null)
        {
            LoadButton.GetComponent<Button>().enabled = false;
            LoadButton.GetComponentsInChildren<Image>()[1].color = Color.gray;
        }
        else
        {
            LoadButton.GetComponent<Button>().enabled = true;
            LoadButton.GetComponentsInChildren<Image>()[1].color = Color.white;
        }

        PausePopup.SetActive(false);

        if (skillGuideSprite != null)
        {
            skillGuideSprite.SetActive(false);
        }
        if (EndingLogo != null)
        {
            EndingLogo.SetActive(false);
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
        GameManager.Instance.skillGrade = 0;

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
            GameManager.Instance.skillGrade = loaded.skillGrade;
        }

        GameManager.Instance.Init();
        Camera.main.GetComponent<CameraController>().Init();
    }

    public void OnClickSave()
    {
        SaveData save = new SaveData();
        save.currentStage = GameManager.Instance.currentStageNum;
        save.skillGrade = GameManager.Instance.skillGrade;
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
    public void StartEnding(Tilemap tilemapToPass)
    {
        if (EndingLogo != null)
        {
            EndingLogo.SetActive(true);

            Ending endStart = EndingLogo.GetComponent<Ending>();
            if (endStart != null)
            {
                endStart.StartAnimation(tilemapToPass);
            }
        }
    }
    public void ShowMainMenu()
    {
        if (EndingLogo != null)
        {
            EndingLogo.SetActive(false);
        }

        PausePopup.SetActive(false);
        skillGuideSprite.SetActive(false);

        MainMenu.SetActive(true);

        GameManager.Instance.ChangeState(GameManager.GameState.Ready);
    }
}
