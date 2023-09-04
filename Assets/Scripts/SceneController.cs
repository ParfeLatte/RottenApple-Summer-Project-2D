using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void ChangeToNextStage(string NextStageName) {
        SceneManager.LoadScene(NextStageName);
    }
}
