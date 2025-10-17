using System;
using System.Runtime.InteropServices;
using UnityEngine;

//1.将游戏打包设置的 "对于D3D11，使用DXGI翻转模型交换链" 关闭
//2.将相机设置为纯色且Color为(0,0,0,0)
//3.将TransparentWindow脚本挂载到相机上
public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    // 添加一个标志来跟踪当前穿透状态
    private bool isMousePenetrationEnabled = true;
    // 添加标志跟踪任务栏隐藏状态
    private bool isTaskbarHidden = false;
    // 添加标志跟踪是否隐藏在任务栏中
    private bool isHiddenInTaskbar = false;

    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern int SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);

    [DllImport("User32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    // 添加获取全局鼠标位置的API
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    // 任务栏相关API
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // 窗口显示状态API
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
    }

    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;
    const uint WS_POPUP = 0x80000000;
    const uint WS_VISIBLE = 0x10000000;

    const uint WS_EX_TOPMOST = 0x00000008;
    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;
    // 添加不显示在任务栏的扩展样式
    const uint WS_EX_TOOLWINDOW = 0x00000080;

    const int SWP_FRAMECHANGED = 0x0020;
    const int SWP_SHOWWINDOW = 0x0040;
    const int LWA_ALPHA = 2;

    // ShowWindow参数
    const int SW_HIDE = 0;
    const int SW_SHOW = 5;
    const int SW_RESTORE = 9;

    private IntPtr HWND_TOPMOST = new IntPtr(-1);
    private IntPtr HWND_NOTOPMOST = new IntPtr(-2);

    private IntPtr _hwnd;
    private IntPtr taskbarHwnd;
    private Camera mainCamera;

    void Start()
    {
#if !UNITY_EDITOR
        MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
        _hwnd = GetActiveWindow();
        int fWidth = Screen.width;
        int fHeight = Screen.height;

        SetWindowLong(_hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        // 初始状态启用鼠标穿透
        SetWindowLong(_hwnd, GWL_EXSTYLE, WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        DwmExtendFrameIntoClientArea(_hwnd, ref margins);
        SetWindowPos(_hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, SWP_FRAMECHANGED | SWP_SHOWWINDOW); 
        ShowWindowAsync(_hwnd, 3); //Forces window to show in case of unresponsive app    // SW_SHOWMAXIMIZED(3)
        
        // 获取任务栏句柄
        taskbarHwnd = FindWindow("Shell_TrayWnd", null);
#endif
        mainCamera = Camera.main;
        ToggleTaskbarVisibility();
    }

    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }

    // 添加方法用于切换鼠标穿透状态
    public void SetMousePenetration(bool enable)
    {
#if !UNITY_EDITOR
        if (_hwnd != IntPtr.Zero && isMousePenetrationEnabled != enable)
        {
            if (enable)
            {
                // 启用鼠标穿透 - 添加WS_EX_TRANSPARENT标志
                uint exStyle = WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TRANSPARENT;
                if (isHiddenInTaskbar)
                {
                    exStyle |= WS_EX_TOOLWINDOW;
                }
                SetWindowLong(_hwnd, GWL_EXSTYLE, exStyle);
            }
            else
            {
                // 禁用鼠标穿透 - 移除WS_EX_TRANSPARENT标志
                uint exStyle = WS_EX_TOPMOST | WS_EX_LAYERED;
                if (isHiddenInTaskbar)
                {
                    exStyle |= WS_EX_TOOLWINDOW;
                }
                SetWindowLong(_hwnd, GWL_EXSTYLE, exStyle);
            }
            isMousePenetrationEnabled = enable;
        }
#endif
    }

    // 添加隐藏/显示任务栏的方法
    public void ToggleTaskbar()
    {
#if !UNITY_EDITOR
        if (taskbarHwnd != IntPtr.Zero)
        {
            if (isTaskbarHidden)
            {
                // 显示任务栏
                ShowWindow(taskbarHwnd, SW_SHOW);
                isTaskbarHidden = false;
            }
            else
            {
                // 隐藏任务栏
                ShowWindow(taskbarHwnd, SW_HIDE);
                isTaskbarHidden = true;
            }
        }
#endif
    }

    // 添加隐藏/显示在任务栏中的方法
    public void ToggleTaskbarVisibility()
    {
#if !UNITY_EDITOR
        if (_hwnd != IntPtr.Zero)
        {
            uint currentExStyle = 0;
            if (isHiddenInTaskbar)
            {
                // 恢复显示在任务栏
                currentExStyle = WS_EX_TOPMOST | WS_EX_LAYERED;
                if (isMousePenetrationEnabled)
                {
                    currentExStyle |= WS_EX_TRANSPARENT;
                }
                isHiddenInTaskbar = false;
            }
            else
            {
                // 隐藏在任务栏中
                currentExStyle = WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TOOLWINDOW;
                if (isMousePenetrationEnabled)
                {
                    currentExStyle |= WS_EX_TRANSPARENT;
                }
                isHiddenInTaskbar = true;
            }
            SetWindowLong(_hwnd, GWL_EXSTYLE, currentExStyle);
        }
#endif
    }

    void Update()
    {
#if !UNITY_EDITOR
        // 按 T 键切换鼠标穿透状态
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetMousePenetration(!isMousePenetrationEnabled);
        }

        /*// 按 H 键切换任务栏显示/隐藏
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleTaskbar();
        }

        // 按 Y 键切换Unity程序在任务栏的显示/隐藏
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleTaskbarVisibility();
        }*/

        // 持续检测鼠标位置并发送射线
        CheckMousePositionAndRaycast();
#endif
    }

    void CheckMousePositionAndRaycast()
    {
#if !UNITY_EDITOR
        // 获取全局鼠标位置
        if (GetCursorPos(out POINT cursorPos))
        {
            // 将屏幕坐标转换为窗口客户区坐标
            POINT clientPos = cursorPos;
            if (ScreenToClient(_hwnd, ref clientPos))
            {
                // 检查鼠标是否在窗口内
                if (clientPos.X >= 0 && clientPos.Y >= 0 && 
                    clientPos.X < Screen.width && clientPos.Y < Screen.height)
                {
                    // 将客户区坐标转换为Unity屏幕坐标
                    Vector3 screenPoint = new Vector3(clientPos.X, Screen.height - clientPos.Y, 0);
                    
                    // 发送射线检测
                    Ray ray = mainCamera.ScreenPointToRay(screenPoint);
                    RaycastHit hit;
                    
                    // 根据射线检测结果决定是否穿透鼠标
                    bool shouldPenetrate = !Physics.Raycast(ray, out hit);
                    SetMousePenetration(shouldPenetrate);
                }
            }
        }
#endif
    }

    // 当应用程序退出时恢复任务栏（重要！）
    void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        // 确保任务栏在程序退出时可见
        if (taskbarHwnd != IntPtr.Zero && isTaskbarHidden)
        {
            ShowWindow(taskbarHwnd, SW_SHOW);
        }
        
        // 确保程序图标在任务栏中可见
        if (_hwnd != IntPtr.Zero && isHiddenInTaskbar)
        {
            uint exStyle = WS_EX_TOPMOST | WS_EX_LAYERED;
            if (isMousePenetrationEnabled)
            {
                exStyle |= WS_EX_TRANSPARENT;
            }
            SetWindowLong(_hwnd, GWL_EXSTYLE, exStyle);
        }
#endif
    }
}