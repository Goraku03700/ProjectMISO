using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RibbonEffect : MonoBehaviour {

    [SerializeField]
    public ParticleSystem m_ribbonParticle;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_ribbonParticle.isPlaying)
        {
            Destroy(this);
        }
    }
}
