using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel:BasePanel
{
    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        switch (btnName)
        {
            case "CloseBtn":
                UIMgr.Instance().HidePanel("SettingPanel");
                break;
            case "ReloadBtn":
                break;
            case"ExitBtn":
                break;
        }
    }
}
