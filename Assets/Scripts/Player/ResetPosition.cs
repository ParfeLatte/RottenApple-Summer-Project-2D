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
//일단 이거 베이스로 만들되, 원래 스크립트에 맞게 바꿔줘야함 + 플레이어 상태도 초기화 해주어야 함 

