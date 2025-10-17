using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scene/Game Scene Registry")]
public class GameSceneRegistrySO : ScriptableObject
{
    [Tooltip("把项目中所有可加载的 GameSceneSO 都拖进来")]
    public List<GameSceneSO> scenes = new List<GameSceneSO>();

    private Dictionary<string, GameSceneSO> _guidMap;

    private void OnEnable()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        _guidMap = new Dictionary<string, GameSceneSO>();
        foreach (var s in scenes)
        {
            if (s == null || s.sceneReference == null) continue;

            // 使用 Addressables 的 AssetGUID 作为键
            var guid = s.sceneReference.AssetGUID;
            if (string.IsNullOrEmpty(guid)) continue;

            _guidMap[guid] = s;
        }
    }

    public GameSceneSO GetByAddressableGuid(string guid)
    {
        if (string.IsNullOrEmpty(guid)) return null;
        if (_guidMap == null || _guidMap.Count == 0) Rebuild();

        return _guidMap.TryGetValue(guid, out var so) ? so : null;
    }
}
