using UnityEngine;
using Photon.Pun;

public enum WeaponUseResult {
    Normal,
    AllUsed,
    NoHit
}

[RequireComponent (typeof (AudioSource))]
public abstract class Weapon : MonoBehaviourPunCallbacks
{   
    [SerializeField] protected WeaponObject weapon;
    [SerializeField] protected GameObject emptyFieldDrop;
    protected AudioSource audioSource;

    protected float lastUseTime = 0;

    public int weaponDurability {
        get { return weapon.durability; }
    }

    public Sprite weaponSprite {
        get { return weapon.sprite; }
    }

    protected virtual void Start() {
        weapon = weapon.GetClone();
        audioSource = GetComponent<AudioSource>();
    }

    public virtual WeaponUseResult Use() {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
            return WeaponUseResult.AllUsed;
        }
        return WeaponUseResult.Normal;
    }

    public virtual WeaponUseResult Use(Vector3 originPosition, Vector3 targetPosition) {
        weapon.durability -= 1;
        if (weapon.durability <= 0) {
            AllUsed();
            return WeaponUseResult.AllUsed;
        }
        return WeaponUseResult.Normal;
    }

    public virtual void WeaponToFieldDrop(Vector3 pos) {
        photonView.RPC("_WeaponToFieldDrop", RpcTarget.All, pos);
    } 

    [PunRPC]
    protected virtual void _WeaponToFieldDrop(Vector3 pos) {
        GameObject emptyDrop = 
            PhotonNetwork.Instantiate(emptyFieldDrop.name, pos, Quaternion.identity);
        emptyDrop.name = weapon.name;
        emptyDrop.GetComponent<SpriteRenderer>().sprite = weapon.sprite;
        FieldItem fi = emptyDrop.GetComponent<FieldItem>();
        fi.itemPrefab = gameObject;
        fi.itemObject = gameObject;
        fi.durability = weapon.durability;

        gameObject.SetActive(false);
    }

    public virtual WeaponCategory GetWeaponType() {
        return weapon.category;
    }

    protected virtual void AllUsed() {

    }

    protected virtual void PlayUseMotion() {

    }

    protected virtual void PlayUseSound() {
        photonView.RPC("_PlayUseSound", RpcTarget.All);
    }
    [PunRPC]
    protected virtual void _PlayUseSound() {
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
                // Debug.Log("Invalid target");
		        continue;
            } else {
                IBumpable bumpTarget = target.GetComponent<IBumpable>();
                if(bumpTarget != null) { 
                    Vector2 force = (target.transform.position - transform.position).normalized * weapon.power;
					// Debug.Log($"Player hit {target.name} with {force.SqrMagnitude()} force");

                    if(bumpTarget is IPlayer) {
                        var holder = transform.root.GetComponent<IPlayer>();
                        bumpTarget.BumpSelf(force, holder);
                    } else {
                        bumpTarget.BumpSelf(force);
                    }

                    attacked = true;
				}
            }

        }

        return attacked;
    }
}
