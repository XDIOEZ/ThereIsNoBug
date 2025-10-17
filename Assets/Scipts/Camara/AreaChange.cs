using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AreaChange : MonoBehaviour
{
    public SpriteRenderer[] areas; // 前景淡入淡出
    public Vector3[] movePositions; // 相机移动到的位置
    
    [Header("相机与淡入淡出时长")]
    public float transitionDuration = 1f;
    public float transitionSpeed = 0.5f;
    
    [Header("")]
    public Camera mainCamera;
    
    public int currentAreaIndex = 0; // 当前区域索引
    private bool isTransitioning = false;
    
    [Header("远景（与相机 z 同步移动，保持远景不变）")]
    public GameObject farBackground; 
    public float cameraMoveDistance; // 相机每次移动的距离

    [Header("远景上下晃动")]
    public float bobAmplitude = 0.2f;            // 振幅（单位：世界坐标）
    public float bobFrequency = 2f;              // 频率（Hz）
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        
        areas = new SpriteRenderer[4];
        for(int i=1;i<=4;i++)
        {
            areas[i-1] = GameObject.Find("areas" + i).GetComponent<SpriteRenderer>();
        }
        
    }

    private void Update()
    {
        if (isTransitioning) return;
        
        if(Input.GetKeyDown(KeyCode.C)&&!isTransitioning)
        {
            ChangeArea();
        }
        else if(Input.GetKeyDown(KeyCode.X)&&!isTransitioning)
        {
            ChangeAreaBack();
        }
    }
    
    //前进
    private void ChangeArea()
    {
        if (currentAreaIndex < areas.Length - 1)
        {
            SceneChange(areas[currentAreaIndex],areas[currentAreaIndex+1]);
            StartCoroutine(TransitionToArea(movePositions[currentAreaIndex+1]));
            currentAreaIndex++;
        }
    }
    //后退
    private void ChangeAreaBack()
    {
        if (currentAreaIndex > 0)
        {
            SceneChange(areas[currentAreaIndex],areas[currentAreaIndex-1]);
            StartCoroutine(TransitionToArea(movePositions[currentAreaIndex-1]));
            currentAreaIndex--;
        }
        else
        {
            return;
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
    }
    
    
    
    //相机和远景同时平滑移动
    private IEnumerator TransitionToArea(Vector3 targetPosition)
    {
        // isTransitioning = true;
        //
        // Vector3 cameraStartPosition = mainCamera.transform.position;
        // Vector3 farBackgroundStartPosition = farBackground.transform.position;
        //
        // float journey = 0f;
        //
        // cameraMoveDistance = targetPosition.z - cameraStartPosition.z;
        //
        // while (journey <= 1f)
        // {
        //     journey += Time.deltaTime * transitionSpeed;
        //     mainCamera.transform.position = Vector3.Lerp(cameraStartPosition, 
        //         new Vector3(targetPosition.x, targetPosition.y, targetPosition.z), journey);
        //     
        //     farBackground.transform.position = Vector3.Lerp(farBackgroundStartPosition, 
        //         new Vector3(farBackgroundStartPosition.x, farBackgroundStartPosition.y, farBackgroundStartPosition.z+cameraMoveDistance), journey);
        //     
        //     
        //     yield return null;
        // }
        //
        // isTransitioning = false;
        if (mainCamera == null) yield break;

        isTransitioning = true;

        Vector3 cameraStartPosition = mainCamera.transform.position;
        Vector3 farStartPosition = farBackground != null ? farBackground.transform.position : Vector3.zero;

        float journey = 0f;
        float bobTime = 0f;
        cameraMoveDistance = targetPosition.z - cameraStartPosition.z;

        while (journey < 1f)
        {
            float dt = Time.deltaTime;
            journey += dt * Mathf.Max(0.0001f, transitionSpeed);
            journey = Mathf.Min(journey, 1f);

            // 相机移动
            mainCamera.transform.position = Vector3.Lerp(
                cameraStartPosition,
                new Vector3(targetPosition.x, targetPosition.y, targetPosition.z),
                journey
            );

            // 远景移动（z 跟随），Y 叠加上下晃动：sin(2πft) * 振幅 * 包络(使首尾为 0)
            if (farBackground != null)
            {
                bobTime += dt;
                float envelope = Mathf.Sin(journey * Mathf.PI); // 0→1→0 平滑包络
                float bob = Mathf.Sin(bobTime * 2f * Mathf.PI * Mathf.Max(0f, bobFrequency)) *
                            Mathf.Max(0f, bobAmplitude) * envelope;

                float farZ = Mathf.Lerp(farStartPosition.z, farStartPosition.z + cameraMoveDistance, journey);
                farBackground.transform.position = new Vector3(
                    farStartPosition.x,
                    farStartPosition.y + bob,
                    farZ
                );
            }

            yield return null;
        }
        // 归位最终位置（消除数值误差，Y 回到初始，Z 到目标）
        if (farBackground != null)
        {
            farBackground.transform.position = new Vector3(
                farStartPosition.x,
                farStartPosition.y,
                farStartPosition.z + cameraMoveDistance
            );
        }
        mainCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

        isTransitioning = false;
        
    }






}
