using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlowerDrag : MonoBehaviour
{
    [Header("��ק����")]
    [Tooltip("�Ƿ�������ק����")]
    public bool enableDrag = true;
    
    [Tooltip("��קʱ��Z��λ�ã�ȷ���������ǰ��")]
    public float dragZPosition = -5f;
    
    [Tooltip("�����������������")]
    public float maxSwapDistance = 3f;
    
    private Camera mainCamera;
    private Vector3 originalPosition;
    private Vector3 clickPosition; // ��¼���ʱ��λ��
    private bool isDragging = false;
    private FlowerChange flowerManager;
    private Collider2D flowerCollider;

    void Start()
    {
        // ��ȡ�������
        mainCamera = Camera.main;
        
        // ��ȡ��ײ�����
        flowerCollider = GetComponent<Collider2D>();
        if (flowerCollider == null)
        {
            Debug.LogError("���������ҪCollider2D������ܽ�����ק������", gameObject);
        }
        
        // ����FlowerChange������
        flowerManager = FindObjectOfType<FlowerChange>();
        if (flowerManager == null)
        {
            Debug.LogError("������δ�ҵ�FlowerChange�����", gameObject);
        }
    }

    void OnMouseDown()
    {
        // ����Ƿ�������ק����
        if (!enableDrag || !flowerCollider.enabled)
            return;
            
        // ��ʼ��ק
        StartDrag();
    }

    void OnMouseDrag()
    {
        // ����Ƿ�������ק
        if (!isDragging || !enableDrag)
            return;
            
        // ���»���λ��
        DragFlower();
    }

    void OnMouseUp()
    {
        // ����Ƿ�������ק
        if (!isDragging || !enableDrag)
            return;
            
        // ������ק������λ�ý���
        EndDrag();
    }

    /// <summary>
    /// ��ʼ��ק����
    /// </summary>
    private void StartDrag()
    {
        isDragging = true;
        originalPosition = transform.position;
        clickPosition = transform.position; // ��¼���ʱ��λ��
        
        // ������������ǰ�㣬ȷ���ɼ�
        Vector3 dragPosition = transform.position;
        dragPosition.z = dragZPosition;
        transform.position = dragPosition;
    }

    /// <summary>
    /// ��ק�����и��»���λ��
    /// </summary>
    private void DragFlower()
    {
        // �������Ļ����ת��Ϊ��������
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z + dragZPosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        
        // ���»���λ�ã�����Z�᲻��
        Vector3 newPosition = new Vector3(worldPosition.x, worldPosition.y, dragZPosition);
        transform.position = newPosition;
    }

    /// <summary>
    /// ������ק�����������佻��
    /// </summary>
    private void EndDrag()
    {
        isDragging = false;
        
        // ��������Ļ���
        GameObject nearestFlower = FindNearestFlower();
        
        // ����ҵ�����Ļ����Ҳ����Լ����򽻻�λ��
        if (nearestFlower != null && nearestFlower != gameObject)
        {
            // ͨ����������������λ��
            if (flowerManager != null)
            {
                flowerManager.SwapFlowersByObjectWithYFixed(gameObject, nearestFlower, clickPosition);
            }
            else
            {
                // ���û�й�������ֱ�ӽ���λ�ã�����Y�ᣩ
                SwapPositionWithYFixed(nearestFlower);
            }
        }
        else
        {
            // ���û���ҵ����ʵĻ��䣬�ص�ԭʼλ�ã���������
            transform.DOMove(originalPosition, 0.3f);
        }
        
        // ��鵱ǰ�����Ƿ���ȷ
        if (flowerManager != null && flowerManager.CheckFlowerOrder())
        {
            Debug.Log("��ϲ������˳����ȷ����Ȼ���������");
            // ���������������Ϸ��ɵ��߼�
        }
    }

    /// <summary>
    /// ��������Ļ���
    /// </summary>
    /// <returns>����Ļ���������û���ҵ��򷵻�null</returns>
    private GameObject FindNearestFlower()
    {
        GameObject nearestFlower = null;
        float minDistance = maxSwapDistance;
        
        // �������л�����������
        if (flowerManager != null)
        {
            for (int i = 0; i < flowerManager.GetFlowerCount(); i++)
            {
                GameObject flower = flowerManager.GetFlower(i);
                
                // �ų��Լ�
                if (flower == gameObject)
                    continue;
                    
                // �������
                float distance = Vector3.Distance(transform.position, flower.transform.position);
                
                // ��������Ļ���
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
    /// ����һ�仨����λ�ã����ָ��Ե�Y�ᣩ
    /// </summary>
    /// <param name="otherFlower">Ҫ��������һ�仨</param>
    private void SwapPositionWithYFixed(GameObject otherFlower)
    {
        if (otherFlower == null)
            return;
            
        // ���ָ��Ե�Y���ֻ꣬����X����
        Vector3 myTargetPosition = new Vector3(otherFlower.transform.position.x, transform.position.y, transform.position.z);
        Vector3 otherTargetPosition = new Vector3(clickPosition.x, otherFlower.transform.position.y, otherFlower.transform.position.z);
        
        // ʹ��DoTweenִ�ж���
        transform.DOMove(myTargetPosition, 0.3f);
        otherFlower.transform.DOMove(otherTargetPosition, 0.3f);
    }

    /// <summary>
    /// ֱ������һ�仨����λ�ã�����Y�᲻�䣩
    /// </summary>
    /// <param name="otherFlower">Ҫ��������һ�仨</param>
    private void SwapPositionWith(GameObject otherFlower)
    {
        if (otherFlower == null)
            return;
            
        // ���ָ��Ե�Y���ֻ꣬����X����
        Vector3 myTargetPosition = new Vector3(otherFlower.transform.position.x, transform.position.y, transform.position.z);
        Vector3 otherTargetPosition = new Vector3(transform.position.x, otherFlower.transform.position.y, otherFlower.transform.position.z);
        
        // ʹ��DoTweenִ�ж���
        transform.DOMove(myTargetPosition, 0.3f);
        otherFlower.transform.DOMove(otherTargetPosition, 0.3f);
    }

    /// <summary>
    /// ���û��䵽ԭʼλ��
    /// </summary>
    public void ResetToOriginalPosition()
    {
        if (!isDragging)
        {
            transform.position = originalPosition;
        }
    }

    /// <summary>
    /// ��ȡ�Ƿ�������ק״̬
    /// </summary>
    /// <returns>�Ƿ�������ק</returns>
    public bool IsDragging()
    {
        return isDragging;
    }
}