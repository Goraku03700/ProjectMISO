using UnityEngine;
using System.Collections;

/// <summary>
/// 指定された方向へ移動するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Movable : MonoBehaviour
{
	private void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        //m_rigidbody.AddForce();
    }

    private Rigidbody m_rigidbody;

    /// <summary>
    /// 移動方向ベクトル　
    /// </summary>
    /// <remarks>
    /// -1.0f～1.0fの範囲を推奨
    /// </remarks>
    [SerializeField, Tooltip("移動方向ベクトル"), Range(-1.0f, 1.0f)]
    private Vector3 m_direction;

    /// <summary>
    /// 移動方向ベクトル(m_directionのプロパティ)
    /// </summary>
    /// <remarks>
    /// -1.0f～1.0fの範囲を推奨
    /// </remarks>
    public Vector3 direction
    {
        get { return direction; }
        set { m_direction = value; }
    }

    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField, Tooltip("移動速度")]
    private float m_speed;

    /// <summary>
    /// 移動速度(m_speedのプロパティ)
    /// </summary>
    public float speed
    {
        get { return speed; }
        set { m_speed = value; }
    }
}