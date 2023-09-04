using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    public System.Action ResetPlayer;

    [SerializeField] private Vector2 resetPos;

    private void Awake()
    {
        resetPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("resetCol"))
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        transform.position = resetPos;
        if(ResetPlayer != null)
        {
            ResetPlayer();
        }
    }

    private void SetResetPos()
    {
        resetPos = transform.position;
    }
}
//�ϴ� �̰� ���̽��� �����, ���� ��ũ��Ʈ�� �°� �ٲ������ + �÷��̾� ���µ� �ʱ�ȭ ���־�� �� 

