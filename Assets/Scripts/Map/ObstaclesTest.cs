using System.Collections.Generic;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs;

    private float[] spawnXPositions = { -2.6f, 0f, 2.6f };
    private float spawnYPosition = 0.15f;
    private int maxObstaclesLane = 2;

    private Dictionary<int, Queue<GameObject>> obstaclePools = new Dictionary<int, Queue<GameObject>>();
    private int poolSizePerObstacle = 3;

    void Start()
    {
        // 장애물 풀 초기화
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            Queue<GameObject> pool = new Queue<GameObject>();
            for (int j = 0; j < poolSizePerObstacle; j++)
            {
                GameObject obj = Instantiate(obstaclePrefabs[i], transform);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            obstaclePools.Add(i, pool);
        }
    }

    public void SpawnObstaclesAtZ(float baseZ)
    {
        int obstacleLines = Random.Range(1, 3); // 1~2줄 생성

        float currentZ = baseZ;

        for (int i = 0; i < obstacleLines; i++)
        {
            SpawnObstacles(currentZ);
            currentZ += 5f;
        }
    }

    void SpawnObstacles(float zPosition)
    {

        List<int> selectedLanes = new List<int>();

        while (selectedLanes.Count < maxObstaclesLane)
        {
            int lane = Random.Range(0, spawnXPositions.Length);
            if (!selectedLanes.Contains(lane))
                selectedLanes.Add(lane);
        }

        foreach (int lane in selectedLanes)
        {
            int prefabIndex = Random.Range(0, obstaclePrefabs.Count);
            GameObject obstacle = GetObstacleFromPool(prefabIndex);

            obstacle.transform.position = new Vector3(spawnXPositions[lane], spawnYPosition, zPosition);
            obstacle.transform.rotation = Quaternion.identity;
            obstacle.SetActive(true);
            obstacle.transform.parent = null; // 원한다면 부모 해제

            Debug.Log($"Spawn obstacle: prefabIndex={prefabIndex}, lane={lane}, position={obstacle.transform.position}");
        }
    }

    GameObject GetObstacleFromPool(int prefabIndex)
    {
        if (obstaclePools.TryGetValue(prefabIndex, out var pool) && pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(obstaclePrefabs[prefabIndex], transform);
            obj.SetActive(false);
            return obj;
        }
    }

    public void ReturnObstacleToPool(int prefabIndex, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform; // 장애물 매니저 밑으로 정리

        if (obstaclePools.TryGetValue(prefabIndex, out var pool))
        {
            pool.Enqueue(obj);
        }
    }
}
