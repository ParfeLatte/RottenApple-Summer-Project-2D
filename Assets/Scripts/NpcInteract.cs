using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcInteract : MonoBehaviour
{
    private Interactor _interactor;
    [SerializeField] private string Name;
    [SerializeField] private int DialogueSetCount;
    private int DialSet = 1;
    public int CurDialogue;
    public int CurDialSet;
    List<DialogueList> _dialogueList = new List<DialogueList>();

    public string Nametxt;
    public string DialTxt;
    public string CharImg;
    public GameObject DialougeBox;


    private void Awake()
    {
       for(int i = 0; i< DialogueSetCount; i++)
        {
            DialogueList list = new DialogueList();
            _dialogueList.Add(list);
            _dialogueList[i].name = Name;
            _dialogueList[i].Setindex = DialSet;
            _dialogueList[i].AddDialogueList();
            DialSet++;
        }
        CurDialogue = 0;
        CurDialSet = 0;
    }

    public bool CheckDialogue()
    {
        if (_interactor == null) return false;
        if (CurDialogue == _dialogueList[CurDialSet]._Talks.Count)
        {
            EndDialogue();
            return false;
        }
        else if (CurDialogue == 0) {
            StartDialogue();
            return true;
        }
        else
        {
            ShowDialogue();
            return true;
        }
    }
    
    private void StartDialogue()
    {
        UIManager.instance.DialogueOnOff();
        Debug.Log("Start Dialogue");
        ShowDialogue();
    }

    private void EndDialogue()
    {
        UIManager.instance.DialogueOnOff();
        CurDialogue = 0;
        Debug.Log("End Dialogue");
    }

    private void ShowDialogue()
    {
        Nametxt = _dialogueList[CurDialSet]._Talks[CurDialogue].Name;
        DialTxt = _dialogueList[CurDialSet]._Talks[CurDialogue].Dialouge;
        CharImg = _dialogueList[CurDialSet]._Talks[CurDialogue].StandingName;
        UIManager.instance.SetDialogue(Nametxt, DialTxt, CharImg);
        CurDialogue++;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Interactor interactor))
        {
            _interactor = interactor;
            _interactor.SetInteractNpc(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Interactor interactor)){
            if (_interactor == null) return;
            interactor.RemoveInteractNpc(this);
            _interactor = null;
        }
    }
}
