using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LauncherBGM : Singleton<LauncherBGM>
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    // Inspector 에표시할 배경음악 목록
    public BgmType[] BGMList;

    private static AudioSource BGM;
    private string NowBGMname = "";

    public bool isPlayingMusic;

    protected override void Awake()
    {
        base.Awake();
        isPlayingMusic = false;

        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;

        SceneManager.activeSceneChanged += CheckTurnOffMusic;


        DontDestroyOnLoad(gameObject);
    }

    public void StartPlaying(AudioClip clip)
    {
        isPlayingMusic = true;
        BGM.clip = clip;
        BGM.Play();
    }

    private void CheckTurnOffMusic(Scene current, Scene next) {
        Debug.Log($"{current.name} -> {next.name}");
        if (next.name != "Lanucher" && next.name != "Lobby") {
            Destroy(gameObject);
        }
        if (next.name == "Arena") {
            Destroy(gameObject);
        }
    }

    void Update() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Arena") {
            Destroy(gameObject);
        }
    }

    public static void ReplaceMusic() {
        BGM.Stop();
        Destroy(BGM);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
            }
    }

}
