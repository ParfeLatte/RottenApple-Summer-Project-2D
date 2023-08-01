using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float dir { get; private set; }
    public bool Jump { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool LeftMouseClick { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {   
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
        Jump = Input.GetButtonDown("Jump");
        if (Jump)
        {
            Debug.Log("Space ON");
        }
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
 