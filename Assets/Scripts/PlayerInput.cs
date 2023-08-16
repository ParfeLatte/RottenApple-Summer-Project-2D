using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public float dir { get; private set; }
    public float VertDir { get; private set; }
    public bool Jump { get; private set; }
    public bool ShiftDown { get; private set; }
    public bool ShiftCheck { get; private set; }
    public bool ShiftUp { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool LeftMouseClick { get; private set; }

    public bool Interact { get; private set; }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        dir = 0;
        Jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyInput();
        GetMouseInput();
        GetMousePos();
    }

    private void GetKeyInput()
    {
        dir = Input.GetAxisRaw("Horizontal");
        VertDir = Input.GetAxisRaw("Vertical");
        Jump = Input.GetButtonDown("Jump");
        if (Jump)
        {
            Debug.Log("Space ON");
        }
        ShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        ShiftCheck = Input.GetKey(KeyCode.LeftShift);
        ShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        Interact = Input.GetKeyUp(KeyCode.F);   
    }

    private void GetMouseInput()
    {
        LeftMouseClick = Input.GetMouseButtonDown(0);
    }

    private void GetMousePos()
    {
        //Debug.Log("Check Mouse Position");
        MousePosition = Input.mousePosition;
        MousePosition = Camera.main.ScreenToWorldPoint(MousePosition);
        //Debug.Log(MousePosition);
    }
}
 