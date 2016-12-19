using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestUI : MonoBehaviour {

    private int i;

    private GameObject stick;

    private float rot;
	// Use this for initialization
	void Start () {
        i = 0;

        stick = GameObject.Find("Stick");

        rot = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            /*stick.transform.localRotation = Quaternion.Euler(stick.transform.localRotation.x,
                                                             stick.transform.localRotation.y,
                                                             stick.transform.localRotation.z - 0.1f);*/
            //rot += 200.0f * Time.deltaTime;

            GameObject.Find("PullStick").GetComponent<PullStick>().ChangeStickLeft();
            
        }
        
        if(Input.GetKey(KeyCode.RightArrow))
        {
            /*
            stick.transform.localRotation = Quaternion.Euler(stick.transform.localRotation.x,
                                                             stick.transform.localRotation.y,
                                                             stick.transform.localRotation.z + 0.1f);
                                                                */
            //rot -= 200.0f * Time.deltaTime;

            GameObject.Find("PullStick").GetComponent<PullStick>().ChangeStickRight();

        }


    }


}
