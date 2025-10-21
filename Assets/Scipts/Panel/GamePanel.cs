using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    #region 注释

    // public UIDrag propBtn1drag;
    // public UIDrag propBtn2drag;
    // public UIDrag propBtn3drag;
    // public UIDrag propBtn4drag;
    // public UIDrag propBtn5drag;
    // public UIDrag propBtn6drag;

    #endregion

    private Image textImg;

    protected override void Awake()
    {
        base.Awake();
        #region 注释

        // propBtn1drag = GetControl<Image>("PropBtn1").gameObject.GetComponent<UIDrag>();
        // propBtn2drag = GetControl<Image>("PropBtn2").gameObject.GetComponent<UIDrag>();
        // propBtn3drag = GetControl<Image>("PropBtn3").gameObject.GetComponent<UIDrag>();
        // propBtn4drag = GetControl<Image>("PropBtn4").gameObject.GetComponent<UIDrag>();
        // propBtn5drag = GetControl<Image>("PropBtn5").gameObject.GetComponent<UIDrag>();
        // propBtn6drag = GetControl<Image>("PropBtn6").gameObject.GetComponent<UIDrag>();

        #endregion

        textImg = GetControl<Image>("TextImage");
        textImg.gameObject.SetActive(false);
    }
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case"SettingBtn":
                UIMgr.Instance().ShowPanel<BasePanel>("SettingPanel", E_UI_Layer.Top);
                break;
            case"PropBtn1":
                print("道具1拖到了某处并使用");
                break;
            case"PropBtn2":
                print("道具2拖到了某处并使用");
                break;
            case"PropBtn3":
                print("道具3拖到了某处并使用");
                break;
            case"PropBtn4":
                print("道具4拖到了某处并使用");
                break;
            case"PropBtn5":
                print("道具5拖到了某处并使用");
                break;
            case"PropBtn6":
                print("道具6拖到了某处并使用");
                break;
            case"启动对话框":
                textImg.gameObject.SetActive(true);
                break;
            case"关闭对话框":
                textImg.gameObject.SetActive(false);
                break;
            case"玩小游戏":
                UIMgr.Instance().ShowPanel<BasePanel>("LittleGamePanel", E_UI_Layer.Top);
                break;
            case"RightBtn":
                print("向右移动");
                break;
            case"LeftBtn":
                print("向左移动");
                break;
            case"ForwardBtn":
                print("向前移动");
                break;
            case"BehindBtn":
                print("向后移动");
                break;
        }
    }
}
