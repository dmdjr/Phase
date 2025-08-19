using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }

    private GameManager _game = new GameManager();
    
    public static GameManager Game { get { return Instance?._game; } }
    private ResourceManager _resource = new ResourceManager();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();
    // private StageManager _stage = new StageManager();
    // private InversionManager _inversion = new InversionManager();

    public static ResourceManager Resource { get { return Instance?._resource; } }
    public static SoundManager Sound { get { return Instance?._sound; } }
    public static UIManager UI { get { return Instance?._ui; } }
    // public static StageManager Stage { get { return Instance?._stage; } }
    // public static InversionManager Inversion { get { return Instance?._inversion; } }

    public static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // 초기화
            s_instance = go.GetComponent<Managers>();
        }
    }
}
