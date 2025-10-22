using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaChange : MonoBehaviour
{
    public int step = -1;//记录移动步数
    public AreaTransition areaTransition;
    
    public SpriteRenderer[] areas; // 前景淡入淡出
    public Vector3[] movePositions; // 相机移动到的位置
    
    [Header("相机与淡入淡出时长")]
    public float transitionDuration = 1f;
    public float transitionSpeed = 0.5f;
    
    [Header("")]
    public Camera mainCamera;
    
    public  int currentYAreaIndex = 0; // 当前区域索引
    private bool isTransitioning = false;
    
    [Header("远景（按索引切换，不随相机移动）")]
    public GameObject[] farBackgrounds; 

    [Header("远景上下晃动")]
    public float bobAmplitude = 0.2f;            // 振幅（单位：世界坐标）
    public float bobFrequency = 2f;              // 频率（Hz）
    
    [Header("事件广播")]
    public VoidEventSO moveUpEvent;
    public VoidEventSO moveDownEvent;
    public VoidEventSO ResetEvent;
    
    // 记录已访问区域索引
    public int[,] visitedAreas;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        areaTransition = GetComponent<AreaTransition>();
        
        areas = new SpriteRenderer[4];
        for(int i=1;i<=4;i++)
        {
            areas[i-1] = GameObject.Find("areas" + i).GetComponent<SpriteRenderer>();
        }

        farBackgrounds = new GameObject[4];
        for(int i=1;i<=4;i++)
        {
            farBackgrounds[i - 1] = GameObject.Find("farBackground" + i);
        }
        
        // 初始化访问集合并标记当前区域为已访问
        visitedAreas = new int[5, 4];
        visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex ] = 1;
        
        // 初始化远景激活状态：只有 currentAreaIndex 对应的远景激活
        if (farBackgrounds != null && farBackgrounds.Length > 0)
        {
            for (int i = 0; i < farBackgrounds.Length; i++)
            {
                farBackgrounds[i].SetActive(false);
            }
            farBackgrounds[currentYAreaIndex].SetActive(true);
        }
        
    }

    private void Update()
    {
        if (isTransitioning) return;

        if(step==4 && currentYAreaIndex==3 && areaTransition.currentXAreaIndex==1)
        {
            step = 77;
        }
        
        currentPosition.Instance.Y_currentindex = currentYAreaIndex;
    }
    
    //前进
    public void ChangeArea()
    {
        
        if (currentYAreaIndex < areas.Length - 1)
        {
            // 统计步数：首次访问 +1，已访问 -1
            if (visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex+1]!=0)
            {
                step--;
                visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex]=0;
            }
            else
            {
                step++;
                visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex+1]=1;
            }
            
            //状态重置
            if (step == 4 && currentYAreaIndex+1!=3 &&  areaTransition.currentXAreaIndex!=1)
            {
                ResetVisitedY();
            }
            //正常前进
            else
            {
                moveUpEvent.RaiseEvent();
                SceneChange(areas[currentYAreaIndex],areas[currentYAreaIndex+1]);
                int targetIndex = currentYAreaIndex + 1;
                StartCoroutine(TransitionToArea(movePositions[targetIndex],targetIndex));
                //currentAreaIndex++;
            }
            
        }
    }
    //后退
    public void ChangeAreaBack()
    {
        if (currentYAreaIndex > 0)
        {
            // 统计步数：首次访问 +1，已访问 -1
            if (visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex-1]!=0)
            {
                step--;
                visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex]=0;
            }
            else
            {
                step++;
                visitedAreas[areaTransition.currentXAreaIndex,currentYAreaIndex-1]=1;
            }
            
            if (step == 4 && currentYAreaIndex-1!=3 &&  areaTransition.currentXAreaIndex!=1)
            {
                ResetVisitedY();
            }
            else
            {
                moveDownEvent.RaiseEvent();
                SceneChange(areas[currentYAreaIndex],areas[currentYAreaIndex-1]);
                int targetIndex = currentYAreaIndex - 1;
                StartCoroutine(TransitionToArea(movePositions[targetIndex],targetIndex));
                //currentAreaIndex--;
            }
        }
    }
    
    // 公共方法：重置步数、摄像机位置，近景、远景、已访问记录
    public void ResetVisitedY()
    {
        ResetEvent.RaiseEvent();
        
        for (int i = 0; i < visitedAreas.GetLength(0); i++)
        {
            for (int j = 0; j < visitedAreas.GetLength(1); j++)
            {
                visitedAreas[i, j] = 0;
            }
        }
        
        Debug.Log("reset!");

        
        
        SceneChange(areas[currentYAreaIndex],areas[1]);
        
        StartCoroutine(TransitionToArea(movePositions[1],1));
        //currentYAreaIndex = 1;
        areaTransition.currentXAreaIndex = 3;
        
        step = 0;
        if (farBackgrounds != null && farBackgrounds.Length > 0)
        {
            for (int i = 0; i < farBackgrounds.Length; i++)
            {
                if (farBackgrounds[i] == null) continue;
                farBackgrounds[i].SetActive(i == currentYAreaIndex);
            }
        }
    }
    
    //场景切换淡入淡出
    private void SceneChange(SpriteRenderer currentScene,SpriteRenderer nextScene)
    {
        if (currentScene == null || nextScene == null)
        {
            Debug.LogWarning("[AreaChange] areas 中存在空引用，请在 Inspector 重新绑定。");
            return;
        }
        
        currentScene.DOFade(0f, transitionDuration);
        nextScene.DOFade(1f, transitionDuration);
        
        //TODO:到达一定地点切换远景
    }
    
    
    
    //相机平滑移动,同时在过渡中切换远景（在进度 0.5 切换）
    private IEnumerator TransitionToArea(Vector3 targetPosition,int targetAreaIndex)
    {
        if (mainCamera == null) yield break;
        if (movePositions == null || movePositions.Length == 0) yield break;
        
        isTransitioning = true;

        Vector3 cameraStartPosition = mainCamera.transform.position;
        Vector3 realTargetPosition = new Vector3(mainCamera.transform.position.x,mainCamera.transform.position.y,targetPosition.z);

        if (step == 4)
        {
            Debug.Log("相机返回初始位置");
            realTargetPosition = new Vector3(30,0,4.5f);
        }
        
        float journey = 0f;
        float elapsed = 0f;
        bool swappedFar = false;
        

        while (journey < 1f)
        {
            float dt = Time.deltaTime;
            elapsed += dt;
            journey += dt * Mathf.Max(0.0001f, transitionSpeed);
            journey = Mathf.Min(journey, 1f);
            
            
            
            // 基础线性插值位置
            Vector3 basePos = Vector3.Lerp(cameraStartPosition, realTargetPosition, journey);

            // 计算晃动偏移（只在 Y 方向）
            float bobY = Mathf.Sin(elapsed * bobFrequency * Mathf.PI * 2f) * bobAmplitude;

            // 应用晃动到相机位置
            mainCamera.transform.position = new Vector3(basePos.x, basePos.y + bobY, basePos.z);
            
            // // 相机移动
            // mainCamera.transform.position = Vector3.Lerp(
            //     cameraStartPosition,
            //     new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, targetPosition.z),
            //     journey
            // );
            
            // 切换远景
            if (!swappedFar && farBackgrounds != null && farBackgrounds.Length > 0)
            {
                if (journey >= 0.7f)
                {
                    
                    if (currentYAreaIndex >= 0 && currentYAreaIndex < farBackgrounds.Length && farBackgrounds[currentYAreaIndex])
                        farBackgrounds[currentYAreaIndex].SetActive(false);

                    if (targetAreaIndex >= 0 && targetAreaIndex < farBackgrounds.Length && farBackgrounds[targetAreaIndex])
                        farBackgrounds[targetAreaIndex].SetActive(true);
                    
                    
                    currentYAreaIndex = targetAreaIndex;
                    
                    swappedFar = true;
                }
            }

            yield return null;
        }

        // 到达最终位置并清除晃动（确保高度精确）
        mainCamera.transform.position = realTargetPosition;

        // 确保远景最终状态正确
        if (farBackgrounds != null && farBackgrounds.Length > 0)
        {
            for (int i = 0; i < farBackgrounds.Length; i++)
            {
                if (farBackgrounds[i]) continue;
                farBackgrounds[i].SetActive(i == targetAreaIndex);
            }
        }
        
        isTransitioning = false;
        
    }






}
