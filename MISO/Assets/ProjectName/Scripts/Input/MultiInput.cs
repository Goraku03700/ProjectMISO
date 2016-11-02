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
    /// 取得したいボタンの名前です。
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
    /// ボタンの押下を取得
    /// </summary>
    /// <param name="keyName">取得するボタンの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されているかを返す</returns>
    public static bool GetButton(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetButton(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// ボタンの押下を取得
    /// </summary>
    /// <param name="key">取得するボタンのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されているかを返す</returns>
    public static bool GetButton(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetButton(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// ボタンの押下を取得
    /// </summary>
    /// <param name="keyName">取得するボタンの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されたかを返す</returns>
    public static bool GetButtonDown(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetButtonDown(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// ボタンの押下を取得
    /// </summary>
    /// <param name="key">取得するボタンのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>押されたかを返す</returns>
    public static bool GetButtonDown(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetButtonDown(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// ボタンが離されたかを取得
    /// </summary>
    /// <param name="keyName">取得するボタンの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>ボタンが離された</returns>
    public static bool GetButtonUp(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetButtonUp(padNames[(int)joypadNumber] + keyName);
    }

    /// <summary>
    /// ボタンが離されたかを取得
    /// </summary>
    /// <param name="key">取得するボタンのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>ボタンが離された</returns>
    public static bool GetButtonUp(Key key, JoypadNumber joypadNumber)
    {
        return Input.GetButtonUp(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    /// <summary>
    /// 軸を取得
    /// </summary>
    /// <param name="keyName">取得するボタンの名前</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>軸の値を取得</returns>
    public static float GetAxis(string keyName, JoypadNumber joypadNumber)
    {
        return Input.GetAxis(keyName + padNames[(int)joypadNumber]);
    }

    /// <summary>
    /// 軸を取得
    /// </summary>
    /// <param name="key">取得するボタンのenum</param>
    /// <param name="joypadNumber">取得するジョイパッドの番号</param>
    /// <returns>軸の値を取得</returns>
    public static float GetAxis(Key key, JoypadNumber joypadNumber)
    {
        string axisName = padNames[(int)joypadNumber] + keyNames[(int)key];

        return Input.GetAxis(axisName);     //Player1 Horizontal
    }

    /// <summary>
    /// パッド名配列
    /// </summary>
    private static string[] padNames;

    /// <summary>
    /// ボタン名配列
    /// </summary>
    private static string[] keyNames;
}
