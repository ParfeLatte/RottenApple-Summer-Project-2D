using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Stage")]
public class StageInfo : ScriptableObject
{
    public string StageName;//현재 스테이지 씬 이름
    public string NextStage;//다음 스테이지 씬 이름

    public bool HaveTarget;//클리어 목표가 있는가

    public bool GetStageInfo()
    {
        return HaveTarget;
    }
}
