using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] GameObject p_bullet;
    [SerializeField] private float Dir = 1f;
    [SerializeField] Transform ShootPos;

    private void Update()
    {
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
        GameObject bullet = Instantiate(p_bullet, gameObject.transform);
        bullet.transform.position = ShootPos.position;
        bullet Bullet = bullet.GetComponent<bullet>();
        Bullet.Shoot(Dir);
    }
}
