using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement p_move;
    [SerializeField] private Vector3 p_LastPos;
    [SerializeField] private Vector3 p_MovedPos;
    [SerializeField] private bool isMove;
    [SerializeField] private bool isPlayerWall;

    private void Awake()
    {
        p_move = player.GetComponent<PlayerMovement>();
        p_move.WallColEvent += () =>
        {
            ChangeLastPos();
            Debug.Log("Check");
        };
        p_move.LandingEvent += () =>
        {
            LandingCheck();
        };
        p_move.WallJumpEvent += () =>
        {
            WallJumpCheck();
        };
    }

    void Update()
    {
        CheckPlayerMove();
        Move();
    }

    private void Move()
    {
        if (!isMove) return;
        if (isPlayerWall) return;
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(PlayerInput.instance.dir * -1, 0, 0) * ScrollSpeed * Time.deltaTime;
        transform.position = curPos + nextPos;  
    }
    
    private void CheckPlayerMove()
    {
        if(PlayerInput.instance.dir == 0)
        {
            p_LastPos = player.transform.position;
            p_MovedPos = player.transform.position;
        }
        else
        {
            p_MovedPos = player.transform.position;
        }

        if(Mathf.Abs(p_LastPos.x - p_MovedPos.x) <= 0.001)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
    }

    private void ChangeLastPos()
    {
        p_LastPos = player.transform.position;
    }

    private void WallJumpCheck()
    {
        isPlayerWall = true;
    }

    private void LandingCheck()
    {
        isPlayerWall = false;
    }
}
