using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform obstacle; // 장애물 오브젝트 넣기

    public Vector3 SetRandomPlace(Vector3 lastPosition)
    {
        Vector3 placePosition = lastPosition;
    }
    
}
