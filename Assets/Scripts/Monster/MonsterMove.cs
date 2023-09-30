using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_dir;

    private bool isDead;

    private Animator _animator;
    private SpriteRenderer _sprite;
    private Rigidbody2D _rigid;
    private CircleCollider2D _col;


    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _col = GetComponent<CircleCollider2D>();
        m_dir = 1f;
        _sprite.flipX = true;
        isDead = false;
    }

    void Update()
    {
        MobMove();
        CheckDir();
    }


    private void MobMove()
    {
        if (isDead) return;
        _rigid.velocity = new Vector2(m_dir, 0) * m_speed;
    }

    private void CheckDir()
    {
        Vector2 frontVec = new Vector2(_rigid.position.x + m_dir * 0.9f, _rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1);
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    private void Turn() 
    {
      if(m_dir == 1f)
        {
            m_dir = -1f;
            _sprite.flipX = false;
        }
        else
        {
            m_dir = 1f;
            _sprite.flipX = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            StartCoroutine(MonsterDie());
            _col.enabled = false;
        }
    }

    private IEnumerator MonsterDie()
    {
        isDead = true;
        _rigid.velocity = new Vector2(0, 0);
        _animator.SetTrigger("Die");

        yield return new WaitForSeconds(0.4f);

        Destroy(gameObject);
    }
}
