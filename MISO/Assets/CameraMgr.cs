using UnityEngine;
using System.Collections;

public class CameraMgr : MonoBehaviour
{

    public Transform pos;
    public GameObject target;
    public Vector3 offset;
    // Use this for initialization
    void Start()
    {
        target = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        pos = target.GetComponent<Transform>();
        this.GetComponent<Transform>().position = pos.position + offset;
    }
}
