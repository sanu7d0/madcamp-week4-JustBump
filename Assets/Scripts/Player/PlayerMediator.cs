using UnityEngine;

public class PlayerMediator : MonoBehaviour
{
    private PlayerInteraction playerInteraction;

    void Awake() {
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    public void AddNewInteractable(Interactable interactable) {
        playerInteraction.AddInteractable(interactable);
    }

    public void RemoveInteractable(Interactable interactable) {
        playerInteraction.RemoveInteractable(interactable);
    }
}
