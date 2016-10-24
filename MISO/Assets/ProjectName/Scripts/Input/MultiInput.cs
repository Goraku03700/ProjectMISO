using UnityEngine;
using System.Collections;

public class MultiInput
{
    public enum JoypadNumber
    {
        Pad1 = 0,
        Pad2 = 1,
        Pad3 = 2,
        Pad4 = 3,
    }

    public enum Key
    {
        Horizontal  = 0,
        Vertical    = 1,
        Attack      = 2,
    }

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

    public static void GetKey(string keyName, JoypadNumber joypadNumber)
    {
        Input.GetKey(padNames[(int)joypadNumber] + keyName);
    }

    public static void GetKey(Key key, JoypadNumber joypadNumber)
    {
        Input.GetKey(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    public static void GetKeyDown(string keyName, JoypadNumber joypadNumber)
    {
        Input.GetKeyDown(padNames[(int)joypadNumber] + keyName);
    }

    public static void GetKeyDown(Key key, JoypadNumber joypadNumber)
    {
        Input.GetKeyDown(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    public static void GetKeyUp(string keyName, JoypadNumber joypadNumber)
    {
        Input.GetKeyUp(padNames[(int)joypadNumber] + keyName);
    }

    public static void GetKeyUp(Key key, JoypadNumber joypadNumber)
    {
        Input.GetKeyUp(padNames[(int)joypadNumber] + keyNames[(int)key]);
    }

    private static string[] padNames;
    private static string[] keyNames;
}
