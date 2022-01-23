using UnityEngine;
using UnityEngine.Events;

public class PlayerMediator : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    void Awake() {
        playerManager = GetComponent<PlayerManager>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    public bool IsDead {
        get { return playerManager.isDead; }
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
        playerManager.onDead.Invoke();
    }

    public void AddListenerToOnBumped(UnityAction call) {
        playerManager.onBumped.AddListener(call);
    }

    public void PickUpItem(GameObject newItem) {
        playerCombat.ChangeCurrentWeapon(newItem);
    }

    public void AddScore(int score) {
        playerManager.AddScore(score);
    }
}
