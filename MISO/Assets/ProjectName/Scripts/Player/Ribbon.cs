using UnityEngine;
using System;
using System.Collections;

namespace Ribbons
{
    public class Ribbon : MonoBehaviour
    {
        public enum MoveDirectionState
        {
            Right,
            Left,
            None,
        }

        public void SizeAdjustEnter()
        {
            rigidbody.useGravity = false;
        }

        public void SizeAdjustUpdate()
        {
            transform.RotateAround(m_playerCharacter.transform.position, Vector3.up, 270.0f * Time.deltaTime);
        }

        public void Throw(Vector3 position, Quaternion rotation, float upPower, float speed)
        {
            m_animatorParameters.isThrow = true;

            transform.position      = position;
            //transform.forward   = direction;
            transform.rotation      = rotation;

            m_speed                 = speed;
            m_upPower               = upPower;

            rigidbody.useGravity    = true;
            m_isDoThrow             = true;

            m_wallColliderObject.SetActive(true);
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

                    playerCharacter.OnRibbonLanding();
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
            //m_triggerColliderObject.SetActive(false);

            //m_rigidbody.AddForce(direction.normalized * power);

            m_isDoPull = true;

            Vector3 direction = position - transform.position;

            direction.Normalize();

            m_pullForce = direction * power;
        }

        public void Shake(float horizontal)
        {
            //if(horizontal > 0)
            //{
            //    // right
            //    if(m_moveDirectionState == MoveDirectionState.Left)
            //    {
            //        _Penalty();
            //    }
            //}
            //else
            //{
            //    // left
            //    if (m_moveDirectionState == MoveDirectionState.Right)
            //    {
            //        _Penalty();
            //    }
            //}

            //m_isDoShake = true;
            m_shake = horizontal;

            if(Mathf.Abs(m_shake) > .0f &&
                m_animatorParameters.isGrounded)
            {
                //m_isDoShake = true;
            }

            if(m_animatorParameters.isGrounded)
            {
                m_isDoShake = true;

                //float angle = horizontal * m_playerCharacter.playerCharacterData.ribbonShakeSpeed;
                m_shakeAngle = horizontal * m_playerCharacter.playerCharacterData.ribbonShakeSpeed;

                //transform.RotateAround(playerCharacter.transform.position, transform.up, angle * Time.deltaTime);
            }
        }

        public void ViolentMove(Vector3 direction)
        {
            //float angle = Vector3.Angle(transform.forward, direction);

            //if(angle > 0)
            //{
            //    // right
            //    //m_moveDirectionState = MoveDirectionState.Right;

            //    if(m_moveDirectionState == MoveDirectionState.Left)
            //    {
            //        m_moveDirectionState = MoveDirectionState.None;
            //    }

            //    m_isDoViolentMove = true;
            //}
            //else
            //{
            //    // left
            //    //m_moveDirectionState = MoveDirectionState.Left;

            //    if(m_moveDirectionState == MoveDirectionState.Right)
            //    {
            //        m_moveDirectionState = MoveDirectionState.None;
            //    }

            //    m_isDoViolentMove = true;
            //}

            m_isDoViolentMove = true;

            float dot = Vector3.Dot(transform.forward, direction);

            Vector3 force = (transform.right * dot).normalized;

            m_rigidbody.AddForce(force * m_playerCharacter.playerCharacterData.ribbonViolentMoveSpeed, ForceMode.Force);
        }

        public void PullEnter()
        {
            //m_triggerColliderObject.SetActive(false);
        }

        public void PullUpdate()
        {
            float angle;

            angle = (m_moveDirectionState == MoveDirectionState.Right) ? 180.0f : -180.0f ;

            transform.RotateAround(playerCharacter.transform.position, transform.up, angle * Time.deltaTime);

            Vector3 direction;

            foreach (var coughtPlayerCharater in m_triggerCollider.coughtPlayerCharacters)
            {
                direction = transform.position - coughtPlayerCharater.transform.position;

                if (direction.magnitude > m_triggerCollider.collider.radius)
                {
                    coughtPlayerCharater.transform.position = transform.position + (direction * m_triggerCollider.collider.radius);
                }
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                direction = transform.position - girl.transform.position;

                if (direction.magnitude > m_triggerCollider.collider.radius)
                {
                    girl.transform.position = transform.position + (direction * m_triggerCollider.collider.radius);
                }
            }

        }

