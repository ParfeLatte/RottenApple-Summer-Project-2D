using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Stage")]
public class StageInfo : ScriptableObject
{
    public string StageName;//���� �������� �� �̸�
    public string NextStage;//���� �������� �� �̸�

    public bool HaveTarget;//Ŭ���� ��ǥ�� �ִ°�

    public bool GetStageInfo()
    {
        return HaveTarget;
    }
}
