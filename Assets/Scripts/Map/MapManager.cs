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
    // 생성할 맵 풀 개수
    [SerializeField] private int poolCountPerPrefab = 2;

    // 각 맵 조각의 길이 (z축 기준)
    private float mapLength = 10f;
    // 다음 맵 조각 Z위치 기준                           
    private float nextSpawnZ;
    // 플레이어의 거리가 가까워지면 재배치                                  
    private float triggerOffsetZ;

    private List<GameObject> mapPool = new List<GameObject>();
    // 중복 호출 방지용
    private bool isRecycling = false;

    void Start()
    {
        nextSpawnZ = 0f;
        triggerOffsetZ = firstSpawnCount * mapLength * 0.75f;

        // 프리팹 리스트 각각 poolCountPerPrefab 만큼 생성해서 풀에 넣기
        for (int i = 0; i < mapPrefabs.Count; i++)
        {
            for (int j = 0; j < poolCountPerPrefab; j++)
            {
                GameObject mapChunk = Instantiate(mapPrefabs[i]);
                mapChunk.SetActive(false);
                mapChunk.transform.SetParent(this.transform);
                mapPool.Add(mapChunk);
            }
        }

        // firstSpawnCount 만큼 맵 배치
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
            RecycleMapPiece();
        }
    }

    void SpawnNextMapPiece()
    {
        List<GameObject> inactiveList = mapPool.FindAll(m => !m.activeInHierarchy);

        if (inactiveList.Count > 0)
        {
            GameObject selected = inactiveList[Random.Range(0, inactiveList.Count)];
            selected.transform.position = new Vector3(0f, 0f, nextSpawnZ);
            selected.SetActive(true);
            nextSpawnZ += mapLength;
        }
    }

    // 기존 맵 조각을 앞으로 이동시켜 재사용
    void RecycleMapPiece()
    {
        isRecycling = true;

        for (int i = 0; i < mapPool.Count; i++)
        {
            GameObject map = mapPool[i];
            if (map.activeInHierarchy && map.transform.position.z < player.position.z - mapLength)
            {
                map.SetActive(false);
            }
        }

        SpawnNextMapPiece();
        isRecycling = false;
    }
}
