﻿using UnityEngine;
using System.Collections;

public class TimeNiddle : MonoBehaviour {

    //[SerializeField]
    private float m_time;

    private float m_nowTime;

    private GameObject obj;

    private bool isStart;

    private float rotateFrame;
	// Use this for initialization
	void Start () {

        GameObject timeObject = GameObject.Find("Time");

        m_time = timeObject.GetComponent<TimeCount>().GetTime();

        rotateFrame = m_time * 30 / 360 ;

        m_nowTime = 0.0f;

        isStart = false;



	
	}

    // Update is called once per frame
    void Update(){

        if (isStart)
        {
            m_nowTime += Time.deltaTime;
        }

        // 現在の角度を求める
        float rot_z = m_nowTime / m_time * 360;

        // 回転
        this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -rot_z + 90.0f);

    }

    public void StartRotate()
    {
        isStart = true;
    }
}


//
//
