using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlowerChange : MonoBehaviour
{
    [TextArea(2, 5)] public string Tip;
    //这是管理位于子对象上的花朵对象 他们的名字为 红 、黄 、蓝 、紫 、绿 、 青 ,橙 紫
    public List<GameObject> Flowers = new List<GameObject>();

    // 定义正确的花朵名称顺序
    [Header("默认为-红、橙、黄、绿、青、蓝、紫")]
    public string[] correctOrder = { "红", "橙", "黄", "绿", "青", "蓝", "紫" };

    [Header("花朵排列设置")]
    [Tooltip("花朵之间的间隔距离")]
    public float flowerSpacing = 2.0f;
    
    [Tooltip("第一个花朵的起始X坐标位置")]
    public float startOffsetX = 0.0f;

    [Header("游戏胜利后调用")]
    public UnityEvent OnWin;

    [Header("胜利条件设置")]
    [Tooltip("true表示按Y轴递增排序为胜利条件，false表示按颜色顺序排序为胜利条件")]
    public bool useYAxisOrderAsWinCondition = true;

    // TODO Start时按顺序获取子对象 填充到 Flowers 列表中
    void Start()
    {
        // 获取所有子对象并按顺序添加到Flowers列表中
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Flowers.Add(child.gameObject);
        }
        
        // TODO 随机打乱花朵位置（只修改位置，不修改列表顺序）
        ShuffleFlowerPositions();
    }

    // 随机打乱花朵位置（只修改位置，不修改列表顺序）
    void ShuffleFlowerPositions()
    {
        // 创建位置列表
        List<Vector3> positions = new List<Vector3>();
        
        // 获取所有花朵的当前位置
        for (int i = 0; i < Flowers.Count; i++)
        {
            positions.Add(Flowers[i].transform.position);
        }
        
        // 使用Fisher-Yates洗牌算法打乱位置列表
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }
        
        // 将打乱后的位置应用到花朵上（只修改x坐标，保持y,z不变）
        for (int i = 0; i < Flowers.Count; i++)
        {
            Vector3 newPosition = Flowers[i].transform.position;
            newPosition.x = positions[i].x;
            // 限制Y轴不能大于1，如果大于1则设为0
            if (newPosition.y > 1f)
            {
                newPosition.y = 0f;
            }
            Flowers[i].transform.position = newPosition;
        }
    }

    /// <summary>
    /// 检查花朵顺序是否正确
    /// 按照红、橙、黄、绿、青、蓝、紫的顺序检查
    /// </summary>
    /// <returns>如果顺序正确返回true，否则返回false</returns>
    public bool CheckFlowerOrder()
    {
        // 按x轴位置对花朵进行排序
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        
        // 检查排序后的花朵是否符合正确顺序
        for (int i = 0; i < sortedFlowers.Count && i < correctOrder.Length; i++)
        {
            if (sortedFlowers[i].name != correctOrder[i])
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// 检查花朵是否按Y轴递增排列（从左到右）
    /// </summary>
    /// <returns>如果Y轴按递增顺序排列返回true，否则返回false</returns>
    public bool CheckYAxisAscendingOrder()
    {
        // 按x轴位置对花朵进行排序
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        // 检查Y轴是否按递增顺序排列
        for (int i = 1; i < sortedFlowers.Count; i++)
        {
            // 如果任何一个花朵的Y轴小于或等于前一个花朵的Y轴，则不满足递增条件
            if (sortedFlowers[i].transform.position.y <= sortedFlowers[i - 1].transform.position.y)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 获取当前花朵顺序
    /// </summary>
    /// <returns>按x轴位置排序的花朵名称列表</returns>
    public List<string> GetCurrentFlowerOrder()
    {
        // 按x轴位置对花朵进行排序
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        
        // 提取花朵名称
        List<string> flowerNames = new List<string>();
        foreach (GameObject flower in sortedFlowers)
        {
            flowerNames.Add(flower.name);
        }
        
        return flowerNames;
    }

    /// <summary>
    /// 交换两个花朵的位置
    /// </summary>
    /// <param name="index1">第一个花朵的索引</param>
    /// <param name="index2">第二个花朵的索引</param>
    public void SwapFlowers(int index1, int index2)
    {
        if (index1 >= 0 && index1 < Flowers.Count && index2 >= 0 && index2 < Flowers.Count)
        {
            // 交换两个花朵的位置（只交换x坐标，保持y,z不变）
            Vector3 pos1 = Flowers[index1].transform.position;
            Vector3 pos2 = Flowers[index2].transform.position;
            
            Vector3 tempX = pos1;
            tempX.x = pos2.x;
            pos2.x = pos1.x;
            
            // 限制Y轴不能大于1，如果大于1则设为0
            if (tempX.y > 1f)
            {
                tempX.y = 0f;
            }
            if (pos2.y > 1f)
            {
                pos2.y = 0f;
            }
            
            Flowers[index1].transform.position = tempX;
            Flowers[index2].transform.position = pos2;
            
            // 每次交换后检查是否达成游戏胜利目标
            CheckWinCondition();
        }
    }
    
    /// <summary>
    /// 根据世界坐标位置交换花朵
    /// </summary>
    /// <param name="flower1">第一个花朵对象</param>
    /// <param name="flower2">第二个花朵对象</param>
    public void SwapFlowersByObject(GameObject flower1, GameObject flower2)
    {
        int index1 = Flowers.IndexOf(flower1);
        int index2 = Flowers.IndexOf(flower2);
        
        if (index1 != -1 && index2 != -1)
        {
            SwapFlowers(index1, index2);
        }
    }
    
    /// <summary>
    /// 根据世界坐标位置交换花朵（保持各自Y轴位置）
    /// </summary>
    /// <param name="flower1">第一个花朵对象</param>
    /// <param name="flower2">第二个花朵对象</param>
    /// <param name="originalPosition">第一个花朵的原始位置</param>
    public void SwapFlowersByObjectWithYFixed(GameObject flower1, GameObject flower2, Vector3 originalPosition)
    {
        if (flower1 == null || flower2 == null)
            return;

        // 保持各自的Y坐标，只交换X坐标
        Vector3 flower1TargetPosition = new Vector3(flower2.transform.position.x, flower1.transform.position.y, flower1.transform.position.z);
        Vector3 flower2TargetPosition = new Vector3(originalPosition.x, flower2.transform.position.y, flower2.transform.position.z);
        
        // 限制Y轴不能大于1，如果大于1则设为0
        if (flower1TargetPosition.y > 1f)
        {
            flower1TargetPosition.y = 0f;
        }
        if (flower2TargetPosition.y > 1f)
        {
            flower2TargetPosition.y = 0f;
        }
        
        // 使用DoTween执行动画
        flower1.transform.DOMove(flower1TargetPosition, 0.3f).OnComplete(() => CheckWinCondition());
        flower2.transform.DOMove(flower2TargetPosition, 0.3f).OnComplete(() => CheckWinCondition());
    }
    
    /// <summary>
    /// 检查游戏胜利条件并在达成时触发胜利逻辑
    /// </summary>
    private void CheckWinCondition()
    {
        bool isWin = false;
        
        // 根据useYAxisOrderAsWinCondition的值选择不同的胜利条件
        if (useYAxisOrderAsWinCondition)
        {
            // 按Y轴递增排序为胜利条件
            isWin = CheckYAxisAscendingOrder();
            if (isWin)
            {
                Debug.Log("恭喜！花朵按Y轴递增顺序排列，游戏胜利！");
            }
        }
        else
        {
            // 按颜色顺序排序为胜利条件
            isWin = CheckFlowerOrder();
            if (isWin)
            {
                Debug.Log("恭喜！花朵按颜色顺序排列，游戏胜利！");
            }
        }
        
        // 如果达成胜利条件，则触发胜利逻辑
        if (isWin)
        {
            OnGameWin();
        }
    }
    
    /// <summary>
    /// 游戏胜利时调用的方法
    /// </summary>
    private void OnGameWin()
    {
        // 示例：将管理器上的Sprite Renderer设置为绿色
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.green;
        }
        OnWin.Invoke();
        // 可以在这里添加其他胜利逻辑
        // 例如禁用花朵拖拽、显示胜利界面等
    }
    
    /// <summary>
    /// 根据索引获取花朵对象
    /// </summary>
    /// <param name="index">花朵索引</param>
    /// <returns>花朵对象</returns>
    public GameObject GetFlower(int index)
    {
        if (index >= 0 && index < Flowers.Count)
        {
            return Flowers[index];
        }
        return null;
    }
    
    /// <summary>
    /// 获取花朵数量
    /// </summary>
    /// <returns>花朵数量</returns>
    public int GetFlowerCount()
    {
        return Flowers.Count;
    }
}