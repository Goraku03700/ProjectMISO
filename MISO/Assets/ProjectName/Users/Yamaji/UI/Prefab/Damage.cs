using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {
   
    [SerializeField]
    private float m_speed;

    [SerializeField]
    private float m_smallSpeed;


    [SerializeField]
    private float m_dispTime;


    enum DamageState
    {
        Big,
        Wait,
        Small,
        None,
    }
    private DamageState m_damageState; 

    private float m_time;

    private Vector3 m_saveScale;

    private bool m_dispFlag;

	// Use this for initialization
	void Start () {

        m_saveScale = this.transform.localScale;

        if (m_speed == 0)
            m_speed = 1.0f;

        this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

        m_damageState = DamageState.None;
   
        m_time = 0.0f;

        m_dispFlag = false;

    }
	
	// Update is called once per frame
	void Update () {

        
        Vector3 objScale = this.transform.localScale;

        switch(m_damageState)
        {
            case DamageState.Big:
                {
                    objScale.x += m_speed * Time.deltaTime;
                    objScale.y += m_speed * Time.deltaTime;

                    if(objScale.x > m_saveScale.x)
                    {
                        m_damageState = DamageState.Wait;
                    }
                }
                break;

            case DamageState.Wait:
                {
                    m_time += Time.deltaTime;

                    if(m_time > m_dispTime)
                    {
                        m_damageState = DamageState.Small;
                    }
                }
                break;

            case DamageState.Small:
                {
                    objScale.x -= m_smallSpeed * Time.deltaTime;
                    objScale.y -= m_smallSpeed * Time.deltaTime;

                    if(objScale.x <= 0.0f)
                    {
                        objScale.x = 0.0f;
                        objScale.y = 0.0f;
                        m_damageState = DamageState.None;
                    }
                }
                break;
        }

        this.transform.localScale = objScale;
	
    }

    /// <summary>
    /// ダメージのエフェクト出す
    /// </summary>
    public void DispDamage()
    {
        m_damageState = DamageState.Big;

        m_time = 0.0f;
    }
}
