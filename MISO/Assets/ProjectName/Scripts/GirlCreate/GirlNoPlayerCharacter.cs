using UnityEngine;
using System.Collections;

public class GirlNoPlayerCharacter : MonoBehaviour 
{
    /// <summary>
    /// NPCの状態
    /// </summary>
    public enum State
    {
        Generation,
        Alive,
        Caught,
        Absorption,
        None
    }
    
    bool m_isDestroy;
    public bool IsDestroy
    {
        get { return m_isDestroy; }
        set { m_isDestroy = value; }
    }

    bool m_isCaught;
    public bool IsCaught
    {
        get { return m_isCaught; }
        set { m_isCaught = value; }
    }

    bool m_isAbsorption;
    public bool IsAbsorption
    {
        get { return m_isAbsorption; }
        set { m_isAbsorption = value; }
    }

    GirlAppearancePosition m_parntGirlAppearancePosition;

    public GirlAppearancePosition m_ParntGirlAppearancePosition
    {
        get { return m_parntGirlAppearancePosition; }
        set { m_parntGirlAppearancePosition = value; }
    }
    [SerializeField]
    State m_status;

    [SerializeField]
    float m_time,m_scale;

    [SerializeField]
    Movable m_movable;
    
    public GameObject m_girlMesh;

    Vector2 m_movementAreaX; //xに最小値yに最大値
    public Vector2 m_MovementAreaX
    {
        get { return m_movementAreaX; }
        set { m_movementAreaX = value; }
    }
    Vector2 m_movementAreaZ;
    public Vector2 m_MovementAreaZ
    {
        get { return m_movementAreaZ; }
        set { m_movementAreaZ = value; }
    }

    [SerializeField]
    LineRenderer m_ribbonLine;

    [SerializeField]
    Color[] m_ribbonColors;
	// Use this for initialization
	void Start () {
        m_status = State.Generation;
        m_time = 0.0f;
        m_targetPosition = GetRandomPositionOnLevel();
        m_ribbonLine.enabled = false;
	}

    [SerializeField]
    float m_speed = 0.15f;
    [SerializeField]
    float m_rotationSmooth = 0.005f;

    public Vector3 m_targetPosition;

    private float m_changeTargetSqrDistance = 0.5f;

    private Quaternion m_beforeRotation;

    private Vector3 m_beforePosition;

    [SerializeField]
    GameObject m_ribbon_Wind;

    [SerializeField]
    MeshRenderer m_ribbon_WindRenderer;

    [SerializeField]
    Material[] m_ribbon_WindMaterial1;

    [SerializeField]
    Material[] m_ribbon_WindMaterial2;

    [SerializeField]
    Material[] m_ribbon_WindMaterial3;

    [SerializeField]
    Material[] m_ribbon_WindMaterial4;

    [SerializeField]
    ParticleSystem m_particleSystem;

    Bezier m_bezier;


    public Vector3 GetRandomPositionOnLevel()
    {
        m_beforeRotation = this.transform.rotation;
        m_beforePosition = this.transform.position;
        return new Vector3(Random.Range(m_movementAreaX.x, m_movementAreaX.y), 0, Random.Range(m_movementAreaZ.x, m_movementAreaZ.y));
    }
	
