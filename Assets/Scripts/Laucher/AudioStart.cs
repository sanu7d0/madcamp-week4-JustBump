using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioStart : MonoBehaviour
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

    void Start()
    {
        BGM = gameObject.AddComponent<AudioSource>();
        BGM.loop = true;
        if (BGMList.Length > 0) PlayBGM(BGMList[0].name);

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        
        if (SceneManager.GetActiveScene().name != "Lobby" && SceneManager.GetActiveScene().name != "Launcher") {
            Destroy(gameObject);
        }
        

    }

    public static void ReplaceMusic() {
        BGM.Stop();
        Destroy(BGM);
    }

    public void PlayBGM(string name)
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
