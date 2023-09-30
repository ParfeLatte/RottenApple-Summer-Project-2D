using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float _dir;
    [SerializeField] private float _speed;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float Dir)
    {
        _dir = Dir;
        BulletMove();
        StartCoroutine(BulletDestroy());
    }
    private void BulletMove()
    {
        _rigid.velocity = new Vector2(_dir, 0) * _speed;
    }

    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void BulletCollision()
    {
        StopCoroutine(BulletDestroy());
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BulletCollision();
        }
    }
}
