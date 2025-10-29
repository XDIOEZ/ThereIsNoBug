using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                CardOut(0,102);
                //index = 102;
                break;
            case"Image1":
                CardOut(1,103);
                //index = 103;
                break;
            case"Image2":
                CardOut(2,104);
                //index = 104;
                break;
            case"Image3":
                CardOut(3,105);
                //Inventory.Instance.AddItem(110).GetComponent<CardItem>().SetCard(index,105);
                break;
            case"Image4":
                CardOut(4,106);
                //Inventory.Instance.AddItem(110).GetComponent<CardItem>().SetCard(index,106);
                break;
            case"Image5":
                CardOut(5,107);
                //Inventory.Instance.AddItem(110).GetComponent<CardItem>().SetCard(index,107);
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
                rt.DOAnchorPos(new Vector3(posx, 0, 0), 1.5f);
            }
        }
        else
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                cardList[i].GetComponent<Button>().onClick.RemoveAllListeners();
                var rt = cardList[i].GetComponent<RectTransform>();
                float posx = rt.anchoredPosition.x;
                rt.DOAnchorPos(new Vector3(posx, 900, 0), 1);


            }
        }
    }
/// <summary>
/// 移走卡牌
/// </summary>
/// <param name="x">选择的卡牌序号</param>
    public void CardOut(int x,int id)
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
                else
                {
                    CardGet(i,id);
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
                else
                {
                    CardGet(i,id);
                }
            }
            CardMove(E_MoveWay.Out,cardObj);

        }


    }
    /// <summary>
    /// 获得的卡牌向下移走
    /// </summary>
    public void CardGet(int x,int id)
    {
        print("获得卡牌"+x);
        var rt = Images[x].GetComponent<RectTransform>();
        float posx = rt.anchoredPosition.x;
        rt.DOAnchorPos(new Vector3(posx,-900,0),1).OnComplete(() =>
        {
            print("添加卡牌到背包");
            if (0<=x&&x<3)
            {
                index=id;
            }
            else
            {
                Inventory.Instance.AddItem(110).GetComponent<CardItem>().SetCard(index,id);
                if (x == 4 && id == 106&&index==102)
                {
                    Debug.Log("1");
                    UIMgr.Instance().GetPanel<GamePanel>("GamePanel").InitDialogBox("很好！你抓住了命运的关键——这两张牌的特别之处，正是你接下来要追寻的线索。",
                        GameObject.Find("areas2").transform.Find("占卜师").transform.position+ new Vector3(0.5f, 1f));
                }
                else if (x >= 3)
                {
                    Debug.Log("2");
                    UIMgr.Instance().GetPanel<GamePanel>("GamePanel").InitDialogBox("牌里藏着迷雾而非真相…不算明智的选择…",
                        transform.position + new Vector3(0.5f, 1f));
                }
                UIMgr.Instance().GetPanel<GamePanel>("GamePanel").StartFourSpeak();
                //TODO：结束小游戏
                //Inventory.Instance.items.FirstOrDefault(x => x.id == 201).GetComponent<FortuneTeller>().GetBack();
                UIMgr.Instance().HidePanel("LittleGamePanel");

            }
        });

    }


}
