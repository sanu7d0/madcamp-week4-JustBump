using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject appleBullet;
    public Transform pos;
    [SerializeField] public float coolTime;
    private float curTime;

    private void Awake() {
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController.onShoot.AddListener(shoot);
    }

    void shoot() {
        if (curTime <= 0)
        {
            ShootingBullet bullet = Instantiate(appleBullet, pos.position, transform.rotation)
                .GetComponent<ShootingBullet>();
            bullet.direction = Camera.main.ScreenToWorldPoint(playerController.mousePos)
                - transform.position;

            curTime = coolTime;
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime;
    }
}
