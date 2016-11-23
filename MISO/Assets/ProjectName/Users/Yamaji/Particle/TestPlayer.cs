using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPlayer : MonoBehaviour
{

    [SerializeField]
    private float m_speed;

    // Use this for initialization
    void Start()
    {

     




    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<DushTest>().stopParticle();

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Time.deltaTime * -m_speed, 0.0f, 0.0f);
            this.GetComponent<DushTest>().startParticle();
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Time.deltaTime * m_speed, 0.0f, 0.0f);
            this.GetComponent<DushTest>().startParticle();
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, Time.deltaTime * m_speed);
            this.GetComponent<DushTest>().startParticle();
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, Time.deltaTime * -m_speed);
            this.GetComponent<DushTest>().startParticle();
        }
    }

    
}

