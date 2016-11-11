using UnityEngine;
using System.Collections;

public class particle_sample : MonoBehaviour {

    [SerializeField]
    private ParticleSystem particle;
    private girl_sample girl;

	// Use this for initialization
	void Start () {
        girl = GameObject.Find("Cube").GetComponent<girl_sample>();
        particle = GameObject.Find("Particle_sample").GetComponent<ParticleSystem>();
        particle.Stop();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.A))
        {
            particle.Play();
            this.transform.eulerAngles = new Vector3(0f, -90f, 0f);
            this.transform.position = girl.transform.position;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            particle.Play();
            this.transform.eulerAngles = new Vector3(0f, 90f, 0f);
            this.transform.position = girl.transform.position;
        }
        if (Input.GetKey(KeyCode.W))
        {
            particle.Play();
            this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            this.transform.position = girl.transform.position;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            particle.Play();
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            this.transform.position = girl.transform.position;
        }
        else
        {
            particle.Stop();
        }

        
	}

    

}
