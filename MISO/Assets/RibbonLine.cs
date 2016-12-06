using UnityEngine;
using System.Collections;

namespace Ribbons
{
    public class RibbonLine : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            m_ribbon = transform.parent.GetComponent<Ribbon>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 center;

            center.x = (m_ribbon.transform.position.x + m_ribbon.playerCharacter.transform.position.x) / 2.0f;
            center.y = (m_ribbon.transform.position.y + m_ribbon.playerCharacter.transform.position.y) / 2.0f;
            center.z = (m_ribbon.transform.position.z + m_ribbon.playerCharacter.transform.position.z) / 2.0f - 2.5f;

            transform.position = center;
            transform.LookAt(m_ribbon.transform);

            Vector3 direction = m_ribbon.transform.position - m_ribbon.playerCharacter.transform.position;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, direction.magnitude / 5);
        }

        Ribbon m_ribbon;
    }
}