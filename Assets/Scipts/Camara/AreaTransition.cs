using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTransition : MonoBehaviour
{
    public Transform[] areas; // 不同的2D场景区域
    public float transitionSpeed = 2f;
    public Camera mainCamera;
    
    private int currentAreaIndex = 1;
    private bool isTransitioning = false;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    
    void Update()
    {
        // 键盘输入切换区域
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isTransitioning)
        {
            MoveToNextArea();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !isTransitioning)
        {
            MoveToPreviousArea();
        }
    }
    
    public void MoveToNextArea()
    {
        if (currentAreaIndex < areas.Length - 1)
        {
            currentAreaIndex++;
            StartCoroutine(TransitionToArea(areas[currentAreaIndex].position));
        }
    }
    
    public void MoveToPreviousArea()
    {
        if (currentAreaIndex > 0)
        {
            currentAreaIndex--;
            StartCoroutine(TransitionToArea(areas[currentAreaIndex].position));
        }
    }
    
    private IEnumerator TransitionToArea(Vector3 targetPosition)
    {
        isTransitioning = true;
        
        Vector3 startPosition = mainCamera.transform.position;
        float journey = 0f;
        
        while (journey <= 1f)
        {
            journey += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPosition, 
                new Vector3(targetPosition.x, targetPosition.y, startPosition.z), journey);
            yield return null;
        }
        
        isTransitioning = false;
    }
    
    // 直接跳转到指定区域
    public void JumpToArea(int areaIndex)
    {
        if (areaIndex >= 0 && areaIndex < areas.Length)
        {
            currentAreaIndex = areaIndex;
            Vector3 targetPosition = areas[areaIndex].position;
            mainCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, 
                mainCamera.transform.position.z);
        }
    }
}
