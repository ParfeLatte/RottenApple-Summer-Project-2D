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

    [SerializeField] private float moveSpeed;
    [SerializeField] private float wallJumpPower;
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

    [SerializeField] private bool isJump;
    [SerializeField] private bool isFloor;
    [SerializeField] private bool isWire;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isGrab;
    [SerializeField] private int JumpCount;
    [SerializeField] private bool isWallJump;
    [SerializeField] private int wallCondition;//�� ���� 0: ���� ���� ����, 1:���� �������, 2:���� ������, 3: ���� ������ ���ϴ� ���� 4: 3�� ���¿��� ����Ƽ� õõ�� ������ 5: ������

    void Awake()
    {
        hook = GetComponentInChildren<Hook>();
        playerRigid = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        p_Reset = GetComponent<ResetPosition>();
        joint.enabled = false;
        isWire = false;
        isWallJump = false;
        isFloor = false;
        wallTime = 0;
    }

    private void Start()
    {
        p_Reset.ResetPlayer += CheckFloor;
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
        yHoldMove();
        yFellMove();
        PutDownWall();
        CheckWire();

    }

    public float returnMoveSpeed()
    {
        return moveSpeed;
    }

    #region Move, Jump
    private void xMove()
    {
        if (wallCondition == 5) return;
        moveDistance = new Vector2(PlayerInput.instance.dir * moveSpeed, playerRigid.velocity.y);
        if (!isWire)
        {
            playerRigid.velocity = moveDistance; 
        }
        else if (isWire)
        {
            playerRigid.AddForce(moveDistance * Time.deltaTime * 10);
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

            jumpCounts[JumpCount].SetActive(false);
            if(JumpEvent != null)
            {
                JumpEvent();
            }
        }
        
    }

    private IEnumerator WallJump()
    {
        //isGrab = false;
        //isWall = false;
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
        yield return new WaitForSeconds(0.15f);
        wallCondition = 0;
    }

    private void CheckFloor()
    {
        RaycastHit2D Floor;
        Floor = Physics2D.Raycast(transform.position, Vector2.down, 0.75f, LayerMask.GetMask("Floor"));
        if(Floor != null)
        {
            JumpCount = 2;
            CheckWallDir();
            foreach (GameObject i in jumpCounts)
            {
                i.SetActive(true);
            }
            wallTime = 0f;
            isFloor = true;
            if(LandingEvent != null)
            {
                LandingEvent();
            }
            Debug.Log("�ٴ��Դϴ�");
        }
        else
        {
            Debug.Log("�����Դϴ�");
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
        jumpCounts[JumpCount - 1].SetActive(true);
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
    private void HoldWall()
    {
        //���� �پ������� Shift�� ������ ���� ����
        //if (!isWall) return;
        if (wallCondition == 1 || wallCondition == 3)
        {
            if (PlayerInput.instance.ShiftDown)
            {
                //isGrab = true;
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
                Debug.Log("�����");
                //���� �ִϸ��̼� ������ �ʿ��� ���� �߰�
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
                Debug.Log("����� �پ�����");
            }
        }
    }
    private void yHoldMove()
    {
        //if (!isWall) return;
        //if (!isGrab) return;
        if (wallCondition != 2) return; 
        if (wallTime >= 0.7f)
        {
            //isGrab = false;
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
            walljumpDir = 1f;
            //wallCondition = 1;
            Debug.Log("���ʿ� ����");
            return;
        }
        else if(right.collider != null)
        {
            walljumpDir = -1f;
            //wallCondition = 1;  
            Debug.Log("�����ʿ� ����");
            return;
        }
        else
        {   
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
            Debug.Log("������ ������"); 
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
//    //Debug.Log("���� �浹����: " + hit.point);
//    ColPos.position = Pos;
//    joint.connectedAnchor = Pos;
//    anchorPos = Pos;
//    lr.SetPosition(1, anchorPos);
//    JumpCount = 1;
//    jumpCounts[JumpCount - 1].SetActive(true);
//}
//else
//{
//    Debug.Log("�浹����");
//}