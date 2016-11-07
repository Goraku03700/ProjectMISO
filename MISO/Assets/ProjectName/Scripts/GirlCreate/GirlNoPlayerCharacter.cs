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

	// Use this for initialization
	void Start () {
        m_status = State.Generation;
        m_time = 0.0f;
        m_targetPosition = GetRandomPositionOnLevel();
	}

    [SerializeField]
    float m_speed = 0.15f;
    [SerializeField]
    float m_rotationSmooth = 40f;

    public Vector3 m_targetPosition;

    private float m_changeTargetSqrDistance = 0.5f;

    [SerializeField]
    float m_levelSize;

    public Vector3 GetRandomPositionOnLevel()
    {
        m_levelSize = 15f;
        return new Vector3(Random.Range(-m_levelSize, m_levelSize), 0, Random.Range(-m_levelSize, m_levelSize));
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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSmooth);

                // 前方に進む
                transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
                //ここに移動を実装予定
                if (m_time > 4.0f)
                {
                    //m_movable.direction = Vector3.Normalize(new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)));
                    m_time = 0.0f;
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
                //なんか処理
                if(m_isAbsorption)
                {
                    
                    m_status = State.Absorption;
                }

                break;
            }
            case State.Absorption:
            {
                //ベジェ曲線での取得演出処理
                //if() 
                {
                    m_status = State.None;
                }
                break;
            }
            case State.None:
            {
                if (this.gameObject.tag == "Player1")
                {
                    m_parntGirlAppearancePosition.m_ParntGirlCreater.m_ParntGirlCreateSystem.GetNPC_Player(0);
                }
                if (this.gameObject.tag == "Player2")
                {
                    m_parntGirlAppearancePosition.m_ParntGirlCreater.m_ParntGirlCreateSystem.GetNPC_Player(1);
                }
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
        int i = LayerMask.NameToLayer("Ribbon");
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
            this.m_status = State.None;
        }
    }


}
