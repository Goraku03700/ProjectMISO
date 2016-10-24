using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Test
{
    public class TitleTest : MonoBehaviour
    {

        public enum State
        {
            Atack,
            Jump,
        }


        // Use this for initialization
        void Start()
        {
            m_sceneSharedData = SceneSharedData.instance;

            m_sceneSharedData.Set("Title", "HP", 20);
            m_sceneSharedData.Set("Stage", "HP", 100);
            m_sceneSharedData.Set("Title", "State", State.Jump);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return))
                SceneManager.LoadScene("ScriptTest");
        }

        private SceneSharedData m_sceneSharedData;
    }
}

