using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.TryGetComponent(out Interactor player))
        {
            player.GetTarget();
            gameObject.SetActive(false);
        }
    }
}
