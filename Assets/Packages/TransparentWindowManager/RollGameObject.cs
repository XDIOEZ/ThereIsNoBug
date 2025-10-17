using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollGameObject : MonoBehaviour
{
    [Header("旋转设置")]
    [SerializeField] private float rotationSpeed = 0.5f;
    [SerializeField] private bool reverseX = true;
    [SerializeField] private bool reverseY = true;

    [Header("平移设置")]
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private bool reverseMoveX = false;
    [SerializeField] private bool reverseMoveY = false;

    [Header("调试设置")]
    [SerializeField] private bool debugAll = false; // ✅ 全局 Debug 开关

    private Camera mainCamera;
    private Vector3 lastMousePosition;
    private bool isDragging = false;
    private bool isMiddleDragging = false;
    private Quaternion initialRotation;
    private Quaternion currentRotation;

    void Start()
    {
        initialRotation = transform.rotation;
        currentRotation = initialRotation;
        mainCamera = Camera.main;

        if (mainCamera == null && debugAll)
            Debug.LogWarning("[RollGameObject] ⚠️ 主相机未找到，请确保相机带有 'MainCamera' 标签。");
    }

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        // 🔹 左键旋转检测
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
            if (debugAll) Debug.Log("[RollGameObject] 左键按下");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            if (debugAll) Debug.Log("[RollGameObject] 左键抬起");
        }

        // 🔹 中键平移检测
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePosition = Input.mousePosition;
            isMiddleDragging = true;
            if (debugAll) Debug.Log("[RollGameObject] 🖱 中键按下");
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isMiddleDragging = false;
            if (debugAll) Debug.Log("[RollGameObject] 🖱 中键抬起");
        }

        // 🔹 左键旋转逻辑
        if (isDragging)
        {
            Vector3 deltaPosition = Input.mousePosition - lastMousePosition;

            float deltaX = reverseX ? -deltaPosition.x : deltaPosition.x;
            float deltaY = reverseY ? -deltaPosition.y : deltaPosition.y;

            Quaternion yRotation = Quaternion.AngleAxis(deltaX * rotationSpeed, Vector3.up);
            Quaternion xRotation = Quaternion.AngleAxis(-deltaY * rotationSpeed, Vector3.right);

            currentRotation = xRotation * yRotation * currentRotation;
            transform.rotation = currentRotation;

            lastMousePosition = Input.mousePosition;

            if (debugAll)
                Debug.Log($"[RollGameObject] 左键旋转 deltaX={deltaX}, deltaY={deltaY}, 当前旋转={currentRotation.eulerAngles}");
        }

        // 🔹 中键平移逻辑
        if (isMiddleDragging)
        {
            if (mainCamera == null) return;

            Vector3 delta = Input.mousePosition - lastMousePosition;
            float moveX = reverseMoveX ? -delta.x : delta.x;
            float moveY = reverseMoveY ? -delta.y : delta.y;

            Vector3 right = mainCamera.transform.right;
            Vector3 up = mainCamera.transform.up;

            Vector3 move = (-right * moveX - up * moveY) * moveSpeed;
            transform.position += move;

            if (debugAll)
                Debug.Log($"[RollGameObject] 中键拖动: delta={delta}, move增量={move}, 新位置={transform.position}");

            lastMousePosition = Input.mousePosition;
        }

        // 🔹 滚轮缩放
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 scale = transform.localScale;
            scale += Vector3.one * scroll;
            scale = new Vector3(
                Mathf.Clamp(scale.x, 0.1f, 10f),
                Mathf.Clamp(scale.y, 0.1f, 10f),
                Mathf.Clamp(scale.z, 0.1f, 10f)
            );
            transform.localScale = scale;

            if (debugAll)
                Debug.Log($"[RollGameObject] 滚轮缩放: scroll={scroll}, 新缩放={transform.localScale}");
        }
    }

    public void ResetTransform()
    {
        currentRotation = initialRotation;
        transform.rotation = initialRotation;
        transform.localScale = Vector3.one;

        if (debugAll)
            Debug.Log("[RollGameObject] 🔄 重置 Transform 完成");
    }

    public void SetRotationSpeed(float speed) => rotationSpeed = speed;

    public void SetRotation(Quaternion rotation)
    {
        currentRotation = rotation;
        transform.rotation = rotation;
    }

    public Quaternion GetCurrentRotation() => currentRotation;
}
