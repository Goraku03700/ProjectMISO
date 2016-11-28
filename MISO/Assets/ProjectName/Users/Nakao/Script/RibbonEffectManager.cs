using UnityEngine;
using System.Collections;

public class RibbonEffectManager : MonoBehaviour {

    [SerializeField]
    RibbonEffect m_ribbonEffectPrefabs;

    [SerializeField]
    Color[] m_playerImageColor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CreateRibbonEffect(Vector3 pos , PlayerCharacter player)
    {
        RibbonEffect obj;
        obj = (RibbonEffect)Instantiate(m_ribbonEffectPrefabs, pos, this.transform.rotation);

        switch(player.tag)
        {
            case "Player1":
                {
                    obj.m_ribbonParticle.startColor = m_playerImageColor[0];
                    break;
                }
            case "Player2":
                {
                    obj.m_ribbonParticle.startColor = m_playerImageColor[1];
                    break;
                }
            case "Player3":
                {
                    obj.m_ribbonParticle.startColor = m_playerImageColor[2];
                    break;
                }
            case "Player4":
                {
                    obj.m_ribbonParticle.startColor = m_playerImageColor[3];
                    break;
                }
        }
        
    }
    
}
