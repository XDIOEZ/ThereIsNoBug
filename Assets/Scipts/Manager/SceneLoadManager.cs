using System;
using System.Collections;
using System.Collections.Generic;
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
    
    
    [Header("事件监听")]
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO dayJumpEvent;
    
    [Header("事件广播")]
    public SceneLoadEventSO unloadedSceneEvent;
    public VoidEventSO afterScneLoadEvent;
    public FadeEventSO fadeEvent;
    
    [Header("场景设置")]
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
        dayJumpEvent.OnEventRaised += DayJump;
    }
    private void OnDisable()
    {
        loadEventSO.LoadRequestEvent -= LoadScene;
        dayJumpEvent.OnEventRaised -= DayJump;
    }

    //新游戏（主菜单加载初始场景）
    private void DayJump()
    {
        _sceneToLoad = menuLoadScene;
        loadEventSO.RaiseEvent(_sceneToLoad,firstPosition,true);
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

        // 仅在非主菜单场景显示玩家，并在黑屏时设置坐标
        if (player)
        {
            if (_sceneToLoad != menuScene)
            {
                player.position = _positionToGo;
                player.gameObject.SetActive(true);
            }
            else
            {
                player.gameObject.SetActive(false);
            }
        }
        
        if (_isFade)
        {
            //渐入
            fadeEvent.FadeOut(fadeDuration);
        }
        
        _isLoading = false;

        //场景加载完成后广播事件,执行渐入后的逻辑
        afterScneLoadEvent?.RaiseEvent();
    }
}
