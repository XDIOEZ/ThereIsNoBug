using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public List<Image> itemImages; 

    // private Image textImg;
    private Canvas canvas;

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
        //获取Canvas组件 方便后续使用
        canvas = UIMgr.Instance().canvas.gameObject.GetComponent<Canvas>();
        int i=0;
        while (i < 6)
        {
            itemImages.Add(GetControl<Image>("PropBtn" + (i + 1)).gameObject.GetComponent<Image>());
            i++;
        }
        // textImg = GetControl<Image>("TextImage");
        // textImg.gameObject.SetActive(false);
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
                print("选择了0位置道具");
                break;
            case"PropBtn2":
                print("选择了1位置道具");
                break;
            case"PropBtn3":
                print("选择了2位置道具");
                break;
            case"PropBtn4":
                print("选择了3位置道具");
                break;
            case"PropBtn5":
                print("选择了4位置道具");
                break;
            case"PropBtn6":
                print("选择了5位置道具");
                break;
            // case"启动对话框":
            //     textImg.gameObject.SetActive(true);
            //     break;
            // case"关闭对话框":
            //     textImg.gameObject.SetActive(false);
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
    public void GetItem(Item item)
    {
        int index = item.GetComponent<InventoryComponent>().index;
        ChangeSprite(index, item);
    }

    public void ChangeSprite(int index,Item item)
    {
        
        if (item.GetImagePath()!=null)
        {
            //TODO: 后续根据图集与否修改使用方法
            itemImages[index].sprite =  ResMgr.Instance().Load<Sprite>(item.GetImagePath());
            
        }
    }
    public void RemoveItem(Item item)
    {
        int _index = item.GetComponent<InventoryComponent>().index;
        itemImages[_index].sprite = null;
    }

    public void InitDialogBox(string speak,Vector3 pos)
    {
        Camera worldCam = Camera.main;
        // 世界坐标 -> 屏幕坐标
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCam, pos);
        
        // 屏幕坐标 -> Canvas 本地坐标（ScreenSpace-Overlay 时传 null camera）
        RectTransform canvasRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPoint);
        //生成对话框在localPoint位置 
        
    }
}
