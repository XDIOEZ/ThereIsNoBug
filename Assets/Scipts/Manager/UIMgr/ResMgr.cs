using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托和 lambda表达式
/// 3.协程
/// 4.泛型
/// </summary>
public class ResMgr : BaseMgrNoMono<ResMgr>
{
    //同步加载资源
    public T Load<T>(string name,UnityAction<T> callback = null) where T:Object
    {
        T res = Resources.Load<T>(name);
        //如果对象是一个GameObject类型的 我把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
        callback?.Invoke(res);
    }

    #region 异步加载

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback">生成完后用资源做什么，参数是这个物体，就算是在没什么好干的也可以添加一个空函数</param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T:Object
    {
        //开启异步加载的协程
        MonoMgr.Instance().StartCoroutine(ReallyLoadAsync(name, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;
    
        // if (r.asset is GameObject)
        //     callback(GameObject.Instantiate(r.asset) as T);
        // else
        //     callback(r.asset as T);
        T result = null;
        if (r.asset is GameObject)
            result = GameObject.Instantiate(r.asset) as T;
        else
            result = r.asset as T;
    
        if (result == null)
            Debug.LogError($"资源加载失败: {name}");
    
        callback?.Invoke(result);
    }

    #endregion
    
}
