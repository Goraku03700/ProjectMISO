using UnityEngine;
using System;
using System.Collections;

public class Ribbon : MonoBehaviour {

    public void Charge()
    {
        
    }

    public void Throw()
    {

    }

    private enum AnimatorParametersID
    {
        IsThrow = 0,
        IsGrounded,
        IsPulled,
    }

    private struct AnimatorParameters
    {
        public bool isThrow;
        public bool isGrounded;
        public bool isPulled;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void _InitializeAnimatorParametersID()
    {
        int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

        m_animatorParametersHashs = new int[arraySize];

        m_animatorParametersHashs[(int)AnimatorParametersID.IsThrow]    = Animator.StringToHash("isThrow");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsGrounded] = Animator.StringToHash("isGrounded");
        m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled]   = Animator.StringToHash("isPulled");
    }

    private void _UpdateAnimatorParameters()
    {
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsThrow],    m_animatorParameters.isThrow);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsGrounded], m_animatorParameters.isGrounded);
        m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled],   m_animatorParameters.isPulled);
    }

    private bool m_isThrow;

    private Animator m_animator;

    private Movable m_movable;

    private AnimatorParameters m_animatorParameters;

    private int[] m_animatorParametersHashs;
}
