using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        
        File.WriteAllText(path, json);

        try
        {

        }
        catch (System.Exception exception)
        {
            Debug.LogError($"Failed to save data to {path}\n{exception}");
        }
        
    }
    
    
    public static T LoadByJson<T>(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Save file not found in {path}");
            return default;
        }

        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;
        }
        catch (System.Exception exception)
        {
            Debug.LogError($"Failed to load data from {path}\n{exception}");
            return default;
        }
    }
    
    
    
    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Path.Combine(Application.persistentDataPath, saveFileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    
    
    
    
    
}
