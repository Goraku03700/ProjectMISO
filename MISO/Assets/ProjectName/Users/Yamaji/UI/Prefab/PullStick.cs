using UnityEngine;
using System.Collections;

public class PullStick : MonoBehaviour {

    [SerializeField]
    private float m_speed;

    private SpriteRenderer m_spriteRenderer;

    enum StickState
    {
        Left,
        Right
  
    }
    private StickState m_stickState;

    private float m_rot;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return m_spriteRenderer;
        }

        set
        {
            m_spriteRenderer = value;
        }
    }

    void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {

        m_rot = 0.0f;

        //m_spriteRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_stickState)
        {
            case StickState.Left:
                {
                    m_rot += m_speed;
                    if (m_rot > 40.0f)
                        m_rot = 40.0f;
                }
                break;

            case StickState.Right:
                {
                    m_rot -= m_speed;
                    if (m_rot < -40.0f)
                        m_rot = -40.0f;
                }
                break;
        }

        this.transform.Rotate(0.0f, 0.0f, m_rot);
        //this.transform.Rotate(new Vector3(0, 0, 1), m_rot);


    }

    public void ChangeStickLeft()
    {
        m_stickState = StickState.Left;
    }

    public void ChangeStickRight()
    {
        m_stickState = StickState.Right;
    }
}
