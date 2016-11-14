using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestParticle : MonoBehaviour {

    ParticleSystem[] particle;
    int time;

    [SerializeField]
    private ParticleSystem idou;

    // Use this for initialization
    void Start () {

        particle = new ParticleSystem[30];
        for (int i = 0; i < 30; i++)
        {
            particle[i] = Instantiate(idou);
            particle[i].Stop();
        }

        time = 0;



	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Time.deltaTime * -2.0f, 0.0f, 0.0f);
            startParticle();
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Time.deltaTime * 2.0f, 0.0f, 0.0f);
            startParticle();
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, Time.deltaTime * 2.0f);
            startParticle();
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, Time.deltaTime * -2.0f);
            startParticle();
        }

    }

    private void startParticle()
    {
        for(int i=0; i<30; i++)
        {
            if(!particle[i].isPlaying)
            {
                particle[i].transform.position = this.transform.position;
                particle[i].Play();

                break;
            }
        }
    }
}
