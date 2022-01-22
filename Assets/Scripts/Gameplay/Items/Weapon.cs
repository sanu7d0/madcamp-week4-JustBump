using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public abstract class Weapon : MonoBehaviour
{   
    [SerializeField] protected WeaponObject weapon;
    protected AudioSource audioSource;

    protected float lastUseTime = 0;

    protected virtual void Start() {
        weapon = weapon.GetClone();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual bool Use() {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
        }
        return true;
    }

    public virtual bool Use(Vector2 targetPosition) {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
        }
        return true;
    }

    public virtual WeaponCategory GetWeaponType() {
        return weapon.category;
    }

    protected virtual void AllUsed() {

    }

    protected virtual void PlayUseMotion() {

    }

    protected virtual void PlayUseSound() {
        audioSource.PlayOneShot(weapon.GetRandomUseSound());
    }

    protected bool TryMeleeAttack(Collider2D hitBox) {
        // Check cooltime
        if (Time.time < lastUseTime + weapon.coolTime) {
            Debug.Log("Fist not ready yet");
            return false;
        }

        Collider2D[] hitTargets = new Collider2D[16];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        int hitCount = hitBox.OverlapCollider(contactFilter, hitTargets);

        bool attacked = false;
        foreach(Collider2D target in hitTargets) {
            if (target == null) break; // if null, all the next is null, so break
            
            // Check the target is not itself
            if (target.transform.GetInstanceID() == transform.root.GetInstanceID()) {
                Debug.Log("Invalid target");
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

        return attacked;
    }
}
