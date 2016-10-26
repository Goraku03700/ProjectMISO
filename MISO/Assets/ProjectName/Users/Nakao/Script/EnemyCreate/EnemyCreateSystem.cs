using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCreateSystem : MonoBehaviour {

    [SerializeField]
    GameObject temporaryCreatePosition;

    [SerializeField]
    GameObject m_girlPrefabs;
    [SerializeField]
    List<GameObject> m_Girls;
    float time;

    bool createflag;

	// Use this for initialization
	void Start () {
        CreateGirl();
        time = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if(time>0.5f)
        {
            CreateCheckPosition();
            if (createflag && temporaryCreatePosition.GetComponent<SphereCollider>().isTrigger)
            {
                CreateGirl();
            }
        }
	}

    void CreateGirl()
    {
        GameObject newGirl = Instantiate(m_girlPrefabs);
        newGirl.gameObject.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0.37f, Random.Range(-4.0f, 4.0f));
        newGirl.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
        m_Girls.Add(newGirl);
        time = 0.0f;
    }

    void CreateCheckPosition()
    {
        Instantiate(temporaryCreatePosition);
        temporaryCreatePosition.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0.37f, Random.Range(-4.0f, 4.0f));
        createflag = true;
    }

}
