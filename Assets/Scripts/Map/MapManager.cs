using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform player;                 // 플레이어 위치
    [SerializeField] private List<GameObject> mapPrefabs;      // Map_1~5 프리팹 리스트
    [SerializeField] private int firstSpawnCount = 5;          // 처음 배치할 맵 조각 개수
    private float mapLength = 10f;                             // 각 맵 조각의 길이 (z축 기준)

    private Queue<GameObject> mapPool = new Queue<GameObject>();
    private float nextSpawnZ;                // 다음 맵 조각 위치 기준
    private int lastUsedIndex = -1;          // 바로 직전 쓴 맵 인덱스 (중복 방지용)

    void Start()
    {
        nextSpawnZ = 0;

        // 처음 5개 맵 조각 배치
        for (int i = 0; i < firstSpawnCount; i++)
        {
            SpawnNextMapPiece();
        }
    }

    void Update()
    {
        float TriggerZ = nextSpawnZ - (firstSpawnCount * mapLength * 0.75f);
        // 플레이어가 다음 맵 조각 기준점에 가까워지면 맵 재배치
        if (player.position.z > TriggerZ)
        {
            RecycleMapPiece();
        }
    }

    void SpawnNextMapPiece()
    {
        int index = GetRandomIndex();

        GameObject newMap = Instantiate(mapPrefabs[index], new Vector3(0, 0, nextSpawnZ), Quaternion.identity);
        mapPool.Enqueue(newMap);

        nextSpawnZ += mapLength;
        lastUsedIndex = index;
    }

    void RecycleMapPiece()
    {
        if (mapPool.Count == 0) return;

        // 맵 조각 꺼내서 앞으로 재배치
        GameObject oldMap = mapPool.Dequeue();

        int index = GetRandomIndex();
        oldMap.transform.position = new Vector3(0, 0, nextSpawnZ);

        // 원한다면 여기서 내부 오브젝트 활성/비활성도 조절 가능

        mapPool.Enqueue(oldMap);
        nextSpawnZ += mapLength;
        lastUsedIndex = index;
    }

    int GetRandomIndex()
    {
        int idx;
        do
        {
            idx = Random.Range(0, mapPrefabs.Count);
        } while (idx == lastUsedIndex && mapPrefabs.Count > 1);

        return idx;
    }
}
