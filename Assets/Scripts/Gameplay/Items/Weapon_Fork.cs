using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Fork : Weapon
{
    private Collider2D hitBox;
    private Vector3 originalPosition;

    private enum State {
        Normal,
        Motion
    }
    private State state;

    protected override void Start()
    {
        base.Start();
        hitBox = GetComponentInChildren<Collider2D>();
        originalPosition = transform.localPosition;
    }

    void Update() {
        switch (state) {
        case State.Motion:
            HandleMotion();
            break;
        }
    }

    public override WeaponUseResult Use()
    {
        if (TryMeleeAttack(hitBox)) {
            PlayUseMotion();
            base.PlayUseSound();
            return base.Use();
        } else {
            return WeaponUseResult.NoHit;
        }
    }

    protected override void PlayUseMotion() {
        transform.localPosition = originalPosition;
        transform.localPosition += Vector3.right * 1f;
        state = State.Motion;
    }

    private void HandleMotion() {
        // Rotate back to original rotation
        if (transform.localPosition.x <= originalPosition.x) {
            transform.localPosition = originalPosition;
            state = State.Normal;
        } else {
            transform.localPosition = 
                Vector3.Lerp(transform.localPosition, originalPosition, 5 * Time.deltaTime);
        }
    }
}
