using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpotLighter : MonoBehaviour {

    [SerializeField]
    Transform m_targetTransform;

    AnimatorStateInfo   m_animatorStateInfo;


    [SerializeField]
    Animator    m_animator;

    Light m_light;

    List<Vector3> m_vectorBuffer = new List<Vector3>();
	// Use this for initialization
	void Start () {
        for(int i = 0; i < 4 ; ++i)
        {
            m_vectorBuffer.Add(m_targetTransform.position);
        }
        m_light = this.gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        m_animatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(0);

        this.transform.position = m_vectorBuffer[0] + Vector3.up * 15.5f;

        if (m_animatorStateInfo.fullPathHash == Animator.StringToHash("Base Layer.CaughtRibbon.InBuilding"))
        {
            m_light.enabled = false;
        }
        else
        {
            m_light.enabled = true;
        }


        m_vectorBuffer.RemoveAt(0);
        m_vectorBuffer.Add(m_targetTransform.position);
	}
}
