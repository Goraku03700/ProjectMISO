﻿using UnityEngine;
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

    State m_status;


    
    public GameObject m_girlMesh;

	// Use this for initialization
	void Start () {
        m_status = State.Generation;
        //m_girlMesh.SetActive(false);
	}

    
	
	// Update is called once per frame
	void Update () {
	    switch(m_status)
        {
            case State.Generation:
            {
                //登場演出
                if(Input.GetKeyDown(KeyCode.L))
                {
                    m_status = State.None;
                }
                //m_status = State.Alive;
                break;
            }
            case State.Alive:
            {
                //ここに移動を実装予定

                if (m_isCaught)
                {
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
}
