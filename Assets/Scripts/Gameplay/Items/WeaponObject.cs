using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponCategory { Melee, Range, Throwable };

[CreateAssetMenu(fileName = "New Weapon Object", menuName = "Weapons")]
public class WeaponObject : ScriptableObject
{
    public new string name;
    public WeaponCategory category;
    public float power;
    public int durability;
    public float coolTime;

    public AudioClip[] useSounds;

    public WeaponObject GetClone() {
        return ScriptableObjectExtension.Clone<WeaponObject>(this);
    }

    public AudioClip GetRandomUseSound() {
        if (useSounds == null || useSounds.Length == 0) {
            Debug.LogError($"No use sound of {name}");
            return null;
        }
        return useSounds[Random.Range(0, useSounds.Length)];
    }
}
