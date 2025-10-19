using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlowerChange : MonoBehaviour
{
    //���ǹ���λ���Ӷ����ϵĻ������ ���ǵ�����Ϊ �� ���� ���� ���� ���� �� �� ,�� ��
    public List<GameObject> Flowers = new List<GameObject>();

    [Header("������������")]
    [Tooltip("����֮��ļ������")]
    public float flowerSpacing = 2.0f;
    
    [Tooltip("��һ���������ʼX����λ��")]
    public float startOffsetX = 0.0f;

    [Header("��Ϸʤ�������")]
    public UnityEvent unityEvent;

    [Header("ʤ����������")]
    [Tooltip("true��ʾ��Y���������Ϊʤ��������false��ʾ����ɫ˳������Ϊʤ������")]
    public bool useYAxisOrderAsWinCondition = true;

    // TODO Startʱ��˳���ȡ�Ӷ��� ��䵽 Flowers �б���
    void Start()
    {
        // ��ȡ�����Ӷ��󲢰�˳����ӵ�Flowers�б���
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Flowers.Add(child.gameObject);
        }
        
        // TODO ������һ���λ�ã�ֻ�޸�λ�ã����޸��б�˳��
        ShuffleFlowerPositions();
    }

    // ������һ���λ�ã�ֻ�޸�λ�ã����޸��б�˳��
    void ShuffleFlowerPositions()
    {
        // ����λ���б�
        List<Vector3> positions = new List<Vector3>();
        
        // ��ȡ���л���ĵ�ǰλ��
        for (int i = 0; i < Flowers.Count; i++)
        {
            positions.Add(Flowers[i].transform.position);
        }
        
        // ʹ��Fisher-Yatesϴ���㷨����λ���б�
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }
        
        // �����Һ��λ��Ӧ�õ������ϣ�ֻ�޸�x���꣬����y,z���䣩
        for (int i = 0; i < Flowers.Count; i++)
        {
            Vector3 newPosition = Flowers[i].transform.position;
            newPosition.x = positions[i].x;
            // ����Y�᲻�ܴ���1���������1����Ϊ0
            if (newPosition.y > 1f)
            {
                newPosition.y = 0f;
            }
            Flowers[i].transform.position = newPosition;
        }
    }

    /// <summary>
    /// ��黨��˳���Ƿ���ȷ
    /// ���պ졢�ȡ��ơ��̡��ࡢ�����ϵ�˳����
    /// </summary>
    /// <returns>���˳����ȷ����true�����򷵻�false</returns>
    public bool CheckFlowerOrder()
    {
        // ������ȷ�Ļ�������˳��
        string[] correctOrder = { "��", "��", "��", "��", "��", "��", "��" };
        
        // ��x��λ�öԻ����������
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        
        // ��������Ļ����Ƿ������ȷ˳��
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
    /// ��黨���Ƿ�Y��������У������ң�
    /// </summary>
    /// <returns>���Y�ᰴ����˳�����з���true�����򷵻�false</returns>
    public bool CheckYAxisAscendingOrder()
    {
        // ��x��λ�öԻ����������
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        // ���Y���Ƿ񰴵���˳������
        for (int i = 1; i < sortedFlowers.Count; i++)
        {
            // ����κ�һ�������Y��С�ڻ����ǰһ�������Y�ᣬ�������������
            if (sortedFlowers[i].transform.position.y <= sortedFlowers[i - 1].transform.position.y)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ��ȡ��ǰ����˳��
    /// </summary>
    /// <returns>��x��λ������Ļ��������б�</returns>
    public List<string> GetCurrentFlowerOrder()
    {
        // ��x��λ�öԻ����������
        List<GameObject> sortedFlowers = new List<GameObject>(Flowers);
        sortedFlowers.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
        
        // ��ȡ��������
        List<string> flowerNames = new List<string>();
        foreach (GameObject flower in sortedFlowers)
        {
            flowerNames.Add(flower.name);
        }
        
        return flowerNames;
    }

    /// <summary>
    /// �������������λ��
    /// </summary>
    /// <param name="index1">��һ�����������</param>
    /// <param name="index2">�ڶ������������</param>
    public void SwapFlowers(int index1, int index2)
    {
        if (index1 >= 0 && index1 < Flowers.Count && index2 >= 0 && index2 < Flowers.Count)
        {
            // �������������λ�ã�ֻ����x���꣬����y,z���䣩
            Vector3 pos1 = Flowers[index1].transform.position;
            Vector3 pos2 = Flowers[index2].transform.position;
            
            Vector3 tempX = pos1;
            tempX.x = pos2.x;
            pos2.x = pos1.x;
            
            // ����Y�᲻�ܴ���1���������1����Ϊ0
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
            
            // ÿ�ν��������Ƿ�����Ϸʤ��Ŀ��
            CheckWinCondition();
        }
    }
    
    /// <summary>
    /// ������������λ�ý�������
    /// </summary>
    /// <param name="flower1">��һ���������</param>
    /// <param name="flower2">�ڶ����������</param>
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
    /// ������������λ�ý������䣨���ָ���Y��λ�ã�
    /// </summary>
    /// <param name="flower1">��һ���������</param>
    /// <param name="flower2">�ڶ����������</param>
    /// <param name="originalPosition">��һ�������ԭʼλ��</param>
    public void SwapFlowersByObjectWithYFixed(GameObject flower1, GameObject flower2, Vector3 originalPosition)
    {
        if (flower1 == null || flower2 == null)
            return;

        // ���ָ��Ե�Y���ֻ꣬����X����
        Vector3 flower1TargetPosition = new Vector3(flower2.transform.position.x, flower1.transform.position.y, flower1.transform.position.z);
        Vector3 flower2TargetPosition = new Vector3(originalPosition.x, flower2.transform.position.y, flower2.transform.position.z);
        
        // ����Y�᲻�ܴ���1���������1����Ϊ0
        if (flower1TargetPosition.y > 1f)
        {
            flower1TargetPosition.y = 0f;
        }
        if (flower2TargetPosition.y > 1f)
        {
            flower2TargetPosition.y = 0f;
        }
        
        // ʹ��DoTweenִ�ж���
        flower1.transform.DOMove(flower1TargetPosition, 0.3f).OnComplete(() => CheckWinCondition());
        flower2.transform.DOMove(flower2TargetPosition, 0.3f).OnComplete(() => CheckWinCondition());
    }
    
    /// <summary>
    /// �����Ϸʤ���������ڴ��ʱ����ʤ���߼�
    /// </summary>
    private void CheckWinCondition()
    {
        bool isWin = false;
        
        // ����useYAxisOrderAsWinCondition��ֵѡ��ͬ��ʤ������
        if (useYAxisOrderAsWinCondition)
        {
            // ��Y���������Ϊʤ������
            isWin = CheckYAxisAscendingOrder();
            if (isWin)
            {
                Debug.Log("��ϲ�����䰴Y�����˳�����У���Ϸʤ����");
            }
        }
        else
        {
            // ����ɫ˳������Ϊʤ������
            isWin = CheckFlowerOrder();
            if (isWin)
            {
                Debug.Log("��ϲ�����䰴��ɫ˳�����У���Ϸʤ����");
            }
        }
        
        // ������ʤ���������򴥷�ʤ���߼�
        if (isWin)
        {
            OnGameWin();
        }
    }
    
    /// <summary>
    /// ��Ϸʤ��ʱ���õķ���
    /// </summary>
    private void OnGameWin()
    {
        // ʾ�������������ϵ�Sprite Renderer����Ϊ��ɫ
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.color = Color.green;
        }
        unityEvent.Invoke();
        // �����������������ʤ���߼�
        // ������û�����ק����ʾʤ�������
    }
    
    /// <summary>
    /// ����������ȡ�������
    /// </summary>
    /// <param name="index">��������</param>
    /// <returns>�������</returns>
    public GameObject GetFlower(int index)
    {
        if (index >= 0 && index < Flowers.Count)
        {
            return Flowers[index];
        }
        return null;
    }
    
    /// <summary>
    /// ��ȡ��������
    /// </summary>
    /// <returns>��������</returns>
    public int GetFlowerCount()
    {
        return Flowers.Count;
    }
}