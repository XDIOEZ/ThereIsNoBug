using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CollectionSO/MapPointsSO")]
public class MapPointsSO : ScriptableObject
{
    [Header("dir的0为左，1为上，2为右，3为下。数字从小到大填，且来时的方向不用填")]
    public List<MapPoint> MapPoints = new List<MapPoint>();

}
