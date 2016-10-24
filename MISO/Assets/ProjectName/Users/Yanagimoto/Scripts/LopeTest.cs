using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Test
{
    [RequireComponent(typeof(Rigidbody))]
    public class LopeTest : MonoBehaviour
    {
        public enum State
        {
            Wait,
            Throw,
            Pull,
            Conflict,
        }

        public void Throw(Vector3 position, Quaternion rotation, float power, float size)
        {
            transform.position = position;
            transform.rotation = rotation;

            m_rigidBody.AddForce(transform.forward * power);
        }

        public void Pull()
        {
            
        }

        void Start()
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }

        private Rigidbody m_rigidBody;        
    }
}

