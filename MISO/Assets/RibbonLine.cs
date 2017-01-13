using UnityEngine;
using System.Collections;

namespace Ribbons
{
    public class RibbonLine : MonoBehaviour
    {

        void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
        }

        void Start()
        {
            m_ribbon = transform.parent.GetComponent<Ribbon>();

            //Vector3 start, end;

            //start = m_startTransform.position;
            //end = gameObject.transform.position;

            //m_lineRenderer.SetPosition(0, start);
            //m_lineRenderer.SetPosition(1, end);            

        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 center;

            //center.x = (m_ribbon.transform.position.x + m_ribbon.playerCharacter.transform.position.x) / 2.0f;
            //center.y = (m_ribbon.transform.position.y + m_ribbon.playerCharacter.transform.position.y) / 2.0f;
            //center.z = (m_ribbon.transform.position.z + m_ribbon.playerCharacter.transform.position.z) / 2.0f - 2.5f;

            //transform.position = center;
            //transform.LookAt(m_ribbon.transform);

            //Vector3 direction = m_ribbon.transform.position - m_ribbon.playerCharacter.transform.position;

            //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, direction.magnitude / 5);

            m_lineRenderer.enabled = true;

            Vector3 start, end;

            start   = m_startTransform.position;
            end     = gameObject.transform.position;

            //start.z += 0.5f;

            m_lineRenderer.SetPosition(0, start);
            m_lineRenderer.SetPosition(1, end);

            float t = 1.0f - m_ribbon.time / m_ribbon.playerCharacter.playerCharacterData.ribbonPenaltyTime;

            if (t < 0.0f)
                t = 0.0f;

            float width = t * m_defaultWidth;

            if(width < 0)
            {
                width = 0.1f;
            }

            m_lineRenderer.SetWidth(width, width);
        }

        [SerializeField]
        Transform m_startTransform;

        Ribbon m_ribbon;
        LineRenderer m_lineRenderer;

        [SerializeField]
        float m_defaultWidth = 1.0f;

        public Transform startTransform
        {
            get
            {
                return m_startTransform;
            }

            set
            {
                m_startTransform = value;
            }
        }

        public LineRenderer lineRenderer
        {
            get
            {
                return m_lineRenderer;
            }

            set
            {
                m_lineRenderer = value;
            }
        }
    }
}