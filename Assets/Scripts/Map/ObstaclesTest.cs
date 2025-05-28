using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    // 플레이어 위치
    [SerializeField] private Transform player;
    // 장애물 프리팹
    [SerializeField] private List<GameObject> obstaclePrefabs;
    // 최초 장애물 생성 시작 위치
    [SerializeField] private float obstacleStartZ = 25f;
    // 처음 배치할 장애물 수
    [SerializeField] private int firstSpawnCount = 5;
    

    // 장애물 생성 Z 간격
    private float spawnIntervalZ = 5f;
    // 다음 장애물 생성 위치
    private float nextSpawnZ;
    // 재배치 트리거 거리
    private float triggerOffsetZ;

    // 3개의 라인
    private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f };

    // 장애물 풀
    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private Queue<GameObject> activeObstacles = new Queue<GameObject>();

    // 풀에서 순서대로 뽑는 인덱스
    private int currentPrefabIndex = 0;
    // 재활용 중복 실행 방지
    private bool isRecycling = false;

    void Start()
    {
        nextSpawnZ = obstacleStartZ;
        // 재활용 체크를 위해 플레이어 기준 거리
        triggerOffsetZ = firstSpawnCount * spawnIntervalZ;

        // 비활성화 상태로 장애물들을 생성 후 풀에 넣음
        for (int i = 0; i < firstSpawnCount; i++)
        {
            GameObject obj = Instantiate(GetNextPrefab());
            obj.SetActive(false);
            obstaclePool.Enqueue(obj);
        }
        // 위에서 만든 장애물들을 앞에 배치하며 활성화
        for (int i = 0; i < firstSpawnCount; i++)
        {
            SpawnNextObstacle();
        }
    }

    void Update()
    {
        // 재활용 중이 아니고 활성 장애물이 있을 때
        if (!isRecycling && activeObstacles.Count > 0)
        {
            // 가장 앞에 있는 장애물
            GameObject frontObstacle = activeObstacles.Peek();
            // 플레이어와 장애물 간 거리
            float distanceBehind = player.position.z - frontObstacle.transform.position.z;

            if (distanceBehind > spawnIntervalZ * 1.0f) // 지나친 거리 기준
            {
                StartCoroutine(RecycleObstacleCoroutine());
            }
        }
    }

    void SpawnNextObstacle()
    {
        // 풀에서 비활성 장애물 꺼내기
        GameObject obstacle = obstaclePool.Dequeue();

        // x축 3개 중 랜덤
        float x = spawnXPosition[Random.Range(0, spawnXPosition.Length)];
        // z축은 다음 생성 위치
        float z = nextSpawnZ;
        // 장애물 위치 설정
        obstacle.transform.position = new Vector3(x, 0.15f, z);
        obstacle.SetActive(true);
        // 활성 장애물 큐에 추가
        activeObstacles.Enqueue(obstacle);
        // 다음 장애물 z 위치 계산
        nextSpawnZ += spawnIntervalZ;
    }

    IEnumerator RecycleObstacleCoroutine()
    {
        isRecycling = true;
        // 활성 장애물이 남아있고 플레이어가 지나간 장애물이 있을 때 계속 재활용
        while (activeObstacles.Count > 0)
        {
            GameObject frontObstacle = activeObstacles.Peek();
            // 플레이어가 해당 장애물을 지나갔는지 확인 spawnIntervalZ 만큼
            if (player.position.z - frontObstacle.transform.position.z > spawnIntervalZ)
            {
                // 지나간 장애물 비활성화하고 풀에 반환
                ReturnObstacleToPool(activeObstacles.Dequeue());
                // 다음 장애물 생성
                SpawnNextObstacle();
            }
            else
            {
                // 안지나갔음 재활용 종료
                break;
            }
            // 한 프레임 대기 후 계속 재활용
            yield return null;
        }
        isRecycling = false;
    }
    /// <summary>
    /// 장애물 프리팹에서 순서대로 가져오는 함수
    /// </summary>
    /// <returns></returns>
    GameObject GetNextPrefab()
    {
        // 현재 인덱스 값을 가져오고
        GameObject prefab = obstaclePrefabs[currentPrefabIndex];
        // 순환하도록 인덱스 증가
        currentPrefabIndex = (currentPrefabIndex + 1) % obstaclePrefabs.Count;
        return prefab;
    }
    /// <summary>
    /// 장애물 비활성화 및 풀에 넣는 함수
    /// </summary>
    /// <param name="obj"></param>
    void ReturnObstacleToPool(GameObject obj)
    {
        obj.SetActive(false);
        obstaclePool.Enqueue(obj);
    }
}