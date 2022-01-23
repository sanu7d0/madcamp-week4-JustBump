using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerMediator playerMediator;

    public List<Tuple<Interactable, int>> interactables;

    void Awake() {
        playerController = GetComponent<PlayerController>();
        playerMediator = GetComponent<PlayerMediator>();
        interactables = new List<Tuple<Interactable, int>>();
    }

    void Start() {
        playerController.onInteract.AddListener(InteractWith);
    }

    void OnDisable() {
        interactables.Clear();
    }

    // TODO: Update 에서 거리 가장 가까운 interactable을 focus 하기?

    private void InteractWith() {
        if (interactables == null || interactables.Count == 0) {
            return;
        }

        interactables[0].Item1.Interact(playerMediator);
    }

    public void AddInteractable(Interactable interactable) {
        interactables.Insert(0, new Tuple<Interactable, int>(interactable, interactable.GetInstanceID()));
    }

    public void RemoveInteractable(Interactable interactable) {
        // Find by id and remove it
        int id = interactable.GetInstanceID();
        bool found = false;
        for (int i=0; i < interactables.Count; i++) {
            if (interactables[i].Item2 == id) {
                interactables.RemoveAt(i);
                found = true;
                break;
            }
        }

        if (found) {
            // Sort by distance in ascending order
            interactables.Sort(
                (a, b) => (DistanceToInteratable(a.Item1) <  DistanceToInteratable(b.Item1)) ? -1 : 1
            );
        }
    }

    private void FocusInteractable() {

    }

    private float DistanceToInteratable(Interactable interactable) {
        return Vector3.Distance(transform.position, interactable.transform.position);
    }
}
