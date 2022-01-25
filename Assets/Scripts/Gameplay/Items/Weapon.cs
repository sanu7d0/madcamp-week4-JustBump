using UnityEngine;
using Photon.Pun;

public enum WeaponUseResult {
    Normal,
    AllUsed,
    NoHit,
    Field
}

[RequireComponent (typeof (AudioSource))]
public abstract class Weapon : MonoBehaviourPunCallbacks, IPunObservable
{   
    protected AudioSource audioSource;
    [SerializeField] protected WeaponObject weapon;

    protected float lastUseTime = 0;

    [SerializeField]
    public int weaponDurability;

    public Sprite weaponSprite {
        get { return weapon.sprite; }
    }


    protected virtual void Start() {
        audioSource = GetComponent<AudioSource>();
        weapon = weapon.GetClone();
        weaponDurability = weapon.durability;
    }

    public virtual WeaponUseResult Use() {
        weaponDurability -= 1;
        if (weaponDurability <= 0) {
            return WeaponUseResult.AllUsed;
        }
        return WeaponUseResult.Normal;
    }

    public virtual WeaponUseResult Use(Vector3 originPosition, Vector3 targetPosition) {
        weaponDurability -= 1;
        if (weaponDurability <= 0) {
            return WeaponUseResult.AllUsed;
        }
        return WeaponUseResult.Normal;
    }

    public virtual WeaponCategory GetWeaponType() {
        return weapon.category;
    }

    protected abstract void PlayUseMotion();

    protected virtual void PlayUseSound() {
        photonView.RPC("_PlayUseSound", RpcTarget.All, weapon.GetRandomUseSound().name);
    }
    [PunRPC]
    protected virtual void _PlayUseSound(string clipName) {
        audioSource.PlayOneShot(AudioManager.Instance.GetAudioClip(clipName));
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting){
            stream.SendNext(this.weaponDurability);
        } else {
            this.weaponDurability = (int)stream.ReceiveNext();
        }
    }
}
