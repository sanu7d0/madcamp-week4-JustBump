using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Axe : Weapon
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
        originalRotation = transform.rotation;
    }

    void Update() {

    }

    public override bool Use()
    {
        // check cooltime
        if (Time.time < lastUseTime + weapon.coolTime) {
            // Debug.Log("Fist not ready yet");
            return false;
        }

        // Attack
        Collider2D[] hitTargets = new Collider2D[16];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        int hitCount = hitBox.OverlapCollider(contactFilter, hitTargets);

        bool attacked = false;
        foreach(Collider2D target in hitTargets) {
            if (target == null) break; // if null, all the next is null, so break
            
            // Check the target is not itself
            if (target.transform.GetInstanceID() == transform.root.GetInstanceID()) {
                // Debug.Log("Invalid target");
		        continue;
            } else {
                IBumpable bumpTarget = target.GetComponent<IBumpable>();
                if(bumpTarget != null) { 
					Debug.Log("Player hit " + target.name);
                    // TODO: Hit origin 지정?
                    bumpTarget.BumpSelf(
                        (target.transform.position - transform.position).normalized
                        * weapon.power);
                    
                    attacked = true;
				}
            }
        }

        // If attacked -> Play attack effect
        if (attacked) {
            PlayUseMotion();
            PlayUseSound();
        }

        return base.Use();
    }

    protected override void PlayUseMotion()
    {
        // base.PlayUseMotion();

        // rotate and back to original rotation
        transform.Rotate(0, 0, -50f);
    }

    private void HandleMotion() {
        if (transform.rotation.z >= originalRotation.z) {
            transform.rotation = originalRotation;
            state = State.Normal;
        } else {
            transform.rotation = 
                Quaternion.Lerp(transform.rotation, originalRotation, 5 * Time.deltaTime);
        }
    }

    protected override void PlayUseSound()
    {
        base.PlayUseSound();
    }

    protected override void AllUsed()
    {
        // Do Something

        base.AllUsed();
    }
    
    /*void OnDrawGizmosSelected() {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }*/
}
