using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundEffect : MonoBehaviour
{
    private bool SunRiseEnd;
    private bool ClearFogEnd;
    [SerializeField] private GameObject Sun;
    [SerializeField] private List<SpriteRenderer> Fogs = new List<SpriteRenderer>();
    [SerializeField] private List<float> reduceValue = new List<float>();
    [SerializeField] private float ReduceTime;//17초로 잡고 계산

    [SerializeField] private Vector3 FirstSunPos;
    [SerializeField] private float FinalSunYPos;

    void Awake()
    {
        SunRiseEnd = false;
        Sun.transform.position = FirstSunPos;
    }

    private void Start()
    {
        for(int i = 0; i < Fogs.Count; i++)
        {
            float Value = Fogs[i].color.a;
            reduceValue.Add(Value);
        }
    }

    void Update()
    {
        
        if (!SunRiseEnd)
        {
            SunRise();
            ClearFog();
        }
    }

    private void SunRise()
    {
        Sun.transform.position += Vector3.up * 1.5f * Time.deltaTime;
        

        if (Sun.transform.position.y >= FinalSunYPos)
        {
            Sun.transform.position= new Vector3(Sun.transform.position.x, FinalSunYPos, Sun.transform.position.z);
            PerfectlyClearFog();
            SunRiseEnd = true;
        }
    }

    private void ClearFog()
    {
        if (SunRiseEnd) return;
        if (ClearFogEnd) return;
        for (int i = 0; i < Fogs.Count; i++)
        {
            float Value = Fogs[i].color.a - (reduceValue[i] / ReduceTime * Time.deltaTime);
            Fogs[i].color = new Color(1f, 1f, 1f, Value);
        }
    }

    private void PerfectlyClearFog()
    {
        for (int i = 0; i < Fogs.Count; i++)
        {
            Fogs[i].color = new Color(1f, 1f, 1f, 0);
        }
        ClearFogEnd = true;
    }
}
