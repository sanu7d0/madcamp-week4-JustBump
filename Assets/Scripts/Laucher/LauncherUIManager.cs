using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LauncherUIManager : Singleton<MonoBehaviour>
{

    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    public UnityEvent onConnectButtonClickedListener;

    public void OnControlPanel() { 
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void OnProgressLabel() {
        controlPanel.SetActive(false);
        progressLabel.SetActive(true);
    }

    public void OnCharacterClicked(BaseEventData data)
    {
        OnProgressLabel();
        PointerEventData ped = (PointerEventData)data;
        string candidatedCharacterName = ped.pointerCurrentRaycast.gameObject.GetComponent<CandidateCharacter>().candidateCharacter.name;
    }

    public void OnConnnectButtonClicked() {
        onConnectButtonClickedListener.Invoke();
    }
}
