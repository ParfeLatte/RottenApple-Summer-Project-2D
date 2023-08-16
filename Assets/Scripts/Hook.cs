using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    CollisionCheck wireCol;
    PlayerMovement p_move;
    CircleCollider2D col;

    public Transform hook;
    public GameObject wire;
    [SerializeField] private Transform Center;
    [SerializeField] private Vector2 pos;   
    [SerializeField] private Vector2 Dir;
    [SerializeField] private Vector2 WireDir;
    [SerializeField] private float radius;
    [SerializeField] private float distance;
    private int condition; // 0:idle, 1:wire½ô 2:µ¹¾Æ¿È 3:¿ÍÀÌ¾î¿¬°áµÊ
    

    void Awake()
    {
        wireCol = GetComponentInChildren<CollisionCheck>();
        p_move = GetComponentInParent<PlayerMovement>();
        radius = Vector2.Distance(Center.position, hook.position);
        col = GetComponentInChildren<CircleCollider2D>();
        condition = 0;
        wireCol.OnCollision += SetWire;
        p_move.CutWireEvent += CutandReturn;
        col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {       
        CheckHook();
        Shootwire();
        Checkwire();
        returnWire();
    }

    private void CheckHook()
    {
        Dir = (PlayerInput.instance.MousePosition - new Vector2(Center.position.x, Center.position.y)).normalized;
        pos = Dir * radius;
        distance = Vector2.Distance(Center.position, wire.transform.position);
        if (condition == 0 || condition == 3)
        {
            hook.transform.localPosition = pos;
        }

    }

    private void Shootwire()
    {
        if (condition != 0) return;
        if (PlayerInput.instance.LeftMouseClick)
        {
            condition = 1;
            WireDir = Dir;
            col.enabled = true;
            Checkwire();
        }     
    }

    private void Checkwire()
    {
        if (condition != 1) return;
        Vector2 nextPos = WireDir * 40f * Time.deltaTime;
        if (distance >= 7f)
        {
            WireDir = (hook.position - wire.transform.position).normalized;
            col.enabled = false;
            condition = 2;
        }
        else
        {
            wire.transform.position += (Vector3)nextPos;
        }

    }

    private void returnWire()
    {
        if (condition != 2) return;
        float wirehookdist = Vector2.Distance(hook.position, wire.transform.position);
        Vector2 nextPos = WireDir * 60f * Time.deltaTime;
        wire.transform.position += (Vector3)nextPos;
        if(wirehookdist <= 1f)
        {
            wire.transform.position = hook.position;
            condition = 0;
         }
    }

    public void CutandReturn()
    {
        wire.transform.parent = hook;
        WireDir = (hook.position - wire.transform.position).normalized;
        col.enabled = false;
        condition = 2;
    }

    public void SetWire()
    {
        condition = 3;
        wire.transform.position = wireCol.ColPos;
        p_move.ShootWire(wire.transform.position);
        wire.transform.parent = null;
    }
}
