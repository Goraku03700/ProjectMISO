using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ImageRandomizer : MonoBehaviour {

    //[SerializeField]
    //Texture[] m_images;

    [SerializeField]
    List<Texture> m_textures;

    //Image m_image;

    //[SerializeField]
    public void Randomize()
    {
        Image image = GetComponent<Image>();

        if(image == null)
        {
            return;
        }

        int index = Random.Range(0, m_textures.Count - 1);

        //Debug.Log(index.ToString());

        //image.material.mainTexture = m_textures[index];
        //image.mainTexture;
        
        
    }

    void Awake()
    {
        Randomize();
    }
}
