using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DialogueList
{
    public string name;
    public int Setindex;
    public int indexNum = 0;
    public bool isEnd = false;
    string FilePath;
    string DataPath;
    public List<DialogueData> _Talks = new List<DialogueData>();
    public void AddDialogueList()
    {
        DataPath = "DialogueData/" + name + "/" + Setindex.ToString() + "/";
        CheckFolder();
    }
    public void CheckFolder()
    {
        DialogueData DataForAdd = JsonUtility.FromJson<DialogueData>(Resources.Load<TextAsset>(DataPath + indexNum).text);
        if(DataForAdd == null)
        {
            Debug.Log("파일이 존재하지 않습니다");
            return;
        }
        _Talks.Add(DataForAdd);
        indexNum++;
        CheckFolder();
    }

    public void DialogueEnd()
    {
        isEnd = true;
    }
}


//FilePath = "D:\\GitHub\\RottenApple-Summer-Project-2D\\Assets\\Resources\\DialogueData\\" + name + "\\" + Setindex.ToString() + "\\" + indexNum + ".txt";

//if (File.Exists(FilePath)){
//    DialogueData DataForAdd = JsonUtility.FromJson<DialogueData>(Resources.Load<TextAsset>(DataPath + indexNum).text);
//    _Talks.Add(DataForAdd);
//    indexNum++;
//    CheckFolder();
//}
//else
//{
//    Debug.Log("업는대???");
//    return;
//}