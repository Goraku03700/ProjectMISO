using UnityEngine;
using System.Collections;
using XInputDotNetPure;


public class Ending : MonoBehaviour {


	// Update is called once per frame
	void Update () {

        XInputDotNetPure.GamePadState pad1 = GamePad.GetState(PlayerIndex.One);
        XInputDotNetPure.GamePadState pad2 = GamePad.GetState(PlayerIndex.Two);
        XInputDotNetPure.GamePadState pad3 = GamePad.GetState(PlayerIndex.Three);
        XInputDotNetPure.GamePadState pad4 = GamePad.GetState(PlayerIndex.Four);
        
        // スタートが押されたら遷移
        if ((pad1.Buttons.Start == ButtonState.Pressed) ||
            (pad2.Buttons.Start == ButtonState.Pressed) ||
             (pad3.Buttons.Start == ButtonState.Pressed) ||
              (pad4.Buttons.Start == ButtonState.Pressed) ||
               (Input.GetKeyDown(KeyCode.A)))
        {
            Fade.ChangeScene("Title");
        }



    }
}
