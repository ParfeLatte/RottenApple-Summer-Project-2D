using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] private StageInfo CurrentStage;
    [SerializeField] private GameObject GoalObj;
    private bool isTarget;

   
    protected override void Awake()
    {
        base.Awake();

        if (CurrentStage.GetStageInfo())
        {
            isTarget = false;
        }
        else
        {
            isTarget = true;
        }
    }
    public void StageClear()
    {
        if (!isTarget) return;
        UIManager.instance.ClearOnoff();
        UIManager.instance.NextStage.onClick.AddListener(() => {
            SceneController.instance.ChangeToNextStage(CurrentStage.NextStage);
            UIManager.instance.ClearOnoff();
        });
    }

    public void GetTarget()
    {
        isTarget = true;
    }
}
