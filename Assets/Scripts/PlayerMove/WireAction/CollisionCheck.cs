using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public System.Action OnCollision;
    public Vector2 ColPos { get; private set; }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GrapplingObj"))
        {
            ColPos = collision.contacts[0].point;
            if (OnCollision != null) OnCollision();
        }
    }
}
