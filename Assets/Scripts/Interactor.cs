using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    enum State
    {
        Disable,
        Ready,
        interacting
    }

    private List<NpcInteract> _Npcs = new List<NpcInteract>();
    private bool isInteract = false;
    private bool continueInteract = false;
    private State state;

    void Start()
    {
        state = State.Disable;
    }

    void Update()
    {
        if (state == State.Disable) return;
        if (PlayerInput.instance.Interact)
        {
            //��ȣ�ۿ� ����
            Interacting();
        }
    }

    private void Interacting()
    {
        if (_Npcs.Count == 0) return;
        if (state == State.Disable) return;

        if (_Npcs[0].CheckDialogue())
        {
            state = State.interacting;
        }
        else
        {
            //â �ݾ��ֱ�
            state = State.Ready;
        }
    }

    //private void ContinueInteract()
    //{
    //    if (_Npcs.Count == 0) return;
    //    if (state != State.interacting) return;

    //    if (_Npcs[0].ShowDialogue())
    //    {
    //        state = State.interacting;
    //    }
    //    else
    //    {
    //        state = State.Ready;
    //        //â �ݾ��ֱ�
    //    }
    //}

    public void SetInteractNpc(NpcInteract interactNpc)
    {
       _Npcs.Add(interactNpc);
        state = State.Ready;
    }

    public void RemoveInteractNpc(NpcInteract RemoveNpc)
    {
        _Npcs.Remove(RemoveNpc);
        state = State.Disable;
    }
}
