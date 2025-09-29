using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string savePath;

    private void Awake()
    {
        // build ver
        savePath = Application.persistentDataPath + "/save.json";
        // debug ver
        //savePath = "Assets/Resources/save.json";

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("저장 완료: " + savePath);
    }

    public SaveData Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("불러오기 완료");
            return data;
        }
        else
        {
            Debug.Log("저장 파일 없음");
            return null;
        }
    }

    public void ResetSave()
    {
        SaveData data = new SaveData { currentStage = 1, skillGrade = 0, clearCnt = GameManager.Instance.clearCnt };
        Save(data);
        return;
    }
}
