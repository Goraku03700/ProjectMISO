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
    Color m_setAlphaColor;
    [SerializeField]
    Color m_setDefaultColor;
    public float radius = 3f;
    Ray[] rays;
    void Start()
    {
        m_screenPositions = new Vector3[2];
        rays = new Ray[2];
    }
    void Update()
    {
        m_screenPositions[0] = m_screenPositions[1] = m_screenPosition;
        m_screenPositions[0].x -= 25;
        m_screenPositions[1].x += 25;

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
            m_images[0].color = m_setAlphaColor;
            m_images[1].color = m_setAlphaColor;
            m_texts[0].color = m_setAlphaColor;
            m_texts[1].color = m_setAlphaColor;
            //Rayが当たるオブジェクトがあった場合はそのオブジェクト名をログに表示
            Debug.Log(hit.collider.gameObject.name);
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.red, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.green, 1, false);
        }
        else if (Physics.SphereCast(rays[1], radius, out hit, maxDistance, layerMask))
        {
            m_images[0].color = m_setAlphaColor;
            m_images[1].color = m_setAlphaColor;
            m_texts[0].color = m_setAlphaColor;
            m_texts[1].color = m_setAlphaColor;
            //Rayが当たるオブジェクトがあった場合はそのオブジェクト名をログに表示
            Debug.Log(hit.collider.gameObject.name);
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.green, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.red, 1, false);
        }
        else
        {
            m_images[0].color = m_setDefaultColor;
            m_images[1].color = m_setDefaultColor;
            m_texts[0].color = m_setDefaultColor;
            m_texts[1].color = m_setDefaultColor;
            //Rayを画面に表示
            Debug.DrawRay(rays[0].origin, rays[0].direction * maxDistance, Color.green, 1, false);
            Debug.DrawRay(rays[1].origin, rays[1].direction * maxDistance, Color.green, 1, false);
        }

        
    }
}
