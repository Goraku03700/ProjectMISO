using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ribbons
{
    public class RibbonTriggerCollider : MonoBehaviour
    {
        void Start()
        {
            m_coughtObjects = new List<GameObject>();
        }

        void OnTriggerEnter(Collider collider)
        {
            // Todo layermask
            if (collider.gameObject.layer == LayerMask.NameToLayer("PlayerCharacter"))
            { 
                coughtObjects.Add(collider.gameObject);

                collider.gameObject.GetComponent<PlayerCharacter>().CaughtRibbon();
            }
        }

        List<GameObject> m_coughtObjects;

        public List<GameObject> coughtObjects
        {
            get
            {
                return m_coughtObjects;
            }

            set
            {
                m_coughtObjects = value;
            }
        }
    }

}