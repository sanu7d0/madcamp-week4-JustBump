using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class TimerBomb : MonoBehaviour
{
    private float power;
    private float explosionRadius;
    private float delayTime;
    private float igniteTime;

    void Update() {
        if (Time.time >= igniteTime + delayTime) {
            DetonateBomb();
        }
    }

    public void InitBomb(float power, float explosionRadius, float delayTimeInSec) {
        this.power = power;
        this.explosionRadius = explosionRadius;
        this.delayTime = delayTimeInSec;
        igniteTime = Time.time;

        // ** Fatal ** Physics2D seems not workin in thread...
        /*int delayTime = Mathf.FloorToInt(delayTimeInSec * 1000);
        Task.Run( async () => {
            await Task.Delay(delayTime);
            DetonateBomb();
        });*/
    }

    private void DetonateBomb() {
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D target in hitTargets) {
            if (target.TryGetComponent<IBumpable>(out IBumpable targetBump)) {
                // Power decreases along the distance
                targetBump.BumpSelf(ExplosionPower(
                    power, 
                    Vector2.Distance(transform.position, target.transform.position),
                    target.transform.position - transform.position));
                
                // Rigidbody2DExtension.AddExplosionForce(targetRb, power, transform.position, explosionRadius);
            }
        }

        Destroy(this.gameObject);
    }

    private Vector2 ExplosionPower(float power, float distance, Vector2 vector) {
        float expPower = power * Sigmoid(distance);
        return vector.normalized * expPower;
    }

    private float Sigmoid(float x) {
        // range: 1 ~ 0
        return 1.5f - Mathf.Exp(x) / (Mathf.Exp(x) + 1);
    }
}
