using UnityEngine;
using System.Collections;

/// <summary>
/// シングルトンオブジェクト用クラス
/// </summary>
/// <remarks>
/// 継承先Awakeメソッドで_InitializeSingletonを呼び出してください。
/// 多重に同じオブジェクトが生成された場合新しいほうが破棄されます。
/// ロードで破棄されるため破棄させたくない場合は継承先でDontDestroyOnLoadを使用してください。
/// </remarks>
/// <typeparam name="T">継承先クラスを指定してください。</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    /// <summary>
    /// シングルトンを初期化します。
    /// </summary>
    /// <returns>
    /// true    : 初期化
    /// false   : オブジェクトが既に存在している
    /// </returns>
    protected bool _InitializeSingleton()
    {
        if(m_instance != null)
        {
            Destroy(gameObject);

            return false;
        }
        else
        {
            m_instance = (T)this;

            return true;
        }
    }

    /// <summary>
    /// 継承先のインスタンス
    /// </summary>
    protected static T m_instance = null;

    /// <summary>
    /// シングルトンを取得するプロパティ
    /// </summary>
    /// <returns>
    /// 生成されてない場合nullが返ります。
    /// </returns>
    public static T instance
    {
        get { return m_instance; }
    }
}