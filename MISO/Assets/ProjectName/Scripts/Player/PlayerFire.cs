using UnityEngine;
using System.Collections;

public class PlayerFire : MonoBehaviour {

    [SerializeField]
    float m_pushPower = 10.0f;

    public void Fire(Transform playerTransform, Rigidbody playerRigidbody)
    {
        playerTransform.position = this.transform.position + Vector3.up + (this.transform.forward * -1);
        playerTransform.rotation = Quaternion.Inverse(this.transform.rotation);
        playerRigidbody.velocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        playerRigidbody.AddForce(-this.transform.forward * m_pushPower + Vector3.up * m_pushPower);
        playerRigidbody.mass = 1.0f;
        BGMManager.instance.PlaySE("se016_OutCampany");
    }
}
