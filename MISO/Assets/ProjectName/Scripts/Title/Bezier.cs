using UnityEngine;
using System.Collections;

public class Bezier : MonoBehaviour {

    private Vector3 m_p0;
    private Vector3 m_p1;
    private Vector3 m_p2;
    private Vector3 m_p3;

    private float m_ti = 0f;

    private Vector3 m_b0 = Vector3.zero;
    private Vector3 m_b1 = Vector3.zero;
    private Vector3 m_b2 = Vector3.zero;
    private Vector3 m_b3 = Vector3.zero;

    private float m_Ax;
    private float m_Ay;
    private float m_Az;

    private float m_Bx;
    private float m_By;
    private float m_Bz;

    private float m_Cx;
    private float m_Cy;
    private float m_Cz;

    // Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point
    // handle1 = v0 + v1
    // handle2 = v3 + v2
    public Bezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        this.m_p0 = v0;
        this.m_p1 = v1;
        this.m_p2 = v2;
        this.m_p3 = v3;
    }
    // 0.0 >= t <= 1.0
    public Vector3 GetPointAtTime(float t)
    {
        this.CheckConstant();
        float t2 = t * t;
        float t3 = t * t * t;
        float x = this.m_Ax * t3 + this.m_Bx * t2 + this.m_Cx * t + m_p0.x;
        float y = this.m_Ay * t3 + this.m_By * t2 + this.m_Cy * t + m_p0.y;
        float z = this.m_Az * t3 + this.m_Bz * t2 + this.m_Cz * t + m_p0.z;
        return new Vector3(x, y, z);
    }

    private void SetConstant()
    {
        this.m_Cx = 3f * ((this.m_p0.x + this.m_p1.x) - this.m_p0.x);
        this.m_Bx = 3f * ((this.m_p3.x + this.m_p2.x) - (this.m_p0.x + this.m_p1.x)) - this.m_Cx;
        this.m_Ax = this.m_p3.x - this.m_p0.x - this.m_Cx - this.m_Bx;
        this.m_Cy = 3f * ((this.m_p0.y + this.m_p1.y) - this.m_p0.y);
        this.m_By = 3f * ((this.m_p3.y + this.m_p2.y) - (this.m_p0.y + this.m_p1.y)) - this.m_Cy;
        this.m_Ay = this.m_p3.y - this.m_p0.y - this.m_Cy - this.m_By;

        this.m_Cz = 3f * ((this.m_p0.z + this.m_p1.z) - this.m_p0.z);
        this.m_Bz = 3f * ((this.m_p3.z + this.m_p2.z) - (this.m_p0.z + this.m_p1.z)) - this.m_Cz;
        this.m_Az = this.m_p3.z - this.m_p0.z - this.m_Cz - this.m_Bz;
    }

    // Check if m_p0, m_p1, m_p2 or m_p3 have change
    private void CheckConstant()
    {
        if (this.m_p0 != this.m_b0 || this.m_p1 != this.m_b1 || this.m_p2 != this.m_b2 || this.m_p3 != this.m_b3)
        {
            this.SetConstant();
            this.m_b0 = this.m_p0;
            this.m_b1 = this.m_p1;
            this.m_b2 = this.m_p2;
            this.m_b3 = this.m_p3;
        }
    }

    /// <summary>
    /// ベジェ曲線の計算をリセットする
    /// </summary>
    /// <param name="v0">始点</param>
    /// <param name="v1">始点のハンドルとなる点</param>
    /// <param name="v2">終点のハンドルとなる点</param>
    /// <param name="v3">終点</param>
    public void ResetBezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        if (m_p0 == v0 && m_p1 == v1 && m_p2 == v2 && m_p3 == v3)
        {

        }
        else
        {
            this.m_p0 = v0;
            this.m_p1 = v1;
            this.m_p2 = v2;
            this.m_p3 = v3;

            this.m_Ax = 0.0f;
            this.m_Ay = 0.0f;
            this.m_Az = 0.0f;

            this.m_Bx = 0.0f;
            this.m_By = 0.0f;
            this.m_Bz = 0.0f;

            this.m_Cx = 0.0f;
            this.m_Cy = 0.0f;
            this.m_Cz = 0.0f;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
