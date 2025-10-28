using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CG2 : MonoBehaviour
{
    public GameSceneSO nextScene;
    
    public void LoadNextScene()
    {
        SceneLoadManager.GetInstance().LoadScene(nextScene,new Vector3(0,0,0),true);
    }
    
}
