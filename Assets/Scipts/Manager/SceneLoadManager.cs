using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneLoadManager : MonoBehaviour
{
    public Transform player;
    
    public Vector3 menuPosition;
    public Vector3 firstPosition;
    
    private bool _isFade = true;
    private bool _isLoading;
    
    const string PLAYER_DATA_FILE_NAME = "PlayerData.json";
    const string SCENE_DATA_FILE_NAME = "SceneData.json";
    
    
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    [FormerlySerializedAs("dayJumpEvent")] public VoidEventSO newGameEvent;
    public VoidEventSO loadLastSceneEvent;
    
    [Header("事件广播")]
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO afterScneLoadEvent;
    public FadeEventSO fadeEvent;
    
    [Header("场景设置")]
    public GameSceneRegistrySO sceneRegistry;
    public GameSceneSO menuLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO _currentScene;
    private GameSceneSO _sceneToLoad;
    
    private Vector3 _positionToGo;
    public float fadeDuration;
    
    private void Start()
    {
        if (player) player.gameObject.SetActive(false);
        loadEventSO.RaiseEvent(menuScene,menuPosition,true);//加载主菜单
    }
    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += LoadScene;
        newGameEvent.OnEventRaised += NewGame;
        loadLastSceneEvent.OnEventRaised += LoadLastScene;
        
        // ISaveable saveable = this;
        // saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
        newGameEvent.OnEventRaised -= NewGame;
        loadLastSceneEvent.OnEventRaised -= LoadLastScene;
        
        // ISaveable saveable = this;
        // saveable.UnregisterSaveData();
    }

    //新游戏（主菜单加载初始场景）
    private void NewGame()
    {
        _sceneToLoad = menuLoadScene;
        loadEventSO.RaiseEvent(_sceneToLoad,firstPosition,true);
    }
    //菜单中继续游戏接口
    public void LoadLastScene()
    {
        var sceneId = SaveSystem.LoadByJson<string>(SCENE_DATA_FILE_NAME);
        if (string.IsNullOrEmpty(sceneId))
        {
            Debug.Log("无存档");
            return;
        }

        var lastPosition = SaveSystem.LoadByJson<Vector3>(PLAYER_DATA_FILE_NAME);

        // 不覆盖 _currentScene，避免卸载错场景
        var targetScene = sceneRegistry != null ? sceneRegistry.GetByAddressableGuid(sceneId) : null;
        if (targetScene == null)
        {
            Debug.LogError($"存档中的场景ID无效或未在注册表中：{sceneId}");
            return;
        }

        loadEventSO.RaiseEvent(targetScene, lastPosition, true);
    }
    
    
    //从一个场景加载另一个场景
    private void LoadScene(GameSceneSO sceneToLoad, Vector3 positionToGo, bool isFade)
    {
        //防止重复加载
        if(_isLoading)
            return;
        Debug.Log("new scene load");
        _isLoading = true;
        this._sceneToLoad = sceneToLoad;
        this._positionToGo = positionToGo;
        this._isFade = isFade;
        
        //如果当前有场景则卸载
        if(_currentScene)
            StartCoroutine(UnLoadPreviousScene());
        else
            LoadNewScne();
    }
    //卸载场景并加载新场景
    IEnumerator UnLoadPreviousScene()
    {
        
        if (_isFade)
        {
            //TODO:渐出
            fadeEvent.FadeIn(fadeDuration);
        }
        
        yield return new WaitForSeconds(fadeDuration);
        
        // 黑屏中隐藏玩家，防止瞬移
        if (player) player.gameObject.SetActive(false);

        //当前场景卸载后广播事件,执行黑屏后的逻辑
        unloadedSceneEvent.RaiseEvent(_sceneToLoad,_positionToGo,true);
        
        yield return _currentScene.sceneReference.UnLoadScene();
        
        
        //加载新场景
        LoadNewScne();
    }
    private void LoadNewScne()
    { 
        var loadingOperation =  _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOperation.Completed += OnLoadCompleted; //场景加载完成后执行
    }
    //场景加载完成后执行逻辑
    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        _currentScene = _sceneToLoad;

        // 仅在非主菜单场景显示玩家，并在黑屏时设置坐标
        // if (player)
        // {
        //     if (_sceneToLoad != menuScene)
        //     {
        //         player.position = _positionToGo;
        //         player.gameObject.SetActive(true);
        //     }
        //     else
        //     {
        //         player.gameObject.SetActive(false);
        //     }
        // }
        
        //记录当前场景，用于游戏退出后继续游戏
        SaveLastScene();
        
        if (_isFade)
        {
            //渐入
            fadeEvent.FadeOut(fadeDuration);
        }
        
        _isLoading = false;

        //场景加载完成后广播事件,执行渐入后的逻辑
        //afterScneLoadEvent?.RaiseEvent();
    }
    

    public void SaveLastScene()
    {
        if (_currentScene == null) return;
        if (_currentScene == menuScene) return; // 主菜单不作为继续点

        // 使用 Addressables 场景的 AssetGUID 作为稳定 ID
        var sceneId = _currentScene.sceneReference.AssetGUID;
        if (string.IsNullOrEmpty(sceneId))
        {
            Debug.LogWarning("当前场景缺少有效的 Addressables GUID，跳过存档。");
            return;
        }

        SaveSystem.SaveByJson(SCENE_DATA_FILE_NAME, sceneId);
        if (player) SaveSystem.SaveByJson(PLAYER_DATA_FILE_NAME, player.position);
    }


    //删除存档接口
    public static void DeleteDataSaveFile()
    {
        SaveSystem.DeleteSaveFile(SCENE_DATA_FILE_NAME);
        SaveSystem.DeleteSaveFile(PLAYER_DATA_FILE_NAME);
    }

    ///<summary>
    /// 存档接口
    // public DataDefination GetDataID()
    // {
    //     return GetComponent<DataDefination>();
    // }
    //
    // public void GetSaveData(Data data)
    // {
    //     data.SaveGameScene(_currentScene);
    // }
    //
    // public void LoadData(Data data)
    // {
    //     var playerID = player.GetComponent<DataDefination>().ID;
    //     if (data.characterPositionDict.ContainsKey(playerID))
    //     {
    //         _positionToGo = data.characterPositionDict[playerID];
    //         _sceneToLoad = data.GetSavedScene();
    //         
    //         LoadScene(_sceneToLoad, _positionToGo, true);
    //         
    //     }
    // }
    /// </summary>
}
