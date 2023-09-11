using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundEffect : MonoBehaviour
{
    private bool SunRiseEnd;
    [SerializeField] private GameObject Sun;
    //[SerializeField] private SpriteRenderer Fog;

    [SerializeField] private Vector3 FirstSunPos;
    [SerializeField] private float FinalSunYPos;

    void Awake()
    {
        SunRiseEnd = false;
        Sun.transform.position = FirstSunPos;
    }

    void Update()
    {
        if (!SunRiseEnd)
        {
            SunRise();
        }
    }

    private void SunRise()
    {
        Sun.transform.position += Vector3.up * Time.deltaTime;

        if (Sun.transform.position.y >= FinalSunYPos)
        {
            Sun.transform.position= new Vector3(Sun.transform.position.x, FinalSunYPos, Sun.transform.position.z);
            SunRiseEnd = true;
        }
    }
}
