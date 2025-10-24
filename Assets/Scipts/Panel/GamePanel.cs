using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    private List<Image> itemImages = new List<Image>(); 
    private Canvas canvas;
    private Image DialogImg;
    private Text SpeakText;
    public GameObject 测试用;
    protected override void Awake()
    {
        base.Awake();
        SpeakText = GetControl<Text>("speak");
        DialogImg = GetControl<Image>("DialogBox");
        DialogImg.gameObject.SetActive(false);
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
                Inventory.Instance.SelectItem(0);
                break;
            case"PropBtn2":
                Inventory.Instance.SelectItem(1);
                break;
            case"PropBtn3":
                Inventory.Instance.SelectItem(2);
                break;
            case"PropBtn4":
                Inventory.Instance.SelectItem(3);
                break;
            case"PropBtn5":
                Inventory.Instance.SelectItem(4);
                break;
            case"PropBtn6":
                Inventory.Instance.SelectItem(5);
                break;

                #region 注释

            // case"启动对话框":
            //     textImg.gameObject.SetActive(true);
            //     break;
            // case"关闭对话框":
            //     textImg.gameObject.SetActive(false);

            #endregion

            case"玩小游戏":
                UIMgr.Instance().ShowPanel<BasePanel>("LittleGamePanel", E_UI_Layer.Top);
                break;
            case "对话框生成测试":
                InitDialogBox("姑姑嘎嘎",测试用.transform.position);
                break;
            case "对话框关闭测试":
                CloseDialogBox();
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
/// <summary>
/// 生成对话框
/// </summary>
/// <param name="speak"></param>
/// <param name="pos"></param>
    public void InitDialogBox(string speak, Vector3 pos)
    {
        if (canvas == null)
        {
            Debug.LogError("Canvas 为 null，无法生成对话框");
            return;
        }
        if (DialogImg == null)
        {
            Debug.LogError("DialogImg 为 null，无法生成对话框");
            return;
        }
        if (SpeakText == null)
        {
            Debug.LogWarning("SpeakText 为 null，文本不会显示");
        }

        RectTransform canvasRect = canvas.transform as RectTransform;
        if (canvasRect == null)
        {
            Debug.LogError("canvas 的 RectTransform 未找到");
            return;
        }

        // 世界坐标 -> 屏幕坐标
        Camera worldCam = Camera.main;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCam, pos);

        // 屏幕坐标 -> Canvas 本地坐标（ScreenSpace-Overlay 时传 null camera）
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out Vector2 localPoint);

        // 确保 DialogImg 是 Canvas 的直接子物体
        RectTransform dialogRect = DialogImg.rectTransform;
        if (dialogRect.parent != canvasRect)
        {
            dialogRect.SetParent(canvasRect, false);
        }

        DialogImg.gameObject.SetActive(true);
        dialogRect.anchoredPosition = localPoint;
        if (SpeakText != null) SpeakText.text = speak;
    }
/// <summary>
/// 关闭对话框
/// </summary>
    public void CloseDialogBox()
    {
        DialogImg.gameObject.SetActive(false);
    }
    
}
