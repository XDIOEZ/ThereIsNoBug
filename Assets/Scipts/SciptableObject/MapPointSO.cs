using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CollectionSO/MapPointsSO")]
public class MapPointsSO : ScriptableObject
{
    [Header("dir��0Ϊ��1Ϊ�ϣ�2Ϊ�ң�3Ϊ�¡����ִ�С���������ʱ�ķ�������")]
    public List<MapPoint> MapPoints = new List<MapPoint>();

}
