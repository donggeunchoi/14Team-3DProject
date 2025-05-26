using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    Rigidbody _rigidbody;
    public float speed = 5f;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.z <= -15)
        {
            Destroy(this.gameObject);
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.back * speed; // 장애물 이동속도
    }
}