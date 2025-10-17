using System;
using System.Runtime.InteropServices;
using UnityEngine;

//1.����Ϸ������õ� "����D3D11��ʹ��DXGI��תģ�ͽ�����" �ر�
//2.���������Ϊ��ɫ��ColorΪ(0,0,0,0)
//3.��TransparentWindow�ű����ص������
public class TransparentWindow : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;

    // ���һ����־�����ٵ�ǰ��͸״̬
    private bool isMousePenetrationEnabled = true;
    // ��ӱ�־��������������״̬
    private bool isTaskbarHidden = false;
    // ��ӱ�־�����Ƿ���������������
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

    // ��ӻ�ȡȫ�����λ�õ�API
    [DllImport("user32.dll")]
    static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

    // ���������API
    [DllImport("user32.dll")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // ������ʾ״̬API
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
    // ��Ӳ���ʾ������������չ��ʽ
    const uint WS_EX_TOOLWINDOW = 0x00000080;

    const int SWP_FRAMECHANGED = 0x0020;
    const int SWP_SHOWWINDOW = 0x0040;
    const int LWA_ALPHA = 2;

    // ShowWindow����
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
        // ��ʼ״̬������괩͸
        SetWindowLong(_hwnd, GWL_EXSTYLE, WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TRANSPARENT);
        DwmExtendFrameIntoClientArea(_hwnd, ref margins);
        SetWindowPos(_hwnd, HWND_TOPMOST, 0, 0, fWidth, fHeight, SWP_FRAMECHANGED | SWP_SHOWWINDOW); 
        ShowWindowAsync(_hwnd, 3); //Forces window to show in case of unresponsive app    // SW_SHOWMAXIMIZED(3)
        
        // ��ȡ���������
        taskbarHwnd = FindWindow("Shell_TrayWnd", null);
#endif
        mainCamera = Camera.main;
        ToggleTaskbarVisibility();
    }

    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }

    // ��ӷ��������л���괩͸״̬
    public void SetMousePenetration(bool enable)
    {
#if !UNITY_EDITOR
        if (_hwnd != IntPtr.Zero && isMousePenetrationEnabled != enable)
        {
            if (enable)
            {
                // ������괩͸ - ���WS_EX_TRANSPARENT��־
                uint exStyle = WS_EX_TOPMOST | WS_EX_LAYERED | WS_EX_TRANSPARENT;
                if (isHiddenInTaskbar)
                {
                    exStyle |= WS_EX_TOOLWINDOW;
                }
                SetWindowLong(_hwnd, GWL_EXSTYLE, exStyle);
            }
            else
            {
                // ������괩͸ - �Ƴ�WS_EX_TRANSPARENT��־
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

    // �������/��ʾ�������ķ���
    public void ToggleTaskbar()
    {
#if !UNITY_EDITOR
        if (taskbarHwnd != IntPtr.Zero)
        {
            if (isTaskbarHidden)
            {
                // ��ʾ������
                ShowWindow(taskbarHwnd, SW_SHOW);
                isTaskbarHidden = false;
            }
            else
            {
                // ����������
                ShowWindow(taskbarHwnd, SW_HIDE);
                isTaskbarHidden = true;
            }
        }
#endif
    }

    // �������/��ʾ���������еķ���
    public void ToggleTaskbarVisibility()
    {
#if !UNITY_EDITOR
        if (_hwnd != IntPtr.Zero)
        {
            uint currentExStyle = 0;
            if (isHiddenInTaskbar)
            {
                // �ָ���ʾ��������
                currentExStyle = WS_EX_TOPMOST | WS_EX_LAYERED;
                if (isMousePenetrationEnabled)
                {
                    currentExStyle |= WS_EX_TRANSPARENT;
                }
                isHiddenInTaskbar = false;
            }
            else
            {
                // ��������������
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
        // �� T ���л���괩͸״̬
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetMousePenetration(!isMousePenetrationEnabled);
        }

        /*// �� H ���л���������ʾ/����
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleTaskbar();
        }

        // �� Y ���л�Unity����������������ʾ/����
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleTaskbarVisibility();
        }*/

        // ����������λ�ò���������
        CheckMousePositionAndRaycast();
#endif
    }

    void CheckMousePositionAndRaycast()
    {
#if !UNITY_EDITOR
        // ��ȡȫ�����λ��
        if (GetCursorPos(out POINT cursorPos))
        {
            // ����Ļ����ת��Ϊ���ڿͻ�������
            POINT clientPos = cursorPos;
            if (ScreenToClient(_hwnd, ref clientPos))
            {
                // �������Ƿ��ڴ�����
                if (clientPos.X >= 0 && clientPos.Y >= 0 && 
                    clientPos.X < Screen.width && clientPos.Y < Screen.height)
                {
                    // ���ͻ�������ת��ΪUnity��Ļ����
                    Vector3 screenPoint = new Vector3(clientPos.X, Screen.height - clientPos.Y, 0);
                    
                    // �������߼��
                    Ray ray = mainCamera.ScreenPointToRay(screenPoint);
                    RaycastHit hit;
                    
                    // �������߼���������Ƿ�͸���
                    bool shouldPenetrate = !Physics.Raycast(ray, out hit);
                    SetMousePenetration(shouldPenetrate);
                }
            }
        }
#endif
    }

    // ��Ӧ�ó����˳�ʱ�ָ�����������Ҫ����
    void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        // ȷ���������ڳ����˳�ʱ�ɼ�
        if (taskbarHwnd != IntPtr.Zero && isTaskbarHidden)
        {
            ShowWindow(taskbarHwnd, SW_SHOW);
        }
        
        // ȷ������ͼ�����������пɼ�
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