using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundParentSet : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement p_move;

    private void Awake()
    {
        p_move = player.GetComponent<PlayerMovement>();
        p_move.JumpEvent += SetParentNull;
        p_move.LandingEvent += SetParent;
    }

    public void SetParent()
    {
        transform.parent = Camera;
    }

    public void SetParentNull()
    {
        transform.parent = null;
    }
}
