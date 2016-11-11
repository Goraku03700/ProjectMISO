using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DushParticle : MonoBehaviour
{
    const float speed = 0.05f;

    ParticleSystem[] particle;
    int time;

    [SerializeField]
    private ParticleSystem idou;

    // Use this for initialization
    void Start()
    {

        particle = new ParticleSystem[30];
        for (int i = 0; i < 30; i++)
        {
            particle[i] = Instantiate(idou);
            //particle[i].Stop();
        }

        time = 0;



    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.A))
        {
            //this.transform.Translate(Time.deltaTime * -1.0f, 0.0f, 0.0f);
            this.transform.Translate(-speed, 0.0f, 0.0f);
            time++;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //this.transform.Translate(Time.deltaTime * 1.0f, 0.0f, 0.0f);
            this.transform.Translate(speed, 0.0f, 0.0f);
            time++;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(0.0f, 0.0f, speed);
            time++;
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(0.0f, 0.0f, -speed);
            time++;
        }

        if(time > 1)
        {
            startParticle();
            time = 0;
        }

    }

    private void startParticle()
    {
        for (int i = 0; i < 30; i++)
        {
            if (!particle[i].isPlaying)
            {
                particle[i].transform.position = this.transform.position;
                particle[i].Play();

                break;
            }
        }
    }
}
