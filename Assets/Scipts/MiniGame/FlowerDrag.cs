using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlowerDrag : MonoBehaviour
{
    [Header("拖拽设置")]
    [Tooltip("是否启用拖拽功能")]
    public bool enableDrag = true;
    
    [Tooltip("拖拽时的Z轴位置，确保在摄像机前方")]
    public float dragZPosition = -5f;
    
    [Tooltip("检测最近花朵的最大距离")]
    public float maxSwapDistance = 3f;
    
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Vector3 clickPosition; // 记录点击时的位置
    private bool isDragging = false;
    private FlowerChange flowerManager;
    private Collider2D flowerCollider;

    void Start()
    {
        // 获取主摄像机
        mainCamera = Camera.main;
        
        // 获取碰撞器组件
        flowerCollider = GetComponent<Collider2D>();
        if (flowerCollider == null)
        {
            Debug.LogError("花朵对象需要Collider2D组件才能进行拖拽操作！", gameObject);
        }
        
        // 查找FlowerChange管理器
        flowerManager = FindObjectOfType<FlowerChange>();
        if (flowerManager == null)
        {
            Debug.LogError("场景中未找到FlowerChange组件！", gameObject);
        }
    }

    void OnMouseDown()
    {
        // 检查是否启用拖拽功能
        if (!enableDrag || !flowerCollider.enabled)
            return;
            
        // 开始拖拽
        StartDrag();
    }

    void OnMouseDrag()
    {
        // 检查是否正在拖拽
        if (!isDragging || !enableDrag)
            return;
            
        // 更新花朵位置
        DragFlower();
    }

    void OnMouseUp()
    {
        // 检查是否正在拖拽
        if (!isDragging || !enableDrag)
            return;
            
        // 结束拖拽并处理位置交换
        EndDrag();
    }

    /// <summary>
    /// 开始拖拽操作
    /// </summary>
    private void StartDrag()
    {
        isDragging = true;
        originalPosition = transform.position;
        clickPosition = transform.position; // 记录点击时的位置
        
        // 将花朵置于最前层，确保可见
        Vector3 dragPosition = transform.position;
        dragPosition.z = dragZPosition;
        transform.position = dragPosition;
    }

    /// <summary>
    /// 拖拽过程中更新花朵位置
    /// </summary>
    private void DragFlower()
    {
        // 将鼠标屏幕坐标转换为世界坐标
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z + dragZPosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        
        // 更新花朵位置，保持Z轴不变
        Vector3 newPosition = new Vector3(worldPosition.x, worldPosition.y, dragZPosition);
        transform.position = newPosition;
    }

    /// <summary>
    /// 结束拖拽操作并处理花朵交换
    /// </summary>
    private void EndDrag()
    {
        isDragging = false;
        
        // 查找最近的花朵
        GameObject nearestFlower = FindNearestFlower();
        
        // 如果找到最近的花朵且不是自己，则交换位置
        if (nearestFlower != null && nearestFlower != gameObject)
        {
            // 通过管理器交换花朵位置
            if (flowerManager != null)
            {
                flowerManager.SwapFlowersByObjectWithYFixed(gameObject, nearestFlower, clickPosition);
            }
            else
            {
                // 如果没有管理器，直接交换位置（保持Y轴）
                SwapPositionWithYFixed(nearestFlower);
            }
        }
        else
        {
            // 如果没有找到合适的花朵，回到原始位置（带动画）
            transform.DOMove(originalPosition, 0.3f);
        }
        
        // 检查当前排序是否正确
        if (flowerManager != null && flowerManager.CheckFlowerOrder())
        {
            Debug.Log("恭喜！花朵顺序正确：红橙黄绿青蓝紫");
            // 可以在这里添加游戏完成的逻辑
        }
    }

    /// <summary>
    /// 查找最近的花朵
    /// </summary>
    /// <returns>最近的花朵对象，如果没有找到则返回null</returns>
    private GameObject FindNearestFlower()
    {
        GameObject nearestFlower = null;
        float minDistance = maxSwapDistance;
        
        // 遍历所有花朵查找最近的
        if (flowerManager != null)
        {
            for (int i = 0; i < flowerManager.GetFlowerCount(); i++)
            {
                GameObject flower = flowerManager.GetFlower(i);
                
                // 排除自己
                if (flower == gameObject)
                    continue;
                    
                // 计算距离
                float distance = Vector3.Distance(transform.position, flower.transform.position);
                
                // 更新最近的花朵
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestFlower = flower;
                }
            }
        }
        
        return nearestFlower;
    }

    /// <summary>
    /// 与另一朵花交换位置（保持各自的Y轴）
    /// </summary>
    /// <param name="otherFlower">要交换的另一朵花</param>
    private void SwapPositionWithYFixed(GameObject otherFlower)
    {
        if (otherFlower == null)
            return;
            
        // 保持各自的Y坐标，只交换X坐标
        Vector3 myTargetPosition = new Vector3(otherFlower.transform.position.x, transform.position.y, transform.position.z);
        Vector3 otherTargetPosition = new Vector3(clickPosition.x, otherFlower.transform.position.y, otherFlower.transform.position.z);
        
        // 使用DoTween执行动画
        transform.DOMove(myTargetPosition, 0.3f);
        otherFlower.transform.DOMove(otherTargetPosition, 0.3f);
    }

    /// <summary>
    /// 直接与另一朵花交换位置（保持Y轴不变）
    /// </summary>
    /// <param name="otherFlower">要交换的另一朵花</param>
    private void SwapPositionWith(GameObject otherFlower)
    {
        if (otherFlower == null)
            return;
            
        // 保持各自的Y坐标，只交换X坐标
        Vector3 myTargetPosition = new Vector3(otherFlower.transform.position.x, transform.position.y, transform.position.z);
        Vector3 otherTargetPosition = new Vector3(transform.position.x, otherFlower.transform.position.y, otherFlower.transform.position.z);
        
        // 使用DoTween执行动画
        transform.DOMove(myTargetPosition, 0.3f);
        otherFlower.transform.DOMove(otherTargetPosition, 0.3f);
    }

    /// <summary>
    /// 重置花朵到原始位置
    /// </summary>
    public void ResetToOriginalPosition()
    {
        if (!isDragging)
        {
            transform.position = originalPosition;
        }
    }

    /// <summary>
    /// 获取是否正在拖拽状态
    /// </summary>
    /// <returns>是否正在拖拽</returns>
    public bool IsDragging()
    {
        return isDragging;
    }
}