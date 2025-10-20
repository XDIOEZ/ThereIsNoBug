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
    
    public void MoveToAreaRight()
    {
        if (currentXAreaIndex < areas.Length - 1)
        {
            countStep( currentXAreaIndex + 1,areaChange.currentYAreaIndex);
            //状态重置
            if(areaChange.step == 4 && areaChange.currentYAreaIndex!=3 && currentXAreaIndex != 1)
            {
                
                areaChange.ResetVisitedY();
            }
            else
            {
                currentXAreaIndex++;
                StartCoroutine(TransitionToArea(areas[currentXAreaIndex].position, currentXAreaIndex));
            }
        }
    }
    
    public void MoveToAreaLeft()
    {
        if (currentXAreaIndex > 0)
        {
            countStep(currentXAreaIndex - 1 ,areaChange.currentYAreaIndex);
            //状态重置
            if(areaChange.step == 4 && areaChange.currentYAreaIndex!=3 && currentXAreaIndex != 1)
            {
                areaChange.ResetVisitedY();
            }
            else
            {
                currentXAreaIndex--;
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
