using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel: BasePanel
{
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "StartBtn":
                
                UIMgr.Instance().HidePanel("StartPanel");
                UIMgr.Instance().ShowPanel<BasePanel>("GamePanel", E_UI_Layer.Mid);
                break;
            case "ExitBtn":
                break;
        }
    }
}
