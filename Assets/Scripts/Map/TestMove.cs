using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void Update()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
