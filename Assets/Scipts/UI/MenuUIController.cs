using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    public CanvasGroup menuRoot;

    [Header("事件广播")]
    public VoidEventSO newGameEvent;

    private bool _clicked;

    public void OnClickStart()
    {
        if (_clicked) return; // 防止双击
        _clicked = true;

        HideMenuImmediate();

        // 触发开始游戏流程（SceneLoadManager 监听此事件）
        newGameEvent?.OnEventRaised?.Invoke();
    }

    private void HideMenuImmediate()
    {
        if (menuRoot != null)
        {
            // 立刻隐藏并禁用交互
            menuRoot.alpha = 0f;
            menuRoot.interactable = false;
            menuRoot.blocksRaycasts = false;
        }
        else
        {
            // 没有 CanvasGroup 时，直接关闭整个面板
            gameObject.SetActive(false);
        }
    }
}
