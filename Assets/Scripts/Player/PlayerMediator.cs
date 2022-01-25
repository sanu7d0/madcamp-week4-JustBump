using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMediator : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerInteraction playerInteraction;
    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;
    private PlayerInventory playerInventory;

    void Awake() {
        playerManager = GetComponent<PlayerManager>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCombat = GetComponent<PlayerCombat>();
        playerInventory = GetComponent<PlayerInventory>();
    }

    public bool IsDead {
        get { return playerManager.isDead; }
    }

    public UnityEvent onWeaponChange {
        get { return playerInventory.onWeaponChange; }
    }

    public Weapon[] weapons {
        get { return playerInventory.weapons; }
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
        playerInventory.SetWeaponAt(newItem);
    }

    public void AddScore(int score) {
        playerManager.AddScore(score);
    }

    public void AddForce(Vector2 force) {
        playerManager.BumpSelf(force);
    }

    public void IncreaseSpeed(float speed)
    {
        playerMovement.IncreaseSpeed(speed);
    }

    public void InitSpeed() {
        playerMovement.InitSpeed();
    }
}
