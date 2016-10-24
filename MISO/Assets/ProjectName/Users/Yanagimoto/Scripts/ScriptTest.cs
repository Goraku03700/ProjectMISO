using UnityEngine;
using System.Collections;

namespace Test
{
    public class ScriptTest : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            m_sceneSharedData = SceneSharedData.instance;
        }

        // Update is called once per frame
        void Update()
        {
            int hp = (int)m_sceneSharedData.Get("Title", "HP");
            hp = (int)m_sceneSharedData.Get("Stage", "HP");
            TitleTest.State state = (TitleTest.State)m_sceneSharedData.Get("Title", "State");
        }

        SceneSharedData m_sceneSharedData;
    }

}

