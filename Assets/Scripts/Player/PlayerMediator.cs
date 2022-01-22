using UnityEngine;

public class PlayerMediator : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;

    void Awake() {
        playerManager = GetComponent<PlayerManager>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void AddNewInteractable(Interactable interactable) {
        playerInteraction.AddInteractable(interactable);
    }

    public void RemoveInteractable(Interactable interactable) {
        playerInteraction.RemoveInteractable(interactable);
    }

    public void StartFalling() {
        playerMovement.StartFalling();
    }

    public void InvokeOnFall() {
        playerManager.onFall.Invoke();
    }
}
