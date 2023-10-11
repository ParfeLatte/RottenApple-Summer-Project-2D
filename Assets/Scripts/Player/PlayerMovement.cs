using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public System.Action CutWireEvent;
    public System.Action WallColEvent;
    public System.Action WallJumpEvent;
    public System.Action LandingEvent;
    public System.Action JumpEvent;

    private Rigidbody2D playerRigid;
    private DistanceJoint2D joint;
    private LineRenderer lr;
    private Hook hook;
    private ResetPosition p_Reset;
    private Animator p_anim;
    private SpriteRenderer p_spr;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float walljumpDir;
    [SerializeField] Vector2 moveDistance;
    public Transform ColPos;
    public RaycastHit2D hit;
    public Vector2 raydir;
    

    public Vector2 shootPos;
    public Vector2 anchorPos;


    public List<GameObject> jumpCounts = new List<GameObject>();

    [SerializeField] private float wallTime;
    private bool isJump;
    private bool isFloor;
    private bool isWire;
    private bool isWall;
    private bool isGrab;
    private int JumpCount;
    private bool isWallJump;
    [SerializeField] private int wallCondition;//벽 상태 0: 벽에 붙지 않음, 1:벽에 닿아있음, 2:벽을 붙잡음, 3: 벽을 붙잡지 못하는 상태 4: 3의 상태에서 붙잡아서 천천히 내려옴 5: 벽점프

    [Header("WallJump")]
    [SerializeField] private float wallJumpPower;
    [SerializeField] private float WallJumpTime;

    void Awake()
    {
        hook = GetComponentInChildren<Hook>();
        playerRigid = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        p_Reset = GetComponent<ResetPosition>();
        p_anim = GetComponent<Animator>();
        p_spr = GetComponent<SpriteRenderer>();
        joint.enabled = false;
        isWire = false;
        isWallJump = false;
        isFloor = false;
        wallTime = 0;
    }

    private void Start()
    {
        p_Reset.ResetPlayer += CheckFloor;
        p_anim.SetBool("isWalk", false);
    }

    void Update()
    {
        Debug.DrawRay(transform.position, raydir.normalized * 5.5f, Color.red);
        Debug.DrawRay(transform.position, Vector2.right * 0.55f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 0.75f, Color.red);
        if (playerRigid.velocity.y >= 20) { playerRigid.velocity = new Vector2(playerRigid.velocity.x , 20f); }

        HoldWall();
        Jump();
        xMove();
        SpriteSwap();
        yHoldMove();
        yFellMove();
        PutDownWall();
        CheckWire();

    }

    public float returnMoveSpeed()
    {
        return moveSpeed;
    }

    private void SpriteSwap()
    {
        if (isWall) return;
        if(PlayerInput.instance.dir == 1f)
        {
            p_spr.flipX = true;
        }
        else if(PlayerInput.instance.dir == -1f)
        {
            p_spr.flipX = false;
        }
    }

    #region Move, Jump
    private void xMove()
    {
        if (wallCondition == 5) return;
        if (PlayerInput.instance.dir == 0)
        {
            p_anim.SetBool("isWalk", false);
        }
        else
        {
            p_anim.SetBool("isWalk", true);
        }

        moveDistance = new Vector2(PlayerInput.instance.dir * moveSpeed, playerRigid.velocity.y);
        if (!isWire)
        {
            playerRigid.velocity = moveDistance; 
        }
        else if (isWire)
        {
            playerRigid.AddForce(moveDistance * Time.deltaTime * 13);
        }
    }
    private void Jump()
    {
        if (JumpCount == 0) return;
        if (PlayerInput.instance.Jump)
        {
            if (isWire)
            {
                Debug.Log("WireJump");
                playerRigid.velocity = Vector2.up * jumpForce + (playerRigid.velocity * 0.8f);
                JumpCount--;
                if (JumpCount != 0)
                {
                    jumpCounts[JumpCount - 1].SetActive(false);
                }
                CutWire();
            }
            else if (wallCondition != 0 && !isFloor)
            {
                StartCoroutine(WallJump());
            }
            else
            {
                Debug.Log("Jump");
                playerRigid.velocity = Vector2.up * jumpForce;
                JumpCount--;
            }

            p_anim.SetBool("isJump", true);
            p_anim.SetBool("isWall", false);
            jumpCounts[JumpCount].SetActive(false);
            if(JumpEvent != null)
            {
                JumpEvent();
            }
        }
        
    }

    private IEnumerator WallJump()
    {
        wallCondition = 5;
        if(WallJumpEvent != null)
        {
            WallJumpEvent();
        }
        Debug.Log("Wall Jump");
        playerRigid.velocity = new Vector2(0.8f * walljumpDir * wallJumpPower, 1.2f * wallJumpPower);
        JumpCount--;
        //if (JumpCount != 0)
        //{
        //    jumpCounts[JumpCount - 1].SetActive(false);
        //}
        yield return new WaitForSeconds(WallJumpTime);
        wallCondition = 0;
    }

    private void CheckFloor()
    {
        RaycastHit2D Floor;
        Floor = Physics2D.Raycast(transform.position, Vector2.down, 1.4f, LayerMask.GetMask("Floor"));
        Debug.DrawRay(transform.position, Vector2.down * 1.4f, Color.red);
        if(Floor != null)
        {
            p_anim.SetBool("isJump", false);
            p_anim.SetBool("isWall", false);
            JumpCount = 2;
            CheckWallDir();
            foreach (GameObject i in jumpCounts)
            {
                Debug.Log("점프 초기화");
                i.SetActive(true);
            }
            wallTime = 0f;
            isFloor = true;
            if(LandingEvent != null)
            {
                LandingEvent();
            }
            Debug.Log("바닥입니다");
        }
        else
        {
            Debug.Log("공중입니다");
            return;
        }
    }
    #endregion
    #region Wire
    public void ShootWire(Vector2 Pos)
    {
        joint.enabled = true;
        isWire = true;
        ColPos.position = Pos;
        joint.connectedAnchor = Pos;
        anchorPos = Pos;
        JumpCount = 1;
        jumpCounts[JumpCount].SetActive(true);
        p_anim.SetBool("isJump", true);
    }
    private void CheckWire()
    {
        shootPos = hook.hook.position;
        anchorPos = hook.wire.transform.position;
        lr.SetPosition(0, shootPos);
        lr.SetPosition(1, anchorPos);
    }

    private void CutWire()
    {
        joint.enabled = false;
        isWire = false;
        if (CutWireEvent != null) CutWireEvent();
    }




    #endregion
    #region WallMove

    private void WallSpriteFlip()
    {
        if(walljumpDir == 1f)
        {
            p_spr.flipX = true;
        }
        else if(walljumpDir == -1f){
            p_spr.flipX = false;
        }
    }

    private void HoldWall()
    {
        //벽에 붙어있을때 Shift를 누르면 벽에 붙음
        //if (!isWall) return;
        if (wallCondition == 1 || wallCondition == 3)
        {
            if (PlayerInput.instance.ShiftDown)
            {
                //isGrab = true;
                isWall = true;
                switch (wallCondition)
                {
                    case 1:
                        wallCondition = 2;
                        break;
                    case 3:
                        wallCondition = 4;
                        break;
                    default:
                        break;
                }
                Debug.Log("잡았음");
                WallSpriteFlip();
                p_anim.SetBool("isWall", true);
                p_anim.SetBool("isJump", false);
            }
        }
    }

    private void PutDownWall()
    {
        if (wallCondition == 2 || wallCondition == 4)
        {
            if (PlayerInput.instance.ShiftUp)
            {
                switch (wallCondition)
                {
                    case 2:
                        wallCondition = 1;
                        break;
                    case 4:
                        wallCondition = 3;
                        break;
                    default:
                        return;
                }
                //isGrab = false;
                Debug.Log("안잡고 붙어있음");
            }
        }
    }
    private void yHoldMove()
    {
        if (wallCondition != 2) return; 
        if (wallTime >= 0.7f)
        {
            wallCondition = 4;
            return;
        }
        if (PlayerInput.instance.VertDir == 1f)
        {
            //isGrab = true;
            moveDistance = new Vector2(0, PlayerInput.instance.VertDir * 4);
            playerRigid.velocity = moveDistance;
            wallTime += Time.deltaTime;
        }
        else
        {   
            playerRigid.velocity = new Vector2(0, 0);
        }
        //if (!isGrab) return;
        //moveDistance = new Vector2(0, playerinput.VertDir * moveSpeed);
        //playerRigid.velocity = moveDistance;
    } 
    private void yFellMove()
    {
        if (wallCondition != 4) return;
        playerRigid.velocity += Vector2.down * 1/5;
        if(playerRigid.velocity.y <= -7f)
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, -7f);
        }
    }

    private void nextWallCheck()
    {
        if (wallCondition == 0 || wallCondition == 5)
        {
            if (PlayerInput.instance.ShiftCheck)
            {
                wallCondition = 2;
                //walljumpDir *= -1;
                wallTime = 0;
            }
            else
            {
                wallCondition = 1;
                wallTime = 0;
            }
            p_anim.SetBool("isJump", false);
        }
    }

    private void CheckWallDir()
    {
        if (wallCondition == 5) return;
        //isWall = true;
        RaycastHit2D left;
        RaycastHit2D right;
        left = Physics2D.Raycast(transform.position, Vector2.left, 0.75f, LayerMask.GetMask("Wall"));       
        right = Physics2D.Raycast(transform.position, Vector2.right, 0.75f, LayerMask.GetMask("Wall"));
        if (left.collider != null)
        {
            isWall = true;
            walljumpDir = 1f;
            //wallCondition = 1;
            Debug.Log("왼쪽에 붙음");
            p_spr.flipX = true;
            p_anim.SetBool("isWall", true);
            return;
        }
        else if(right.collider != null)
        {
            isWall = true;
            walljumpDir = -1f;
            //wallCondition = 1;  
            Debug.Log("오른쪽에 붙음");
            p_spr.flipX = false;
            p_anim.SetBool("isWall", true);
            return;
        }
        else
        {
            isWall = false;
            p_anim.SetBool("isWall", false);
            walljumpDir = 0f;
            wallCondition = 0;
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (WallColEvent != null)
            {
                WallColEvent();
            }
            CheckWallDir();
            nextWallCheck();
            JumpCount = 2;
            //jumpCounts[JumpCount - 1].SetActive(true);
            foreach (GameObject i in jumpCounts)
            {
                i.SetActive(true);
            }
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            CheckFloor();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (WallColEvent != null)
            {
                WallColEvent();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (WallColEvent != null)
            {
                WallColEvent();
            }
            if (wallCondition != 5)
            {
                wallCondition = 0;
            }
            isWall = false;
            p_anim.SetBool("isWall", false);
            Debug.Log("벽에서 떨어짐"); 
        }
        if (collision.gameObject.CompareTag("Floor"))
        {
            isFloor = false;
        }
    }
}
//raydir = playerinput.MousePosition - new Vector2(transform.position.x, transform.position.y);
//hit = Physics2D.Raycast(transform.position, raydir.normalized, 5.5f, LayerMask.GetMask("GrapplingObj"));
//if(hit.collider != null)
//{
//    joint.enabled = true;
//    lr.enabled = true;
//    isWire = true;
//    //Debug.Log("레이 충돌지점: " + hit.point);
//    ColPos.position = Pos;
//    joint.connectedAnchor = Pos;
//    anchorPos = Pos;
//    lr.SetPosition(1, anchorPos);
//    JumpCount = 1;
//    jumpCounts[JumpCount - 1].SetActive(true);
//}
//else
//{
//    Debug.Log("충돌안함");
//}