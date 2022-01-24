using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Fork : Weapon
{
    private Collider2D hitBox;
    private Quaternion originalRotation;

    private enum State {
        Normal,
        Motion
    }
    private State state;

    protected override void Start()
    {
        base.Start();
        hitBox = GetComponentInChildren<Collider2D>();
        originalRotation = transform.localRotation;
    }

    /*void Update() {
        switch (state) {
        case State.Motion:
            HandleMotion();
            break;
        }
    }*/

    public override WeaponUseResult Use()
    {
        if (TryMeleeAttack(hitBox)) {
            // PlayUseMotion();
            base.PlayUseSound();
            return base.Use();
        } else {
            return WeaponUseResult.NoHit;
        }
    }

    protected override void PlayUseMotion() {

    }

    private void HandleMotion() {
        // Rotate back to original rotation
        if (transform.localRotation.z >= originalRotation.z) {
            transform.localRotation = originalRotation;
            state = State.Normal;
        } else {
            transform.localRotation = 
                Quaternion.Lerp(transform.localRotation, originalRotation, 5 * Time.deltaTime);
        }
    }
}
