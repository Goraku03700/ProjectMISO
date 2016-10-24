using UnityEngine;
using System.Collections;

/// <summary>
/// ジョイパッドの入力をenumで取得できるクラスです。
/// </summary>
/// <remarks>
/// UnityのInputクラスをラッピングしているだけなので使い方は同じです。
/// </remarks>
public class MultiInput
{
    /// <summary>
    /// ジョイパッドの番号です。
    /// </summary>
    public enum JoypadNumber
    {
        Pad1 = 0,
        Pad2 = 1,
        Pad3 = 2,
        Pad4 = 3,
    }

    /// <summary>
    /// 取得したいキーの名前です。
    /// </summary>
    public enum Key
    {
        Horizontal  = 0,
        Vertical    = 1,
        Attack      = 2,
    }

    /// <summary>
    /// staticコンストラクタ
    /// </summary>
    static MultiInput()
    {
        padNames = new string[]{
            "Player1 ",
            "Player2 ",
            "Player3 ",
            "Player4 "
        };

        keyNames = new string[]{
            "Horizontal",
            "Vertical",
            "Attack",
        };
    }

    /// <summary>
    /// キーの押下を取得
    /// </summary>
    /// <param name="keyName">取得するキーの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されているかを返す</returns>
    public static bool GetKey(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetKey(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// キーの押下を取得
    /// </summary>
    /// <param name="key">取得するキーのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されているかを返す</returns>
    public static bool GetKey(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetKey(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// キーの押下を取得
    /// </summary>
    /// <param name="keyName">取得するキーの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されたかを返す</returns>
    public static bool GetKeyDown(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetKeyDown(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// キーの押下を取得
    /// </summary>
    /// <param name="key">取得するキーのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されたかを返す</returns>
    public static bool GetKeyDown(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetKeyDown(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// キーが離されたかを取得
    /// </summary>
    /// <param name="keyName">取得するキーの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>キーが離された</returns>
    public static bool GetKeyUp(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetKeyUp(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// キーが離されたかを取得
    /// </summary>
    /// <param name="key">取得するキーのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>キーが離された</returns>
    public static bool GetKeyUp(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetKeyUp(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// 軸を取得
    /// </summary>
    /// <param name="keyName">取得するキーの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>軸の値を取得</returns>
    public static float GetAxis(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetAxis(keyName + padNames[(int)joypadNumber]);
    }

    /// <summary>
    /// 軸を取得
    /// </summary>
    /// <param name="key">取得するキーのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>軸の値を取得</returns>
    public static float GetAxis(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetAxis(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// パッド名配列
    /// </summary>
    private static string[] padNames;

    /// <summary>
    /// キー名配列
    /// </summary>
    private static string[] keyNames;
}
