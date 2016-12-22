using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    const string ConstDamagePrefabPath = "Prefab/GameEffect/UI/Damage";


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

    //private bool[] m_dispFlag;

	// Use this for initialization
	void Start () {

        // 状態初期化
        m_damageState = DamageState.None;

        // タイム初期化
        m_time = 0.0f;
        
        
        // 初期の大きさ保存
        m_saveScale = this.transform.localScale;

        // 消す
        this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
  


    }
	
	// Update is called once per frame
	void Update () {


        Vector3 objScale = this.transform.localScale;

        switch (m_damageState)
        {
            // 拡大中
            case DamageState.Big:
                {
                    objScale.x += m_speed * Time.deltaTime;
                    objScale.y += m_speed * Time.deltaTime;

                    // 元の大きさになったら
                    if (objScale.x > m_saveScale.x)
                    {
                        // 遷移
                        m_damageState = DamageState.Wait;

                        // 上限設定
                        objScale = m_saveScale;
                    }
                }
                break;

            // 止める
            case DamageState.Wait:
                {
                    m_time += Time.deltaTime;

                    // 一定時間たったら遷移
                    if (m_time > m_dispTime)
                    {
                        m_damageState = DamageState.Small;
                    }
                }
                break;

            // 縮小中
            case DamageState.Small:
                {
                    objScale.x -= m_smallSpeed * Time.deltaTime;
                    objScale.y -= m_smallSpeed * Time.deltaTime;

                    // 小さくなったら遷移
                    if (objScale.x <= 0.0f)
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
    /// ダメージエフェクト表示
    /// </summary>
    public void DispDamage()
    {
        if (m_damageState == DamageState.None)
        {
            m_damageState = DamageState.Big;
            m_time = 0.0f;
        }
    }
}
