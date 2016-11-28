using UnityEngine;
using System.Collections;

public class SkyboxControl : MonoBehaviour
{
    public GameObject skyboxCamera;//スカイボックスカメラ
    public float skyboxChangeAngle;//１フレームで回転させたい値
    public Vector3 skyboxAxis;//回転軸
    float skyboxAngle;//変更後の角度（アングル）

    private bool isDark;

    private Material m_material;

    [SerializeField]
    private Color m_startColor;

    [SerializeField]
    private Color m_endColor;

    [SerializeField]
    private float m_changeColorSpeed;

    void Start()
    {
        //skybox = RenderSettings.skybox;
        //RenderSettings.skybox.shader = Shader.Find("_Tint");
        
        //RenderSettings.skybox.SetColor("_Tint", Color.red);

        m_material = RenderSettings.skybox;

        m_material.SetColor("_Tint", m_startColor);

        isDark = false;

    }

    //スカイボックスを回転させる
    void Update()
    {
        skyboxAngle += skyboxChangeAngle;
        skyboxCamera.transform.rotation = Quaternion.AngleAxis(skyboxAngle, skyboxAxis);

        if(isDark)
        {
            // 現在の色を取得
            Color nowColor = m_material.GetColor("_Tint");

            // 設定する色
            Color setColor = new Color(nowColor.r + (-m_changeColorSpeed) * Time.deltaTime,
                                       nowColor.g + (-m_changeColorSpeed) * Time.deltaTime,
                                       nowColor.b + (-m_changeColorSpeed) * Time.deltaTime);

            if(setColor.r < m_endColor.r)
            {
                setColor = m_endColor;
            }

            // 色を暗くする
            m_material.SetColor("_Tint", setColor);
        }
    }

    public void Dark()
    {
        isDark = true;
    }
}
