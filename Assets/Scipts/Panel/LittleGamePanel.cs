using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public enum E_MoveWay
{
    In,
    Out
}

public class LittleGamePanel : BasePanel
{
    public List<Image> Images = new List<Image>();
    private int index = -1;
    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < 9; i++)
        {
            Images.Add(GetControl<Image>("Image"+i));
        }
        BronCard(1);
    }

    protected override void OnClick(string btnName)
    {
        base.OnClick(btnName);
        //TODO：缺少添加ID
        switch (btnName)
        {
            case"CloseBtn":
                UIMgr.Instance().HidePanel("LittleGamePanel");
                break;
            case"Image0":
                CardOut(0);
                index = 0;
                break;
            case"Image1":
                CardOut(1);
                index = 1;
                break;
            case"Image2":
                CardOut(2);
                index = 2;
                break;
            case"Image3":
                CardOut(3);
                Inventory.Instance.AddItem(0).GetComponent<CardItem>().SetCard(index,3);
                break;
            case"Image4":
                CardOut(4);
                Inventory.Instance.AddItem(0).GetComponent<CardItem>().SetCard(index,3);
                break;
            case"Image5":
                CardOut(5);
                Inventory.Instance.AddItem(0).GetComponent<CardItem>().SetCard(index,3);
                break;
            case"Image6":
                CardOut(6);
                Inventory.Instance.AddItem(0);
                break;
            case"Image7":
                CardOut(7);
                Inventory.Instance.AddItem(0);
                break;
            case"Image8":
                CardOut(8);
                Inventory.Instance.AddItem(0);
                break;
        }
    }
    /// <summary>
    ///  卡片下滑
    /// </summary>
    /// <param name="x">第几组（从一开始）</param>
    public void BronCard(int x)
    {
        List<GameObject> cardObj = new List<GameObject>(); 
        for (int i = 3 * (x - 1); i < 3 * x; i++)
        {
            cardObj.Add(Images[i].gameObject);
        }

        CardMove(E_MoveWay.In,cardObj);
    }
    public void CardMove(E_MoveWay moveWay,List<GameObject> cardList)
    {
        if (moveWay==E_MoveWay.In)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                var rt = cardList[i].GetComponent<RectTransform>();
                float posx = rt.anchoredPosition.x;
                rt.DOAnchorPos(new Vector3(posx, 0, 0), 2);
            }
        }
        else
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].GetComponent<Button>().onClick.RemoveAllListeners();
                var rt = cardList[i].GetComponent<RectTransform>();
                float posx = rt.anchoredPosition.x;
                rt.DOAnchorPos(new Vector3(posx, 700, 0), 1);


            }
        }
    }
/// <summary>
/// 移走卡牌
/// </summary>
/// <param name="x">选择的卡牌序号</param>
    public void CardOut(int x)
    {
        List<GameObject> cardObj = new List<GameObject>(); 
        if (0 <= x && x<3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i!=x)
                {
                    cardObj.Add(Images[i].gameObject);
                }
            }
            CardMove(E_MoveWay.Out,cardObj);
            BronCard(2);
        }
        else if(3<=x&& x<6)
        {
            for (int i = 3; i < 6; i++)
            {
                if (i!=x)
                {
                    cardObj.Add(Images[i].gameObject);
                }
            }
            CardMove(E_MoveWay.Out,cardObj);
            BronCard(3);
        }
        else if(6<=x&& x<9)
        {
            for (int i = 6; i < 9; i++)
            {
                if (i!=x)
                {
                    cardObj.Add(Images[i].gameObject);
                }
            }
            CardMove(E_MoveWay.Out,cardObj);
            //TODO：结束小游戏
        }
    }
}
