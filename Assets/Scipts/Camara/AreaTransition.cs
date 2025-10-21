using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaTransition : MonoBehaviour
{
    public Transform[] areas; // 不同的2D场景区域
    public float transitionSpeed = 2f;
    public Camera mainCamera;
    
    public int currentXAreaIndex = 3;
    private bool _isTransitioning = false;
    
    public AreaChange areaChange;

    [Header("事件广播")]
    public VoidEventSO moveLeftEvent;
    public VoidEventSO moveRightEvent;
    
    
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        areaChange = GetComponent<AreaChange>();
        

    }
    
    void Update()
    {
        // 键盘输入切换区域
        if (Input.GetKeyDown(KeyCode.RightArrow) && !_isTransitioning)
        {
            MoveToAreaRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !_isTransitioning)
        {
            MoveToAreaLeft();
        }
    }
    
    public void countStep(int targetXAreaIndex, int targetYAreaIndex)
    {
        Debug.Log("LRcountStep");
        // 统计步数：首次访问 +1，已访问 -1
        if (areaChange.visitedAreas[targetXAreaIndex,targetYAreaIndex]!=0)
        {
            areaChange.step--;
            areaChange.visitedAreas[currentXAreaIndex,areaChange.currentYAreaIndex]=0;
        }
        else
        {
            areaChange.step++;
            areaChange.visitedAreas[targetXAreaIndex,targetYAreaIndex]=1;
        }
    }
    private bool ShouldResetAtStep4(int nextX, int nextY)
    {
        // 指定位置为 (x=1, y=3)，达到阈值且目标位置不是指定位置时重置
        return areaChange.step >= 4 && !(nextX == 1 && nextY == 3);
    }
    
    public void MoveToAreaRight()
    {
        if (currentXAreaIndex < areas.Length - 1)
        {
            int nextX = currentXAreaIndex + 1;
            int nextY = areaChange.currentYAreaIndex;
            
            countStep( nextX, nextY);
            //状态重置
            if (ShouldResetAtStep4(nextX, nextY))
            {
                areaChange.ResetVisitedY();
                // 回到指定位置（X=3；Y=0）
                currentXAreaIndex = 3;
                StartCoroutine(TransitionToArea(areas[currentXAreaIndex].position, currentXAreaIndex));
            }
            else
            {
                moveRightEvent.RaiseEvent();
                currentXAreaIndex = nextX;
                StartCoroutine(TransitionToArea(areas[currentXAreaIndex].position, currentXAreaIndex));
            }
        }
    }
    
    public void MoveToAreaLeft()
    {
        if (currentXAreaIndex > 0)
        {
            int nextX = currentXAreaIndex - 1;
            int nextY = areaChange.currentYAreaIndex;
            
            countStep(nextX, nextY);
            
            //状态重置
            if (ShouldResetAtStep4(nextX, nextY))
            {
                areaChange.ResetVisitedY();
                currentXAreaIndex = 3;
                StartCoroutine(TransitionToArea(areas[currentXAreaIndex].position, currentXAreaIndex));
            }
            else
            {
                moveLeftEvent.RaiseEvent();
                currentXAreaIndex = nextX;
                StartCoroutine(TransitionToArea(areas[currentXAreaIndex].position, currentXAreaIndex));
            }
        }
    }
    
    private IEnumerator TransitionToArea(Vector3 targetPosition, int targetAreaIndex)
    {
        _isTransitioning = true;
        
        Vector3 startPosition = mainCamera.transform.position;
        float journey = 0f;
        
        while (journey <= 1f)
        {
            journey += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPosition, 
                new Vector3(targetPosition.x, targetPosition.y, startPosition.z), journey);
            yield return null;
        }
        
        _isTransitioning = false;
    }
    
}
