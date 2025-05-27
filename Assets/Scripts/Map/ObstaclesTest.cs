using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesTest : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclesPrefabs;

    [Header("Obstacle Setting")]
    private float[] spawnXPosition = { -2.6f, 0f, 2.6f };
    private float spawnYPosition = 0.15f;
    private int maxObstaclesLane = 2;

    void SpawnObstacles(float zPosition)
    {
        List<int> selectedObstacles = new List<int>();

        while (selectedObstacles.Count < maxObstaclesLane)
        {
            int random = Random.Range(0, maxObstaclesLane);

            if (!selectedObstacles.Contains(random))
            {
                selectedObstacles.Add(random);
            }
        }

        foreach (int i in selectedObstacles)
        {
            Vector3 spawnPos = new Vector3(spawnXPosition[i], spawnYPosition, zPosition);
            int prefabsI = Random.Range(0, obstaclesPrefabs.Count);

            Instantiate(obstaclesPrefabs[prefabsI], spawnPos, Quaternion.identity);
        } 
    }
}
