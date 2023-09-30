using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    [SerializeField] private Transform[] Backgrounds;
    [SerializeField] private int LeftCheck;
    [SerializeField] private int RightCheck;
    [SerializeField] private float scrollAmount;
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 LeftCheckPos;
    [SerializeField] private Vector3 RightCheckPos;

    void Awake()
    {
        scrollAmount = Backgrounds[1].position.x - Backgrounds[0].position.x;
        LeftCheck = 1;
        RightCheck = 0;
        LeftCheckPos = Backgrounds[LeftCheck].position;
        RightCheckPos = Backgrounds[RightCheck].position;
    }
    void Update()
    {
        Scrolling();
    }

    private void Scrolling() 
    {
        if (PlayerInput.instance.dir == 0) return;

        if (PlayerInput.instance.dir < 0)
        {
            if (Vector3.Distance(Player.position, Backgrounds[LeftCheck].position) >= scrollAmount)
            {
                LeftReposition(PlayerInput.instance.dir);
            }
        }
        else if(PlayerInput.instance.dir > 0)
        {
            if (Vector3.Distance(Player.position, Backgrounds[RightCheck].position) >= scrollAmount)
            {
                RightReposition(PlayerInput.instance.dir);
            }
        }
    } 
    private void LeftReposition(float dir)
    {
        Vector3 Nextpos = new Vector3(scrollAmount * 2f * dir, 0, 0) ;
        Backgrounds[LeftCheck].position = Nextpos + Backgrounds[LeftCheck].position;
        if (LeftCheck == 0)
        {
            LeftCheck++;
            RightCheck--;
        }
        else
        {
            LeftCheck = 0;
            RightCheck = 1;
        }
       
        LeftCheckPos = Backgrounds[LeftCheck].position;
        RightCheckPos = Backgrounds[RightCheck].position;
    }

    private void RightReposition(float dir)
    {
        Vector3 Nextpos = new Vector3(scrollAmount * 2f * dir, 0, 0);
        Backgrounds[RightCheck].position = Nextpos + Backgrounds[RightCheck].position;
        if (RightCheck == 0)
        {
            RightCheck++;
            LeftCheck--;
        }
        else
        {
            RightCheck = 0;
            LeftCheck = 1;
        }

        RightCheckPos = Backgrounds[RightCheck].position;
        LeftCheckPos = Backgrounds[LeftCheck].position;
    }

}