	// Update is called once per frame
	void Update () {
        m_girlMesh.transform.rotation = new Quaternion(0.0f,m_girlMesh.transform.rotation.y,0.0f,1.0f);
 //       m_girlMesh.transform.rotation = 0.0f;_
        m_time += Time.deltaTime;
	    switch(m_status)
        {
            case State.Generation:
            {
                /*float scale = Mathf.Lerp(0.0f,m_scale,m_time);
                m_girlMesh.transform.localScale = new Vector3(scale,scale,scale);
                //登場演出
                if(scale == m_scale)
                {
                    m_status = State.Alive;
                    m_movable.direction = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
                    m_movable.speed = 3.0f;
                    m_time = 0.0f;
                }
                */
                if (m_time >= 0.25f)
                {
                    float scale = Mathf.Lerp(0.0f, m_scale, m_time);
                    m_girlMesh.transform.localScale = new Vector3(scale, scale, scale);
                }
                if(m_time >= 1.0f)
                {
                    m_status = State.Alive;
                    m_girlMesh.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
                    m_movable.direction = Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)));
                    //m_movable.speed = 4.0f;
                    m_time = 0.0f;
                }
                //m_status = State.Alive;
                break;
            }
            case State.Alive:
            {
                float sqrDistanceToTarget = Vector3.SqrMagnitude(this.transform.position - m_targetPosition);
                if (sqrDistanceToTarget < m_changeTargetSqrDistance)
                {
                    m_targetPosition = GetRandomPositionOnLevel();
                }

                // 目標地点の方向を向く
                Quaternion targetRotation = Quaternion.LookRotation(m_targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * m_rotationSmooth);

                // 前方に進む
                transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
                //ここに移動を実装予定
                if (m_time > 4.0f)
                {
                    //m_movable.direction = Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)));
                    //m_time = 0.0f;
                }

                if (m_isCaught)
                {
                    m_movable.speed = 0.0f;
                    m_status = State.Caught;
                }
                break;
            }
            case State.Caught:
            {
                Quaternion targetRotation = Quaternion.LookRotation(m_targetPosition - transform.position);
                transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, 1.0f);
                m_ribbonLine.SetPosition(0, this.transform.localPosition + Vector3.up / 1.5f);
                //なんか処理
                if(m_isAbsorption)
                {
                    m_ribbonLine.enabled = false;
                    m_time = 0.0f;
                    m_status = State.Absorption;
                    m_particleSystem.startColor = new  Color32(82,255,68,255);
                    m_particleSystem.emissionRate = 20.0f;
                    m_particleSystem.Play();
                }

                break;
            }
            case State.Absorption:
            {
                if (m_time > 1f)
                {
                    m_status = State.None;
                    m_time = 1f;
                }
                m_particleSystem.startSize = Mathf.Lerp(2.0f, 0.0f, m_time);
                transform.localPosition =  m_bezier.GetPointAtTime(m_time);
                transform.localScale = Vector3.Lerp(new Vector3(0.1f,0.1f,0.1f), Vector3.zero, m_time);
                //ベジェ曲線での取得演出処理
                
                break;
            }
            case State.None:
            {
                //取得をUIに通知
                //m_girlMesh.SetActive(false);
                //m_parntGirlAppearancePosition.IsDestroy = true;
                m_parntGirlAppearancePosition.m_ParntGirlCreater.m_CreateGirlNumber--;
                Destroy(m_girlMesh);
                Destroy(this);
                break;
            }
            default:
            {
                break;
            }

        }


	}

    void OnCollisionEnter(Collision collision)
    {
        int i = LayerMask.NameToLayer("RibbonTrigger");
        if(collision.gameObject.layer == i)
        {
            if(collision.gameObject.tag == "Player1")
            {
                this.gameObject.tag = collision.gameObject.tag;
            }
            if (collision.gameObject.tag == "Player2")
            {
                this.gameObject.tag = collision.gameObject.tag;
            }
            m_isCaught = true;
            this.m_status = State.None;			gameObject.layer = LayerMask.NameToLayer("CaughtGirl");        }
    }

    public void CatchRibbon(PlayerCharacter playerCharacter)
    {
        gameObject.tag      = playerCharacter.gameObject.tag;
        m_isCaught          = true;
        gameObject.layer    = LayerMask.NameToLayer("CaughtGirl");
        m_status            = State.Caught;
        m_targetPosition = playerCharacter.transform.localPosition;
        // 目標地点の方向を向く
        
        m_ribbonLine.enabled = true;
        m_ribbonLine.sortingOrder = 4;
        m_ribbonLine.SetPosition(0, this.transform.localPosition + Vector3.up/1.5f);
        m_ribbonLine.SetPosition(1, playerCharacter.transform.position + Vector3.up/1.5f);
        m_ribbon_Wind.SetActive(true);
        switch(gameObject.tag)
        {
            case "Player1":
                {
                    m_ribbonLine.SetColors(m_ribbonColors[0], m_ribbonColors[0]);
                    m_ribbon_WindRenderer.materials = m_ribbon_WindMaterial1;
                    //m_ribbon_WindRenderer.materials = m_ribbon_WindMaterials[0];
                    break;
                }
            case "Player2":
                {
                    m_ribbonLine.SetColors(m_ribbonColors[1], m_ribbonColors[1]);
                    m_ribbon_WindRenderer.materials = m_ribbon_WindMaterial2;
                    break;
                }
            case "Player3":
                {
                    m_ribbonLine.SetColors(m_ribbonColors[2], m_ribbonColors[2]);
                    m_ribbon_WindRenderer.materials = m_ribbon_WindMaterial3;
                    break;
                }
            case "Player4":
                {
                    m_ribbonLine.SetColors(m_ribbonColors[3], m_ribbonColors[3]);
                    m_ribbon_WindRenderer.materials = m_ribbon_WindMaterial4;
                    break;
                }
        }
    }

    public void CatchRibbonRelease()
    {
        m_isCaught          = false;
        gameObject.layer    = LayerMask.NameToLayer("Girl");
        m_status            = State.Alive;
    }

    public void Collect(Vector3 billPosition)
    {
        m_isAbsorption = true;
        m_bezier = new Bezier(this.transform.localPosition, Vector3.Lerp(this.transform.localPosition, billPosition, 0.4f) + Vector3.up * 2f, Vector3.Lerp(this.transform.position, billPosition, 0.6f) + Vector3.up * 2f, billPosition);

        //m_bezier.ResetBezier(transform.position, Vector3.Lerp(transform.position, billPosition, 0.4f) + Vector3.up * 2, Vector3.Lerp(transform.position, billPosition, 0.6f) + Vector3.up * 2, billPosition);
    }
}
