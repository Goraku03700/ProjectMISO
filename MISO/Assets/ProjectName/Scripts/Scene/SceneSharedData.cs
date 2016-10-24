using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// シーン間でデータを共有するためのオブジェクトです。
/// </summary>
/// <remarks>
/// シーン名とデータ名のペア(例:StageSelect.No)をキーとしてデータを保持しています。
/// </remarks>
public class SceneSharedData : SingletonMonoBehaviour<SceneSharedData>
{
    /// <summary>
    /// データをセットします
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="dataName">データ名</param>
    /// <param name="data">セットするデータ</param>
    public void Set(string sceneName, string dataName, object data)
    {
        string key = sceneName + '.' + dataName;

        m_dataDictionary[key] = data;
    }

    /// <summary>
    /// データをセットします
    /// </summary>
    /// <param name="key">キー名</param>
    /// <param name="data">セットするデータ</param>
    public void Set(string key, object data)
    {
        m_dataDictionary[key] = data;
    }

    /// <summary>
    /// データを取得します
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="dataName">データ名</param>
    /// <returns>
    /// 取得したデータ
    /// </returns>
    public object Get(string sceneName, string dataName)
    {
        string key = sceneName + '.' + dataName;

        return m_dataDictionary[key];
    }

    /// <summary>
    /// データの型を指定して取得します(ジェネリック版)
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="dataName">データ名</param>
    /// <typeparam name="T">取得するデータの型を指定してください。</typeparam>
    /// <returns>
    /// 取得したデータ
    /// </returns>
    public T Get<T>(string sceneName, string dataName)
    {
        string key = sceneName + '.' + dataName;

        return (T)m_dataDictionary[key];
    }

    /// <summary>
    /// データを取得します
    /// </summary>
    /// <param name="key">データ名</param>
    /// <returns>
    /// 取得したデータ
    /// </returns>
    public object Get(string key)
    {
        return m_dataDictionary[key];
    }

    /// <summary>
    /// データの型を指定してを取得します(ジェネリック版)
    /// </summary>
    /// <param name="key">データ名</param>
    /// <typeparam name="T">取得するデータの型を指定してください。</typeparam>
    /// <returns>
    /// 取得したデータ
    /// </returns>
    public T Get<T>(string key)
    {
        return (T)m_dataDictionary[key];
    }

    /// <summary>
    /// データを削除します
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    /// <param name="dataName">データ名</param>
    public void Remove(string sceneName, string dataName)
    {
        string key = sceneName + '.' + dataName;

        m_dataDictionary.Remove(key);
    }

    /// <summary>
    /// 不必要なデータを削除します
    /// </summary>
    /// <param name="key">データ名</param>
    public void Remove(string key)
    {
        m_dataDictionary.Remove(key);
    }

    /// <summary>
    /// Awakeメソッド
    /// </summary>
    void Awake()
    {
        bool isInitialize = base._InitializeSingleton();

        if(isInitialize)
        {
            // ロードで破棄しない
            DontDestroyOnLoad(this);

            // 初期化
            m_dataDictionary = new Dictionary<string, object>();
        }        
    }

    /// <summary>
    /// データを格納するDictionary(C++のmap)
    /// </summary>
    private Dictionary<string, object> m_dataDictionary;

    /// <summary>
    /// m_dataDictionaryに対するプロパティ
    /// </summary>
    public Dictionary<string, object> dataDictionary
    {
        get { return m_dataDictionary; }
    }

}
