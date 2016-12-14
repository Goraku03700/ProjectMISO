using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RayTest : MonoBehaviour
{
    Vector3[] m_screenPositions;
    public Vector3 m_screenPosition;
    public Image[] m_images;
    public Text[] m_texts;
    [SerializeField]
    float m_distance;
    [SerializeField]
    Color m_setAlphaColor;
    [SerializeField]
    Color m_setDefaultColor;
    public float m_radius;
    float radius;
    Ray[] rays;
    void Start()
    {
        m_screenPositions = new Vector3[2];
        rays = new Ray[2];
    }
    void Update()
    {
        m_screenPositions[0] = m_screenPositions[1] = new Vector3(Screen.width * m_screenPosition.x, Screen.height * m_screenPosition.y); ;
        m_screenPositions[0].x -= m_distance*Screen.width;
        m_screenPositions[1].x += m_distance * Screen.width;
        radius = Screen.width * m_radius;
        //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        rays[0] = Camera.main.ScreenPointToRay(m_screenPositions[0]);
        rays[1] = Camera.main.ScreenPointToRay(m_screenPositions[1]);
        RaycastHit hit;
        //レイヤーマスク作成
        int layerMask = LayerMask.GetMask(new string[] {"PlayerCharacter"});

        //Rayの長さ
        float maxDistance = 100;

        if (Physics.SphereCast(rays[0],radius, out hit, maxDistance, layerMask))
        {
            for (int i = 0; i < m_images.Length; ++i)
            {
                m_images[i].color = m_setAlphaColor;
            }
            for (int i = 0; i < m_texts.Length; ++i)
            {
                m_texts[i].color = m_setAlphaColor;
            }
            //Rayが当たるオブジェクトがあった場合はそのオブジェクト名をログに表示
            Debug.Log(hit.collider.gameObject.name);
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.red, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.green, 1, false);
        }
        else if (Physics.SphereCast(rays[1], radius, out hit, maxDistance, layerMask))
        {
            for (int i = 0; i < m_images.Length; ++i)
            {
                m_images[i].color = m_setAlphaColor;
            }
            for (int i = 0; i < m_texts.Length; ++i)
            {
                m_texts[i].color = m_setAlphaColor;
            }
            //Rayが当たるオブジェクトがあった場合はそのオブジェクト名をログに表示
            Debug.Log(hit.collider.gameObject.name);
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.green, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.red, 1, false);
        }
        else
        {
            for (int i = 0; i < m_images.Length; ++i)
            {
                m_images[i].color = m_setDefaultColor;
            }
            for (int i = 0; i < m_texts.Length; ++i)
            {
                m_texts[i].color = m_setDefaultColor;
            }
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.green, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.green, 1, false);
        }

        
    }
}
