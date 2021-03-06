﻿#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// InputManagerを自動的に複数Inputに設定してくれるクラス
/// </summary>
public class MultiInputManagerGenarator {


	/// <summary>
	/// インプットマネージャーを再設定します。
	/// </summary>
	[MenuItem("Input/ Generate MultiInput")]
	public static void ResetInputManager()
	{
		Debug.Log("インプットマネージャーの設定を開始します。");
		InputManagerGenerator inputManagerGenerator = new InputManagerGenerator();

		Debug.Log("設定を全てクリアします。");
		inputManagerGenerator.Clear();

		Debug.Log("プレイヤーごとの設定を追加します。");
		for(int i = 1; i <= 4; i++)
		{
			AddPlayerInputSettings(inputManagerGenerator, i);
		}
		
		Debug.Log("グローバル設定を追加します。");
		AddGlobalInputSettings(inputManagerGenerator);

		Debug.Log("インプットマネージャーの設定が完了しました。");
	}

	/// <summary>
	/// グローバルな入力設定を追加する（OK、キャンセルなど）
	/// </summary>
	/// <param name="inputManagerGenerator">Input manager generator.</param>
	private static void AddGlobalInputSettings(InputManagerGenerator inputManagerGenerator)
	{
		// 横方向
		{
			var name = "Horizontal";
			inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 1));
			inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "a", "d", "left", "right"));
		}

		// 縦方向
		{
			var name = "Vertical";
			inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, 0, 2));
			inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, "s", "w", "down", "up"));
		}

		// 決定
		{
			var name = "OK";
			inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "z", "joystick button 0"));
		}

		// キャンセル
		{
			var name = "Cancel";
			inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "x", "joystick button 1"));
		}

		// ポーズ
		{
			var name = "Pause";
			inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, "escape", "joystick button 7"));
		}
	}

    /// <summary>
    /// プレイヤーごとの入力設定を追加する
    /// </summary>
    /// <param name="inputManagerGenerator">Input manager generator.</param>
    /// <param name="playerIndex">Player index.</param>
    private static void AddPlayerInputSettings(InputManagerGenerator inputManagerGenerator, int playerIndex)
    {
        if (playerIndex < 1 || playerIndex > 4)
        {
            Debug.LogError("プレイヤーインデックスの値が不正です。");
        }

        string upKey = "", downKey = "", leftKey = "", rightKey = "", attackKey = "";
        GetAxisKey(out upKey, out downKey, out leftKey, out rightKey, out attackKey, playerIndex);

        int joystickNum = playerIndex;

        // 横方向
        {
            var name = string.Format("Player{0} Horizontal", playerIndex);
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, 1));
            //inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
        }

        // 縦方向
        {
            var name = string.Format("Player{0} Vertical", playerIndex);

            var padAxis = InputAxis.CreatePadAxis(name, joystickNum, 2);

            padAxis.invert = true;

            inputManagerGenerator.AddAxis(padAxis);
            //inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
        }

        // 横方向
        {
            var name = string.Format("Player{0} Horizontal2", playerIndex);
            inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, joystickNum, 4));
            //inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
        }

        // 縦方向
        {
            var name = string.Format("Player{0} Vertical2", playerIndex);

            var padAxis = InputAxis.CreatePadAxis(name, joystickNum, 5);

            padAxis.invert = true;

            inputManagerGenerator.AddAxis(padAxis);
            //inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
        }


        // 攻撃
        //{
        //    var axis = new InputAxis();
        //    var name = string.Format("Player{0} Attack", playerIndex);
        //    var button = string.Format("joystick {0} button 1", joystickNum);
        //    inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        //}


        // Cancel
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Cancel", playerIndex);
            var button = string.Format("joystick {0} button 2", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }

        // Item
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Item", playerIndex);
            var button = string.Format("joystick {0} button 3", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }

        // Throw
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Throw", playerIndex);
            var button = string.Format("joystick {0} button 0", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }

        //Hold
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Hold", playerIndex);
            var button = string.Format("joystick {0} button 1", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }

        // Dash
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Dash", playerIndex);
            var button = string.Format("joystick {0} button 5", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }

        // Pause　難市
        {
            var axis = new InputAxis();
            var name = string.Format("Player{0} Pause", playerIndex);
            var button = string.Format("joystick {0} button 7", joystickNum);
            inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, button, attackKey));
        }
    }

	/// <summary>
	/// キーボードでプレイした場合、割り当たっているキーを取得する
	/// </summary>
	/// <param name="upKey">Up key.</param>
	/// <param name="downKey">Down key.</param>
	/// <param name="leftKey">Left key.</param>
	/// <param name="rightKey">Right key.</param>
	/// <param name="attackKey">Attack key.</param>
	/// <param name="playerIndex">Player index.</param>
	private static void GetAxisKey(out string upKey, out string downKey, out string leftKey, out string rightKey, out string attackKey, int playerIndex)
	{
		upKey = "";
		downKey = "";
		leftKey = "";
		rightKey = "";
		attackKey = "";

		switch(playerIndex)
		{
		case 1:
			upKey = "w";
			downKey = "s";
			leftKey = "a";
			rightKey = "d";
			attackKey = "e";
			break;
		case 2:
			upKey = "i";
			downKey = "k";
			leftKey = "j";
			rightKey = "l";
			attackKey = "o";
			break;
		case 3:
			upKey = "up";
			downKey = "down";
			leftKey = "left";
			rightKey = "right";
			attackKey = "[0]";
			break;
		case 4:
			upKey = "[8]";
			downKey = "[5]";
			leftKey = "[4]";
			rightKey = "[6]";
			attackKey = "[9]";
			break;
		default:
			Debug.LogError("プレイヤーインデックスの値が不正です。");
			upKey = "";
			downKey = "";
			leftKey = "";
			rightKey = "";
			attackKey = "";
			break;
		}
	}
}

#endif