using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float _dir;
    [SerializeField] private float _speed;

    private Animator _animator;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
        StartCoroutine(BulletBoom());
    }

    private IEnumerator BulletBoom()
    {
        _animator.SetTrigger("Boom");
        _rigid.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    private void BulletCollision()
    {   
        StopCoroutine(BulletDestroy());
        StartCoroutine(BulletBoom());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            BulletCollision();
        }
    }
}
