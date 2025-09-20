using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PausePopup;
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

    public void ShowPausePopup()
    {
        PausePopup.SetActive(true);
    }

    public void HidePausePopup()
    {
        PausePopup.SetActive(false);
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
