using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public System.Action CutWireEvent;

    private PlayerInput playerinput;
    private Rigidbody2D playerRigid;
    private DistanceJoint2D joint;
    private LineRenderer lr;
    private Hook hook;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] Vector2 moveDistance;
    public Transform ColPos;
    public RaycastHit2D hit;
    public Vector2 raydir;

    public Vector2 shootPos;
    public Vector2 anchorPos;


    public List<GameObject> jumpCounts = new List<GameObject>();

    private bool isJump;
    private bool isWire;
    [SerializeField] private int JumpCount;

    // Start is called before the first frame update
    void Awake()
    {
        hook = GetComponentInChildren<Hook>();
        playerinput = GetComponent<PlayerInput>();
        playerRigid = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        joint.enabled = false;
        isWire = false;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Move();
        CheckWire();
        Debug.DrawRay(transform.position, raydir.normalized * 5.5f, Color.red);
        if (playerRigid.velocity.y >= 20) { playerRigid.velocity = new Vector2(playerRigid.velocity.x , 20f); }
    }

    private void Move()
    {
        moveDistance = new Vector2(playerinput.dir * moveSpeed, playerRigid.velocity.y);
        if (!isWire)
        {
            playerRigid.velocity = moveDistance;
        }
        else if (isWire)
        {
            
            playerRigid.AddForce(moveDistance * Time.deltaTime * 10);
        }
    }

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

    private void Jump()
    {
        if (JumpCount == 0) return;
        if (playerinput.Jump)
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
            else
            {
                Debug.Log("Jump");
                playerRigid.velocity = Vector2.up * jumpForce;
                JumpCount--;
            }

            jumpCounts[JumpCount].SetActive(false);
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            JumpCount = 2;
            foreach(GameObject i in jumpCounts)
            {
                i.SetActive(true);
            }
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