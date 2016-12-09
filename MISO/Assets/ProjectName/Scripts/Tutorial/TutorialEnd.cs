using UnityEngine;
using System.Collections;

public class TutorialEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {

        // スタートが押されたか
        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad1) || Input.GetKeyDown(KeyCode.Z))
        {
            GameObject.Find("Ready1").GetComponent<Ready>().DispReady();
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad2) || Input.GetKeyDown(KeyCode.X))
        {
            GameObject.Find("Ready2").GetComponent<Ready>().DispReady();
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad3) || Input.GetKeyDown(KeyCode.C))
        {
            GameObject.Find("Ready3").GetComponent<Ready>().DispReady();
        }

        if (MultiInput.GetButtonDown("Pause", MultiInput.JoypadNumber.Pad4) || Input.GetKeyDown(KeyCode.V))
        {
            GameObject.Find("Ready4").GetComponent<Ready>().DispReady();
        }


    }
}
