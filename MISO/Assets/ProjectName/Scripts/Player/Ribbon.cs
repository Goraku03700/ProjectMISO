using UnityEngine;
using System;
using System.Collections;

namespace Ribbons
{
    public class Ribbon : MonoBehaviour
    {
        public void SizeAdjustUpdate()
        {
            // test code
            transform.Rotate(Vector3.up, 10.0f * Time.deltaTime, Space.World);
        }

        public void Throw(Vector3 position, Vector3 direction, float upPower, float speed)
        {
            m_animatorParameters.isThrow = true;

            transform.position = position;
            transform.forward = direction;

            m_speed = speed;

            m_rigidbody.AddForce(Vector3.up * upPower);
        }

        public void Move()
        {
            transform.Translate((transform.forward * m_speed) * Time.deltaTime);
        }

        public void ThrowUpdate()
        {
            m_animatorParameters.isGrounded = Physics.Raycast(
                transform.position,
                Vector3.down,
                m_raycastLength,
                m_raycastLayerMask);
        }

        public void Pull()
        {

        }

        public void PullUpdate()
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

        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();

            m_raycastLayerMask = LayerMask.NameToLayer("Stage");
        }

        private void _InitializeAnimatorParametersID()
        {
            int arraySize = Enum.GetValues(typeof(AnimatorParametersID)).Length;

            m_animatorParametersHashs = new int[arraySize];

            m_animatorParametersHashs[(int)AnimatorParametersID.IsThrow] = Animator.StringToHash("isThrow");
            m_animatorParametersHashs[(int)AnimatorParametersID.IsGrounded] = Animator.StringToHash("isGrounded");
            m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled] = Animator.StringToHash("isPulled");
        }

        private void _UpdateAnimatorParameters()
        {
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsThrow], m_animatorParameters.isThrow);
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsGrounded], m_animatorParameters.isGrounded);
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled], m_animatorParameters.isPulled);
        }

        private static int m_raycastLayerMask;

        private static float m_raycastLength;

        private float m_speed;

        private Animator m_animator;

        private Movable m_movable;

        private Rigidbody m_rigidbody;

        private AnimatorParameters m_animatorParameters;

        private int[] m_animatorParametersHashs;
    }

}