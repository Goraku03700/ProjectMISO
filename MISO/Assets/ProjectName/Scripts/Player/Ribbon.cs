using UnityEngine;
using System;
using System.Collections;

namespace Ribbons
{
    public class Ribbon : MonoBehaviour
    {
        public void SizeAdjustEnter()
        {
            rigidbody.useGravity = false;
        }

        public void SizeAdjustUpdate()
        {
            // test code
            transform.RotateAround(transform.parent.position, Vector3.up, 180.0f * Time.deltaTime);
        }

        public void Throw(Vector3 position, Quaternion rotation, float upPower, float speed)
        {
            m_animatorParameters.isThrow = true;

            transform.position  = position;
            //transform.forward   = direction;
            transform.rotation  = rotation;

            m_speed = speed;

            rigidbody.useGravity = true;

            rigidbody.AddForce(Vector3.up * upPower + transform.forward * speed);

            //UnityEditor.EditorApplication.isPaused = true;
        }

        public void Move()
        {
            transform.Translate((transform.forward * m_speed) * Time.deltaTime);
        }

        public void ThrowUpdate()
        {
            if(m_rigidbody.velocity.y <= 0.0f)
            {
                m_animatorParameters.isGrounded = Physics.Raycast(
                transform.position,
                Vector3.down,
                m_raycastLength,
                m_raycastLayerMask);

                if (m_animatorParameters.isGrounded)
                {
                    m_colliderObject.SetActive(true);
                    m_triggerColliderObject.SetActive(true);

                    m_playerCharacter.OnRibbonLanding();
                }
            }
        }

        public void StickUp()
        {
            if(m_rigidbody.velocity.y > 0.0f)
            {
                m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, .0f, m_rigidbody.velocity.z);
            }
        }

        public void Pull(Vector3 position, float power)
        {
            m_rigidbody.AddForce((position - transform.position) * power);
        }

        public void PullUpdate()
        {

        }

        public void Pulled()
        {
            m_animatorParameters.isPulled = true;

            foreach(var gameObject in m_triggerCollider.coughtObjects)
            {
                // todo layermask
                if(gameObject.layer == LayerMask.NameToLayer("PlayerCharacter"))
                {
                    gameObject.GetComponent<PlayerCharacter>().Collect();
                }

                if(gameObject.layer == LayerMask.NameToLayer("Girl"))
                {
                    
                }
            }

            GameObject.Destroy(this.gameObject);
        }

        //public void 

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
            rigidbody   = GetComponent<Rigidbody>();
            m_animator  = GetComponent<Animator>();

            m_raycastLayerMask = LayerMask.GetMask(new string[] { "Stage" });
            //m_raycastLayerMask = 1 << 15;

            Transform colliderTransform         = transform.FindChild("RibbonCollider");
            Transform triggerColliderTransform  = transform.FindChild("RibbonTriggerCollider");

            m_colliderObject        = colliderTransform.gameObject;
            m_triggerColliderObject = triggerColliderTransform.gameObject;

            m_colliderObject.SetActive(false);
            m_triggerColliderObject.SetActive(false);

            m_triggerCollider = m_triggerColliderObject.GetComponent<RibbonTriggerCollider>();

            m_playerCharacter = transform.parent.GetComponent<PlayerCharacter>();

            _InitializeAnimatorParametersID();
        }

        void Update()
        {
            _UpdateAnimatorParameters();
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
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsThrow], m_animatorParameters.isThrow);
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsGrounded], m_animatorParameters.isGrounded);
            m_animator.SetBool(m_animatorParametersHashs[(int)AnimatorParametersID.IsPulled], m_animatorParameters.isPulled);
        }

        private static int m_raycastLayerMask;

        [SerializeField]
        private float m_raycastLength;

        private float m_speed;

        private Animator m_animator;

        private Movable m_movable;

        private Rigidbody m_rigidbody;

        private GameObject m_colliderObject;

        private GameObject m_triggerColliderObject;

        private RibbonTriggerCollider m_triggerCollider;

        private PlayerCharacter m_playerCharacter;

        private AnimatorParameters m_animatorParameters;

        private int[] m_animatorParametersHashs;

        public Rigidbody rigidbody
        {
            get
            {
                return m_rigidbody;
            }

            set
            {
                m_rigidbody = value;
            }
        }
    }

}