using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // 플레이어 위치
    [SerializeField] private Transform player;
    // Map_1~5 프리팹 리스트                
    [SerializeField] private List<GameObject> mapPrefabs;
    // 처음 배치할 맵 조각 개수
    [SerializeField] private int firstSpawnCount = 10;

    // 각 맵 조각의 길이 (z축 기준)
    private float mapLength = 10f;
    // 다음 맵 조각 Z위치 기준                           
    private float nextSpawnZ;
    // 플레이어의 거리가 가까워지면 재배치                                  
    private float triggerOffsetZ;

    private Queue<GameObject> mapPool = new Queue<GameObject>();
    // 중복 호출 방지용
    private bool isRecycling = false;

    void Start()
    {
        nextSpawnZ = 0;
        triggerOffsetZ = firstSpawnCount * mapLength * 0.75f; // 생성된 구간의 75% 다음 맵 조각 준비

        // 처음 맵 조각 배치
        for (int i = 0; i < firstSpawnCount; i++)
        {
            SpawnNextMapPiece();
        }
    }

    void Update()
    {
        // 플레이어가 다음 맵 조각 기준점에 가까워지면 맵 재배치
        if (player.position.z > nextSpawnZ - triggerOffsetZ && !isRecycling)
        {
            StartCoroutine(RecycleMapPieceCoroutine());
        }
    }

    void SpawnNextMapPiece()
    {
        // 첫 생성시 랜덤 패턴 배치
        int index = Random.Range(0, mapPrefabs.Count);

        GameObject newMap = Instantiate(mapPrefabs[index],
        new Vector3(0, 0, nextSpawnZ), Quaternion.identity);

        mapPool.Enqueue(newMap);
        nextSpawnZ += mapLength;
    }

    // 기존 맵 조각을 앞으로 이동시켜 재사용
    IEnumerator RecycleMapPieceCoroutine()
    {
        isRecycling = true;

        if (mapPool.Count > 0)
        {
            // 가장 오래된 맵 꺼냄
            GameObject oldMap = mapPool.Dequeue();
            // 앞쪽으로 이동                   
            oldMap.transform.position = new Vector3(0, 0, nextSpawnZ);

            // 다시 풀에 추가
            mapPool.Enqueue(oldMap);
            nextSpawnZ += mapLength;

            // 한 프레임 쉬어서 부하를 분산
            yield return null;
        }

        isRecycling = false;
    }
}
