using UnityEngine;
using System.Collections;

public class DushTest : MonoBehaviour {

   
    private ParticleSystem[] m_particles;

    [SerializeField]
    private ParticleSystem m_particleRun;

    [SerializeField]
    private int m_particleMax; 

    // Use this for initialization
	void Start () {

        for(int i=0; i<m_particleMax; i++)
        {

        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
