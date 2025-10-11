using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class SceneLoadManager : MonoBehaviour
{
    
    public Vector3 menuPosition;
    public Vector3 firstPosition;
    
    private bool _isFade;
    private bool _isLoading;
    
    
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO newGameEvent;
    
    [Header("事件广播")]
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO afterScneLoadEvent;
    
    [Header("场景设置")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;
    private GameSceneSO _currentScene;
    private GameSceneSO _sceneToLoad;
    
    private Vector3 _positionToGo;
    public float fadeDuration;
    
    private void Start()
    {
        loadEventSO.RaiseEvent(menuScene,menuPosition,true);//加载主菜单
    }

    private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += LoadScene;
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
        newGameEvent.OnEventRaised -= NewGame;
    }

    //新游戏（主菜单加载初始场景）
    private void NewGame()
    {
        _sceneToLoad = firstLoadScene;
        loadEventSO.RaiseEvent(_sceneToLoad,firstPosition,true);
    }
    
    //从一个场景加载另一个场景
    private void LoadScene(GameSceneSO sceneToLoad, Vector3 positionToGo, bool isFade)
    {
        //防止重复加载
        if(_isLoading)
            return;
        
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
        }
        
        yield return new WaitForSeconds(fadeDuration);

        //当前场景卸载后广播事件
        unloadedSceneEvent.RaiseEvent(_sceneToLoad,_positionToGo,true);
        
        yield return _currentScene.sceneReference.UnLoadScene();
        
        
        //隐藏玩家，防止瞬移
        //player.gameObject.SetActive(false);
        
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

        //player.position = positionToGo;

        //坐标调整完毕，显示玩家
        //player.gameObject.SetActive(true);
        
        if (_isFade)
        {
            //TODO:渐入
            //fadeEvent.FadeOut(fadeDuration);
        }
        
        _isLoading = false;

        afterScneLoadEvent?.RaiseEvent();
    }
}
