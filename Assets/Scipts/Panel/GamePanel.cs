using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePanel : BasePanel
{
    private List<Image> itemImages = new List<Image>(); 
    private Canvas canvas;
    private Image DialogImg;
    private Text SpeakText;
    public Vector3 测试用;
    public RectTransform PropImgrt;
    public GameObject realPropboxposobj;
    private Vector2 hidePos;
    private Vector2 realPos;
    public bool isPropBoxshow = false;
    
    private int uiSelectedIndex = -1;//当前 UI 选中索引（-1 表示无选中）
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
        while (i < 5)
        {
            itemImages.Add(GetControl<Image>("PropBtn" + (i + 1)).gameObject.GetComponent<Image>());
            i++;
        }

        Image PropImg = GetControl<Image>("PropImg");
        PropImgrt = PropImg.gameObject.GetComponent<RectTransform>();
        hidePos = PropImgrt.anchoredPosition;
        realPos = realPropboxposobj.GetComponent<RectTransform>().anchoredPosition;
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
                ToggleSelectByUI(0);
                break;
            case"PropBtn2":
                ToggleSelectByUI(1);
                break;
            case"PropBtn3":
                ToggleSelectByUI(2);
                break;
            case"PropBtn4":
                ToggleSelectByUI(3);
                break;
            case"PropBtn5":
                ToggleSelectByUI(4);
                break;
            // case"PropBtn6":
            //     ToggleSelectByUI(5);
            //     break;

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
                InitDialogBox("姑姑嘎嘎",测试用);
                break;
            case "对话框关闭测试":
                CloseDialogBox();
                break;
        }
    }

    #region 修改物品栏显示

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

    #endregion

    #region 对话框生成关闭

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
            StartCoroutine(CloseDialogBox());
        }
    /// <summary>
    /// 关闭对话框
    /// </summary>
        IEnumerator CloseDialogBox()
        {
            yield return new WaitForSeconds(2f);
            DialogImg.gameObject.SetActive(false);
        }

    #endregion


    #region 道具栏动画

    public void PropBoxIn()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(PropImgrt.DOAnchorPos(realPos, 0.5f))
            .AppendCallback(() =>
            {
                print("动画完成");
                print(isPropBoxshow);
                if (!isPropBoxshow)
                {
                    PropBoxOut2();
                }
            });
        // .OnComplete(); 
    }
    public void PropBoxin2()
    {
        isPropBoxshow = true;
        //print(isPropBoxshow);
    }
    public void PropBoxOut2()
    {
        isPropBoxshow = false;
        //print(isPropBoxshow);
        PropImgrt.DOAnchorPos(hidePos, 0.5f);
    }

    #endregion
    /// <summary>
    /// UI 选中切换
    /// </summary>
    /// <param name="index"></param>
    private void ToggleSelectByUI(int index)
    {
        if (uiSelectedIndex == index)
        {
            print("取消选中道具栏"+index);
            // 取消选中
            uiSelectedIndex = -1;
            //UpdateSelectionHighlight(uiSelectedIndex);
            // TODO：若需要同步背包状态，取消背包当前选中
            
        }
        else
        {
            print("选中道具栏"+index);
            // 选中新索引
            uiSelectedIndex = index;
            // 同步背包当前选中（若有背包逻辑）
            Inventory.Instance?.SelectItem(index);
        }

        UpdateSelectionHighlight();
    }
    private void GetNormalColor(Image img)
    {
        img.color = new Color32(255,255,255,255);
        //return colors.normalColor;
    }
    private void GetSelectedColor(Image img)
    {
        img.color = new Color32(81, 81, 81,255);
    }
    // 替换原有的刷新高亮方法：未选中→normal，选中→selected
    private void UpdateSelectionHighlight()
    {
        for (int i = 0; i < itemImages.Count; i++)
        {
            if (i == uiSelectedIndex)
            {
                // 设置为选中颜色
                GetSelectedColor(itemImages[i]);
            }
            else
            {
                // 设置为正常颜色
                GetNormalColor(itemImages[i]);
            }
        }
    }
    
}
