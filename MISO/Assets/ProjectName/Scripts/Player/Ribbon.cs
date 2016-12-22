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
        }

        public void SizeAdjustEnter()
        {
            rigidbody.useGravity = false;

            //m_ribbonLine.gameObject.SetActive(true);

            transform.position += (Vector3.forward * 5.0f) + Vector3.up;
        }

        public void SizeAdjustUpdate()
        {
            //Quaternion q = transform.rotation;

            transform.RotateAround(m_playerCharacter.transform.position, Vector3.up, -270.0f * Time.deltaTime);
            //transform.rotation = q;

            //Vector3 position = m_playerCharacter.transform.position;

            //position.y = transform.position.y;

            //transform.LookAt(position);
        }

        public void SizeAdjustExit()
        {
            rigidbody.useGravity = true;

            //m_ribbonLine.gameObject.SetActive(true);
        }

        public void Throw(Vector3 position, Quaternion rotation, float upPower, float speed)
        {
            m_animatorParameters.isThrow    = true;
            rigidbody.useGravity            = true;

            //transform.position      = position;
            //transform.forward     = direction;
            //transform.rotation      = rotation;
            m_throwPosition         = position;
            m_throwRotation         = rotation;

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
                    //m_pullArrow.spriteRenderer.enabled = true;

                    playerCharacter.OnRibbonLanding();

                    //m_rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
                }
            }
        }

        public void StickUp()
        {
            if(m_rigidbody.velocity.y > 0.0f)
            {
                //m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, .0f, m_rigidbody.velocity.z);
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

            //if(Mathf.Abs(m_shake) > .0f &&
            //    m_animatorParameters.isGrounded)
            //{
            //    //m_isDoShake = true;
            //}

            if(m_animatorParameters.isGrounded)
            {
                m_isDoShake = true;

                //float angle = horizontal * m_playerCharacter.playerCharacterData.ribbonShakeSpeed;
                m_shakeAngle = horizontal * m_playerCharacter.playerCharacterData.ribbonShakeSpeed;

                //transform.RotateAround(playerCharacter.transform.position, transform.up, angle * Time.deltaTime);

                //Vector3 playerDirection = m_playerCharacter.transform.position - transform.position;

                //var direction = ((m_playerCharacter.transform.right * playerDirection.magnitude) - rigidbody.position).normalized;
                //var rotation = Quaternion.AngleAxis(horizontal * 60, Vector3.up) * direction * Mathf.Abs(horizontal);
                //var desired = rotation + m_shake * direction;
                //var change = desired * m_playerCharacter.playerCharacterData.ribbonShakeSpeed - rigidbody.velocity;
                //// Debug lines: Red - current heading
                ////              Blue - applied heading
                //Debug.DrawLine(rigidbody.position, rigidbody.position + rigidbody.velocity, Color.red);
                //Debug.DrawLine(rigidbody.position, rigidbody.position + change, Color.blue);
                //rigidbody.AddForce(change * Time.deltaTime, ForceMode.VelocityChange);

                //Quaternion q = Quaternion.AngleAxis(m_shakeAngle * Time.deltaTime, Vector3.up);

                //m_rigidbody.MovePosition(q * (transform.position - m_playerCharacter.transform.position) + m_playerCharacter.transform.position);
                ////m_rigidbody.MovePosition(q * (m_playerCharacter.transform.position - transform.position) + transform.position);
                ////m_rigidbody.MoveRotation(transform.rotation * q);

                //m_rigidbody.velocity = q * m_rigidbody.velocity;
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

            if(dot < 0)
            {
                m_moveDirectionState = MoveDirectionState.Right;

                m_pullStick.ChangeStickLeft();
            }
            else
            {
                m_moveDirectionState = MoveDirectionState.Left;

                m_pullStick.ChangeStickRight();
            }

            Vector3 force = (transform.right * dot).normalized;

            m_rigidbody.AddForce(force * m_playerCharacter.playerCharacterData.ribbonViolentMoveSpeed, ForceMode.Force);
        }

        public void PullEnter()
        {
            //m_triggerColliderObject.SetActive(false);

            //m_pullArrowGameObject.SetActive(true);
        }

        public void PullUpdate()
        {
            //float angle;

            //angle = (m_moveDirectionState == MoveDirectionState.Right) ? 180.0f : -180.0f ;

            //transform.RotateAround(playerCharacter.transform.position, transform.up, angle * Time.deltaTime);

            int objectsCount = m_triggerCollider.coughtGirls.Count + m_triggerCollider.coughtPlayerCharacters.Count;

            if (objectsCount > 0)
            {
                //m_pullStick.spriteRenderer.enabled = true;
                m_pullStick.gameObject.SetActive(true);
            }
            else
            {
                //m_pullStick.spriteRenderer.enabled = false;
                m_pullStick.gameObject.SetActive(false);
            }

            Vector3 direction;

            foreach (var coughtPlayerCharater in m_triggerCollider.coughtPlayerCharacters)
            {
                direction = transform.position - coughtPlayerCharater.transform.position;

                if (direction.magnitude > m_triggerCollider.collider.radius * 4)
                {
                    coughtPlayerCharater.transform.position = transform.position + (direction.normalized * m_triggerCollider.collider.radius * .75f);
                    coughtPlayerCharater.transform.position = transform.position;
                }
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                direction = transform.position - girl.transform.position;

                if (direction.magnitude > m_triggerCollider.collider.radius * 4)
                {
                    girl.transform.position = transform.position + (direction.normalized * m_triggerCollider.collider.radius * .75f);
                    girl.transform.position = transform.position;
                }
            }

        }

        public void Pulled()
        {
            m_animatorParameters.isPulled = true;

            //int tempScore = m_playerCharacter.player.score;
            int addScore = 0;
            int addPlayer = 0;
             
            foreach(var playerCharacter in m_triggerCollider.coughtPlayerCharacters)
            {
                if (playerCharacter.player.score - m_playerCharacter.playerCharacterData.collectScoreMinus < 0)
                {
                    //m_playerCharacter.player.score += m_playerCharacter.playerCharacterData.collectScoreMinus;
                    //playerCharacter.player.score -= m_playerCharacter.playerCharacterData.collectScoreMinus;

                    //int addScore = Math.Abs(playerCharacter.player.score);
                    addScore += playerCharacter.player.score;

                    playerCharacter.player.score = 0;

                    //m_playerCharacter.player.score += addScore;
                }
                else
                {
                    playerCharacter.player.score -= m_playerCharacter.playerCharacterData.collectScoreMinus;

                    //m_playerCharacter.player.score += m_playerCharacter.playerCharacterData.collectScoreMinus;
                    addScore += m_playerCharacter.playerCharacterData.collectScoreMinus;
                }

                addPlayer += 1;
                playerCharacter.Collect();
                m_playerCharacter.npcGetParticle.Play();
            }

            foreach (var girl in m_triggerCollider.coughtGirls)
            {
                girl.Collect(m_playerCharacter.playerFire.transform.position);

                //m_playerCharacter.player.score++;
                addScore += 1;

                m_playerCharacter.npcGetParticle.Play();
            }

            m_animator.SetBool("isPulled", true);

            //int score = m_playerCharacter.player.score - tempScore;

            //if(addScore > 0)
            //{
            //    //StartCoroutine(m_playerCharacter.PulledCorutine(score));
            //    m_playerCharacter.StartPulledCorutine(addScore);
            //}

            m_playerCharacter.StartPulledCorutine(addScore, addPlayer);

            Destroy(gameObject);
        }

        public void CollectUpdate()
        {
            
        }

        public void Breake()
        {
            if(m_triggerCollider.coughtPlayerCharacters != null)
            {
                foreach (var playerCharacter in m_triggerCollider.coughtPlayerCharacters)
                {
                    playerCharacter.CatchRelease();
                }

                foreach (var girl in m_triggerCollider.coughtGirls)
                {
                    girl.CatchRibbonRelease();
                }
            }

            m_playerCharacter.BreakeRibbon();

            Destroy(gameObject);


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

            m_meshRenderer = transform.Find("RibbonMesh/ribbon_circle").GetComponent<MeshRenderer>();

            Transform colliderTransform         = transform.FindChild("RibbonCollider");
            Transform triggerColliderTransform  = transform.FindChild("RibbonTriggerCollider");
            Transform wallCollideTransform      = transform.FindChild("RibbonWallCollider");
            Transform pullAllowTransform        = transform.FindChild("PullStick");
            Transform ribbonLineTransform       = transform.FindChild("RibbonLine");
            Transform meshTransform             = transform.FindChild("RibbonMesh");

            m_colliderObject        = colliderTransform.gameObject;
            m_triggerColliderObject = triggerColliderTransform.gameObject;
            m_wallColliderObject    = wallCollideTransform.gameObject;
            m_pullArrowGameObject   = pullAllowTransform.gameObject;
            m_meshObject            = meshTransform.gameObject;
            m_pullStick             = m_pullArrowGameObject.GetComponent<PullStick>();
            m_ribbonLine            = ribbonLineTransform.gameObject.GetComponent<RibbonLine>();

            m_pullArrowDefaultLossyScale = m_pullArrowGameObject.transform.lossyScale;

            switch (gameObject.tag)
            {
                case "Player1":
                    {
                        m_meshRenderer.material = m_playerCharacter.ribbonMaterials[0];
                        //m_ribbonLine.lineRenderer.material = m_playerCharacter.ribbonMaterials[0];
                    }
                    break;

                case "Player2":
                    {
                        m_meshRenderer.material = m_playerCharacter.ribbonMaterials[1];
                    }
                    break;

                case "Player3":
                    {
                        m_meshRenderer.material = m_playerCharacter.ribbonMaterials[2];
                    }
                    break;

                case "Player4":
                    {
                        m_meshRenderer.material = m_playerCharacter.ribbonMaterials[3];
                    }
                    break;

                default:
                    {
                        Debug.LogAssertion("タグが設定されていません");
                        break;
                    }
            }       // end of switch(gameObject.tag)

            m_ribbonLine.lineRenderer.material = m_playerCharacter.ribbonLineMaterial;

            m_colliderObject.SetActive(false);
            m_triggerColliderObject.SetActive(false);
            //m_pullStick.spriteRenderer.enabled = false;

            m_triggerCollider = m_triggerColliderObject.GetComponent<RibbonTriggerCollider>();

            //playerCharacter = transform.parent.GetComponent<PlayerCharacter>();

            //GetComponent<HingeJoint>().connectedBody = m_playerCharacter.rigidbody;

            m_moveDirectionState = UnityEngine.Random.value < .5f ? MoveDirectionState.Left : MoveDirectionState.Right;

            switch (m_moveDirectionState)
            {
                case MoveDirectionState.Right:
                    m_pullStick.ChangeStickLeft();
                    break;
                case MoveDirectionState.Left:
                    m_pullStick.ChangeStickRight();
                    break;
            }

            //m_pullStick.spriteRenderer.enabled = false;
            m_pullStick.gameObject.SetActive(false);
            //m_pullArrowGameObject.transform.position = playerCharacter.transform.position;

            m_isStarted = true;

            _InitializeAnimatorParametersID();
        }

        void Update()
        {
            Vector3 direction = transform.position - playerCharacter.transform.position;

            direction.y = 0;

            if(direction.magnitude > .0f)
                transform.forward = direction;

            if (m_animatorParameters.isGrounded)
            {
                //transform.rotation = Quaternion.Euler(.0f, playerCharacter.transform.rotation.eulerAngles.y * -1.0f, .0f);

                //Vector3 aa =  transform.rotation.eulerAngles;

                

                //Vector3 position = playerCharacter.transform.position;

                //position.y += 5.0f;

                Vector3 center;

                center.x = (transform.position.x + playerCharacter.transform.position.x) / 2.0f;
                center.y = (transform.position.y + playerCharacter.transform.position.y) / 2.0f + 5.0f;
                center.z = (transform.position.z + playerCharacter.transform.position.z) / 2.0f;

                m_pullArrowGameObject.transform.position = center;

                Vector3 lossScale = m_pullArrowGameObject.transform.lossyScale;
                Vector3 localScale = m_pullArrowGameObject.transform.localScale;

                m_pullArrowGameObject.transform.localScale = new Vector3(
                        localScale.x / lossScale.x * m_pullArrowDefaultLossyScale.x,
                        localScale.y / lossScale.y * m_pullArrowDefaultLossyScale.y,
                        //localScale.y,
                        localScale.z / lossScale.z * m_pullArrowDefaultLossyScale.z);
            }
            else
            {
                //m_meshObject.transform.rotation = transform.rotation;
                //m_meshObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                //transform.rotation = m_playerCharacter.transform.rotation;
            }

            _UpdateAnimatorParameters();
        }

        void FixedUpdate()
        {
            if(m_isStarted)
            {
                if (m_isDoThrow)
                {
                    transform.position = m_throwPosition;
                    transform.rotation = m_throwRotation;
                    rigidbody.AddForce(Vector3.up * m_upPower + transform.forward * m_speed);

                    m_isDoThrow = false;
                }
            }

            if(m_isDoShake)
            {
                //transform.RotateAround(playerCharacter.transform.position, transform.up, m_shakeAngle * Time.deltaTime);

                Quaternion  q = Quaternion.AngleAxis(m_shakeAngle * Time.deltaTime, Vector3.up);
                Vector3     v = q * (transform.position - m_playerCharacter.transform.position) + m_playerCharacter.transform.position;

                m_rigidbody.MovePosition(v);
                //m_rigidbody.MovePosition(q * (m_playerCharacter.transform.position - transform.position) + transform.position);
                //m_rigidbody.MoveRotation(transform.rotation * q);

                //m_rigidbody.velocity = q * m_rigidbody.velocity;

                //m_rigidbody.AddForce(v * 200, ForceMode.);

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

            if(m_animatorParameters.isGrounded)
            {
                int objectsCount = m_triggerCollider.coughtGirls.Count + m_triggerCollider.coughtPlayerCharacters.Count;

                if(objectsCount > 0)
                {
                    float reboundPower = ((objectsCount * m_playerCharacter.playerCharacterData.ribbonReboundCountRatio) * m_playerCharacter.playerCharacterData.ribbonReboundPower);
                    m_rigidbody.AddForce(transform.forward * reboundPower, ForceMode.Force);
                }

                if (m_triggerCollider.coughtPlayerCharacters.Count < 1)
                {
                    float violenetPower = ((objectsCount * m_playerCharacter.playerCharacterData.ribbonReboundCountRatio) * m_playerCharacter.playerCharacterData.ribbonViolentMoveSpeed);

                    m_changeTime += Time.deltaTime;

                    float changeTime = m_changeTime / (m_playerCharacter.playerCharacterData.ribbonPenaltyTime / objectsCount);

                    if (changeTime > 1.0f)
                    {
                        //m_moveDirectionState = m_moveDirectionState == MoveDirectionState.Left ? MoveDirectionState.Left : MoveDirectionState.Right; 
                        if (m_moveDirectionState == MoveDirectionState.Left)
                        {
                            m_moveDirectionState = MoveDirectionState.Right;

                            m_pullStick.ChangeStickLeft();
                        }
                        else if (m_moveDirectionState == MoveDirectionState.Right)
                        {
                            m_moveDirectionState = MoveDirectionState.Left;

                            m_pullStick.ChangeStickRight();
                        }

                        m_changeTime = 0.0f;
                    }

                    switch (m_moveDirectionState)
                    {
                        case MoveDirectionState.Right:
                            m_rigidbody.AddForce(transform.right * violenetPower, ForceMode.Force);
                            break;
                        case MoveDirectionState.Left:
                            m_rigidbody.AddForce((transform.right * -1) * violenetPower, ForceMode.Force);
                            break;
                    }
                }

                m_time += Time.deltaTime;

                if(m_time > m_playerCharacter.playerCharacterData.ribbonPenaltyTime)
                {
                    Breake();
                }
            }

            if(transform.position.y < -1.0f)
            {
                transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
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

        private MeshRenderer m_meshRenderer;

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

        public MeshRenderer meshRenderer
        {
            get
            {
                return m_meshRenderer;
            }

            set
            {
                m_meshRenderer = value;
            }
        }

        public float time
        {
            get
            {
                return m_time;
            }

            set
            {
                m_time = value;
            }
        }

        private float m_time;
        private float m_changeTime;

        private GameObject m_pullArrowGameObject;

        private PullStick m_pullStick;

        private RibbonLine m_ribbonLine;

        private Vector3     m_throwPosition;
        private Quaternion  m_throwRotation;

        private bool m_isStarted;

        private GameObject m_meshObject;

        private Vector3 m_pullArrowDefaultLossyScale;

        public int caughtObjectCount
        {
            get{ return m_triggerCollider.coughtGirls.Count + m_triggerCollider.coughtPlayerCharacters.Count; }
        }
    }
}