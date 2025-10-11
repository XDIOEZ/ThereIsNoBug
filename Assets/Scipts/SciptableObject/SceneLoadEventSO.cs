using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO,Vector3,bool> LoadRequestEvent;
    
    
    
    
    
    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="locationToLoad">要加载的场景</param>
    /// <param name="spawnPosition">加载后角色传送地点</param>
    /// <param name="isFade">是否屏幕渐隐</param>
    public void RaiseEvent(GameSceneSO locationToLoad, Vector3 spawnPosition, bool isFade)
    {
        if (LoadRequestEvent != null)
        {
            LoadRequestEvent.Invoke(locationToLoad, spawnPosition, isFade);
        }
        else
        {
            Debug.LogWarning("No event listener for " + name);
        }
    }
    
}
