using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Dialogue")]
    [SerializeField] private GameObject DialogueBox;
    [SerializeField] private Text NameTxt;
    [SerializeField] private Text DialogueTxt;
    [SerializeField] private Image CharStandImg;

    [Header("GameClear")]
    [SerializeField] private GameObject ClearBox;
    public Button NextStage; 

    protected override void Awake()
    {
        base.Awake();
    }

    #region SetDialogue

    public void SetDialogue(string name, string dialogue, string SpriteName)
    {
        NameTxt.text = name;
        DialogueTxt.text = dialogue;
        CharStandImg.sprite = Resources.Load<Sprite>("CharacterSprite/" + SpriteName);
    } 

    public void DialogueOnOff()
    {
        if (DialogueBox.activeSelf)
        {
            DialogueBox.SetActive(false);
        }
        else
        {
            DialogueBox.SetActive(true);
        }
    }
    #endregion

    #region StageClear

    public void ClearOnoff()
    {
        if (ClearBox.activeSelf)
        {
            ClearBox.SetActive(false);
        }
        else
        {
            ClearBox.SetActive(true);
        }
    }
    #endregion
}
