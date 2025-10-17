using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public string sceneToSave; 
    
    //工厂模式
    public void SaveGameScene(GameSceneSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSavedScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
    
    public Dictionary<string,Vector3> characterPositionDict = new Dictionary<string, Vector3>();
    
    //存储人物相关属性
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();
    
    
}
