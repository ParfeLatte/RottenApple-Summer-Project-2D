using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] GameObject p_bullet;
    [SerializeField] private float Dir = 1f;
    [SerializeField] Transform ShootPos;
    [SerializeField] private float FireCoolTime;
    [SerializeField] private float FireTime;

    private void Awake()
    {
        FireTime = 0f;
    }

    private void Update()
    {
        FireTime += Time.deltaTime;

        if(PlayerInput.instance.dir == 1f)
        {
            Dir = 1f;
        }
        else if(PlayerInput.instance.dir == -1f)
        {
            Dir = -1f;
        }

        if (PlayerInput.instance.Fire)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (FireTime < FireCoolTime) return;
        FireTime = 0f;
        GameObject bullet = Instantiate(p_bullet, gameObject.transform);
        bullet.transform.position = ShootPos.position;
        bullet Bullet = bullet.GetComponent<bullet>();
        Bullet.Shoot(Dir);
    }
}
