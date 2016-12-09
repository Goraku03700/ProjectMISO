using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestUI : MonoBehaviour {

    private int i;
	// Use this for initialization
	void Start () {
        i = 0;
	}

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject.Find("PlayerIcon1P").GetComponent<PlayerIcon>().ChangeIconAngry();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject.Find("PlayerIcon1P").GetComponent<PlayerIcon>().ChangeIconNormal();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject.Find("PlayerIcon1P").GetComponent<PlayerIcon>().ChangeIconSad();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Find("Time").GetComponent<TimeCount>().StartTime();
        }
        */

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject.Find("PullArrow").GetComponent<PullArrow>().ChangeArrowLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GameObject.Find("PullArrow").GetComponent<PullArrow>().ChangeArrowRight();

        }
    }


}