        public void Pulled()
        {
            m_animatorParameters.isPulled = true;

            foreach(var playerCharacter in m_triggerCollider.coughtPlayerCharacters)
            {
                playerCharacter.Collect();
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                girl.Collect(m_playerCharacter.transform.localPosition);

                m_playerCharacter.player.score++;
            }

            Destroy(gameObject);
        }

        public void Breake()
        {
            foreach (var playerCharacter in m_triggerCollider.coughtPlayerCharacters)
            {
                playerCharacter.CatchRelease();
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                girl.CatchRibbonRelease();
            }

            Destroy(gameObject);

            m_playerCharacter.BreakeRibbon();
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
            rigidbody   = GetComponent<Rigidbody>();
            m_animator  = GetComponent<Animator>();

            m_raycastLayerMask = LayerMask.GetMask(new string[] { "Stage" });
            //m_raycastLayerMask = 1 << 15;

            Transform colliderTransform         = transform.FindChild("RibbonCollider");
            Transform triggerColliderTransform  = transform.FindChild("RibbonTriggerCollider");
            Transform wallCollideTransform      = transform.FindChild("RibbonWallCollider");

            m_colliderObject        = colliderTransform.gameObject;
            m_triggerColliderObject = triggerColliderTransform.gameObject;
            m_wallColliderObject    = wallCollideTransform.gameObject;

            m_colliderObject.SetActive(false);
            m_triggerColliderObject.SetActive(false);

            m_triggerCollider = m_triggerColliderObject.GetComponent<RibbonTriggerCollider>();

            //playerCharacter = transform.parent.GetComponent<PlayerCharacter>();

            //GetComponent<HingeJoint>().connectedBody = m_playerCharacter.rigidbody;

            _InitializeAnimatorParametersID();
        }

        void Update()
        {
            _UpdateAnimatorParameters();
        }

        void FixedUpdate()
        {
            if(m_isDoThrow)
            {
                rigidbody.AddForce(Vector3.up * m_upPower + transform.forward * m_speed);

                m_isDoThrow = false;
            }

            if(m_isDoShake)
            {
                transform.RotateAround(playerCharacter.transform.position, transform.up, m_shakeAngle * Time.deltaTime);

                m_isDoShake = false;
            }

            if(m_isDoPull)
            {
                rigidbody.AddForce(m_pullForce, ForceMode.Force);

                m_isDoPull = false;
            }

            if(m_isDoShake)
            {
                m_rigidbody.AddForce((transform.right * m_shake) * m_playerCharacter.playerCharacterData.ribbonShakeSpeed, ForceMode.Force);

                m_isDoShake = false;
            }
        }

        private void _Penalty()
        {
            float penaltyTime = 0.0f;

            foreach (var playerCharacter in m_triggerCollider.coughtPlayerCharacters)
            {
                // penaltyTime = playerCharacter.AddPenaltyTime():

                if(penaltyTime > playerCharacter.playerCharacterData.ribbonPenaltyTime)
                {
                    Breake();
                }
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                // penaltyTime = girl.AddPenaltyTime();

                if (penaltyTime > playerCharacter.playerCharacterData.ribbonPenaltyTime)
                {
                    Breake();
                }
            }
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

        private Animator m_animator;

        private Movable m_movable;

        private Rigidbody m_rigidbody;

        private GameObject m_colliderObject;

        private GameObject m_triggerColliderObject;

        private GameObject m_wallColliderObject;

        private RibbonTriggerCollider m_triggerCollider;

        private float m_upPower;

        private float m_speed;

        private Vector3 m_pullForce;

        private float m_shakeAngle;

        private Vector3 m_violentMoveForce;

        private bool m_isDoThrow;

        private bool m_isDoPull;

        private bool m_isShake;

        private bool m_isDoViolentMove;

        private bool m_isDoShake;

        private float m_shake;

        private PlayerCharacter m_playerCharacter;

        public PlayerCharacter playerCharacter
        {
            get
            {
                return m_playerCharacter;
            }

            set
            {
                m_playerCharacter = value;
            }
        }

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

        private MoveDirectionState m_moveDirectionState;

        public MoveDirectionState moveDirectionState1
        {
            get
            {
                return m_moveDirectionState;
            }

            set
            {
                m_moveDirectionState = value;
            }
        }
    }

}