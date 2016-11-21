using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ribbons
{
    public class RibbonTriggerCollider : MonoBehaviour
    {
        void Start()
        {
            m_parentRibbon  = transform.parent.GetComponent<Ribbon>();
            //m_coughtObjects = new List<GameObject>();
            m_coughtGirls = new List<GirlNoPlayerCharacter>();
            m_coughtPlayerCharacters = new List<PlayerCharacter>();

            //Physics.SphereCastAll()
        }

        void FixedUpdate()
        {
            //if (m_isOneFrameDuration)
            //{
            //    gameObject.SetActive(false);
            //}

            //m_isOneFrameDuration = true;

            m_durationTime += Time.deltaTime;

            if(m_durationTime > m_activeTime)
            {
                gameObject.SetActive(false);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            // Todo layermask
            if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerCharacter"))
            { 
                if(collider.gameObject != m_parentRibbon.playerCharacter.gameObject)
                {
                    //coughtObjects.Add(collider.gameObject);

                    PlayerCharacter playerCharacter = collider.gameObject.GetComponent<PlayerCharacter>();

                    playerCharacter.CaughtRibbon(m_parentRibbon);

                    coughtPlayerCharacters.Add(playerCharacter);
                }
            }
            else if(collider.gameObject.layer == LayerMask.NameToLayer("Girl"))
            {
                //coughtObjects.Add(collider.gameObject);

                var girl = collider.gameObject.GetComponent<GirlNoPlayerCharacter>();

                girl.CatchRibbon(m_parentRibbon.playerCharacter);

                coughtGirls.Add(girl);
            }
        }

        private bool m_isOneFrameDuration;

        private float m_durationTime;

        [SerializeField]
        private float m_activeTime;

        private Ribbon m_parentRibbon;

        //private List<GameObject> m_coughtObjects;

        //public List<GameObject> coughtObjects
        //{
        //    get
        //    {
        //        return m_coughtObjects;
        //    }

        //    set
        //    {
        //        m_coughtObjects = value;
        //    }
        //}

        private List<PlayerCharacter> m_coughtPlayerCharacters;

        public List<PlayerCharacter> coughtPlayerCharacters
        {
            get
            {
                return m_coughtPlayerCharacters;
            }

            set
            {
                m_coughtPlayerCharacters = value;
            }
        }

        private List<GirlNoPlayerCharacter> m_coughtGirls;

        public List<GirlNoPlayerCharacter> coughtGirls
        {
            get
            {
                return m_coughtGirls;
            }

            set
            {
                m_coughtGirls = value;
            }
        }
    }

}