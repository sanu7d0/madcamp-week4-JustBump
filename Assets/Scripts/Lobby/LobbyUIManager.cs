using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [SerializeField] private GameObject selectGroup;
    [SerializeField] private GameObject readyGroup;
    [SerializeField]  public UnityEvent onClickedLeaveButtonListener;
    public UnityEvent<string> onCharacterClickedListener;


    void Start() {
        selectGroup.SetActive(true);
        readyGroup.SetActive(false);
    }

    public void onClickedLeaveButton() {
        onClickedLeaveButtonListener.Invoke();
    }

    public void OnCharacterClicked(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        
        string candidatedCharacterName;
        if (ped.pointerCurrentRaycast.gameObject.
            TryGetComponent<CandidateCharacter>(out CandidateCharacter character)) {
            candidatedCharacterName = character.candidateCharacter.name;
        } else {
            candidatedCharacterName = 
                ped.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<CandidateCharacter>()
                    .candidateCharacter.name;
        }
        onCharacterClickedListener.Invoke(candidatedCharacterName);
        
        selectGroup.SetActive(false);
        readyGroup.SetActive(true);
    }

    public void appendChild(GameObject childObject) {
        var parentTansfrom = GetComponent<RectTransform>();
        childObject.transform.SetParent(parentTansfrom, false);
    }
}
