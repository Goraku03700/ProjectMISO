﻿using UnityEngine;
using System.Collections;

public class Ready : MonoBehaviour {

    [SerializeField]
    float m_speed;

    private bool m_isDisp;

	// Use this for initialization
	void Start () {

        m_isDisp = false;

        this.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

	}

    // Update is called once per frame
    void Update() {

        if (m_isDisp)
        {
            Vector3 objScale = this.transform.localScale;

            objScale.x += m_speed * Time.deltaTime;
            objScale.y += m_speed * Time.deltaTime;

            if(objScale.x > 1.0f)
            {
                objScale.x = 1.0f;
                objScale.y = 1.0f;
            }

            this.transform.localScale = objScale;
        }
	
	}

    public void DispReady()
    {
        m_isDisp = true;
    }
}

