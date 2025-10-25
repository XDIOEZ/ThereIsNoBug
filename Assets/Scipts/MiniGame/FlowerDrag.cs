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

    [Tooltip("检测最近花朵的最大距离")]
    public float maxSwapDistance = 3f;

    private Camera mainCamera;
    private Vector3 originalPosition;
    private Vector3 clickPosition;
    private float keepZ; // 记录初始Z轴
    private bool isDragging = false;
    private FlowerChange flowerManager;
    private Collider2D flowerCollider;

    void Start()
    {
        mainCamera = Camera.main;

        flowerCollider = GetComponent<Collider2D>();
        if (flowerCollider == null)
            Debug.LogError("需添加 Collider2D 组件!", gameObject);

        flowerManager = FindObjectOfType<FlowerChange>();
        if (flowerManager == null)
            Debug.LogError("场景中未找到 FlowerChange!", gameObject);
    }

    void OnMouseDown()
    {
        if (!enableDrag || !flowerCollider.enabled) return;
        StartDrag();
    }

    void OnMouseDrag()
    {
        if (!enableDrag || !isDragging) return;
        DragFlower();
    }

    void OnMouseUp()
    {
        if (!isDragging || !enableDrag) return;
        EndDrag();
    }

    private Tweener dragTween; // 拖拽跟随 Tween

    private void StartDrag()
    {
        isDragging = true;
        originalPosition = transform.position;
        clickPosition = transform.position;
        keepZ = transform.position.z;

        // 初始化持续 tween：不自动完成
        dragTween = transform.DOMove(transform.position, 0.15f)
            .SetEase(Ease.OutQuad)
            .SetAutoKill(false)
            .Pause();
    }

    private void DragFlower()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z - keepZ);
        Vector3 targetPos = mainCamera.ScreenToWorldPoint(mousePosition);
        targetPos.z = keepZ;

        if (!dragTween.IsPlaying()) dragTween.Play();

        // 动态更新目标值达到平滑效果
        dragTween.ChangeEndValue(targetPos, true);
    }

    private void EndDrag()
    {
        isDragging = false;

        if (dragTween != null)
        {
            dragTween.Kill();
            dragTween = null;
        }

        FlowerDrag nearestFlower = FindNearestFlower();

        if (nearestFlower != null && nearestFlower != this)
        {
            if (flowerManager != null)
            {
                flowerManager.SwapFlowersByObjectWithYFixed(gameObject, nearestFlower.gameObject, clickPosition);
            }
            else
            {
                SwapPositionWithYFixed(nearestFlower.gameObject);
            }
        }
        else
        {
            transform.DOMove(originalPosition, 0.3f).SetEase(Ease.OutQuad);
        }

        if (flowerManager != null && flowerManager.CheckFlowerOrder())
        {
            Debug.Log("✅ 顺序正确!");
        }
    }

    /// <summary>
    /// 查找最近花朵
    /// </summary>
    private FlowerDrag FindNearestFlower()
    {
        FlowerDrag nearestFlower = null;
        float minDistance = maxSwapDistance;

        if (flowerManager != null)
        {
            for (int i = 0; i < flowerManager.GetFlowerCount(); i++)
            {
                FlowerDrag flower = flowerManager.GetFlower(i);
                if (flower == this) continue;

                float distance = Vector3.Distance(transform.position, flower.transform.position);
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
    /// 保持Y交换位置 (DoTween动画)
    /// </summary>
    private void SwapPositionWithYFixed(GameObject otherFlower)
    {
        if (!otherFlower) return;

        Vector3 myTargetPos = new Vector3(otherFlower.transform.position.x, transform.position.y, keepZ);
        Vector3 otherTargetPos = new Vector3(clickPosition.x, otherFlower.transform.position.y, otherFlower.transform.position.z);

        transform.DOMove(myTargetPos, 0.3f);
        otherFlower.transform.DOMove(otherTargetPos, 0.3f);
    }

    public bool IsDragging() => isDragging;
}
