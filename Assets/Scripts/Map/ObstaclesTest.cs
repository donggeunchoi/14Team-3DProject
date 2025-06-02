using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    // 플레이어 Transform 참조 (위치 비교용)
    [SerializeField] private Transform player;
    // 장애물 프리팹 리스트 (Inspector에서 설정)
    [SerializeField] private List<GameObject> obstaclePrefabs;
    // 게임 시작 시 최초 장애물 생성 z 위치 (한 번만 사용)
    [SerializeField] private float obstacleStartZ = 25f;
    // 각 프리팹별로 초기 풀에 미리 생성할 개수
    [SerializeField] private int firstSpawnCount = 4;

    // 그룹 간 z 축 간격
    private float spawnIntervalZ = 8f;
    // 다음으로 생성할 장애물의 z 위치
    private float nextSpawnZ;
    // 장애물을 생성할 수 있는 x 축 라인 위치 3개
    private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f };

    // 프리팹별로 관리할 오브젝트 풀 (Queue)
    private Dictionary<GameObject, Queue<GameObject>> obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
    // 현재 활성화된 장애물을 순서대로 보관할 큐
    private Queue<GameObject> activeObstacles = new Queue<GameObject>();

    // 재활용 코루틴이 실행 중인지 여부
    private bool isRecycling = false;

    // 플레이어 앞쪽으로 장애물을 미리 생성할 최대 거리 (Inspector에서 설정 가능)
    [SerializeField] private float maxAheadDistance = 70f;

    // 게임 시작 시 한 번만 호출되는 초기화 메서드
    void Start()
    {
        // nextSpawnZ를 게임 시작 시 사용할 obstacleStartZ로 초기화
        nextSpawnZ = obstacleStartZ;

        // 프리팹별로 오브젝트 풀을 생성
        foreach (var prefab in obstaclePrefabs)
        {
            // 현재 프리팹 키로 빈 큐 생성
            obstaclePools[prefab] = new Queue<GameObject>();
            // firstSpawnCount만큼 미리 Instantiate해서 풀에 저장
            for (int i = 0; i < firstSpawnCount; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                obstaclePools[prefab].Enqueue(obj);
            }
        }

        // Start()에서는 obstacleStartZ만 한 번 사용하고, 추가 생성은 Update()에서 처리
    }

    // 매 프레임 호출되는 업데이트 메서드
    void Update()
    {
        // 재활용 중이 아니고 활성 장애물이 있을 때만 재활용 조건 검사
        if (!isRecycling && activeObstacles.Count > 0)
        {
            GameObject frontObstacle = activeObstacles.Peek();
            float distanceBehind = player.position.z - frontObstacle.transform.position.z;

            // 플레이어가 해당 장애물을 일정 거리 이상 지나가면 재활용 시작
            if (distanceBehind > spawnIntervalZ * 1.5f)
            {
                StartCoroutine(RecycleObstacleCoroutine());
            }
        }

        // 항상 플레이어 + maxAheadDistance까지 장애물이 있도록 보충
        float targetZ = player.position.z + maxAheadDistance;
        while (nextSpawnZ < targetZ)
        {
            SpawnNextObstacle();
        }
    }

    // 새로운 장애물 그룹을 생성하는 메서드
    void SpawnNextObstacle()
    {
        // 한 그룹 당 생성할 장애물 개수를 1~2 사이에서 랜덤 선택
        int obstacleCount = Random.Range(1, 3); // {1, 2}

        // 라인 순서를 무작위로 섞기 위해 리스트 복사 후 셔플
        List<float> shuffledLines = new List<float>(spawnXPosition);
        Shuffle(shuffledLines);

        // obstacleCount 개수만큼, 각각 다른 x 위치에서 생성
        for (int i = 0; i < obstacleCount; i++)
        {
            // shuffledLines[i]는 중복 없는 서로 다른 라인
            float x = shuffledLines[i];

            // 장애물 프리팹을 랜덤으로 선택
            GameObject prefab = GetRandomPrefab();
            Queue<GameObject> pool = obstaclePools[prefab];

            GameObject obstacle;
            // 풀에 비활성화된 오브젝트가 있으면 재사용
            if (pool.Count > 0)
            {
                obstacle = pool.Dequeue();
            }
            else
            {
                // 풀에 없으면 새로 Instantiate
                obstacle = Instantiate(prefab);
                obstacle.SetActive(false);
                obstacle.transform.SetParent(this.transform);
            }

            // 장애물 회전값 초기화
            obstacle.transform.rotation = Quaternion.identity;
            // ObstacleBehavior 컴포넌트 초기화 호출
            obstacle.GetComponent<ObstacleBehavior>().InitObstacle();
            // 장애물 위치 설정 (x 라인, y=0, z=nextSpawnZ)
            obstacle.transform.position = new Vector3(x, 0f, nextSpawnZ);
            // 장애물 활성화
            obstacle.SetActive(true);

            // 활성 장애물 큐에 등록
            activeObstacles.Enqueue(obstacle);
        }

        // 다음 그룹 위치를 spawnIntervalZ만큼 증가
        nextSpawnZ += spawnIntervalZ;
    }

    // 장애물을 재활용하고 풀에 반환하는 코루틴
    IEnumerator RecycleObstacleCoroutine()
    {
        // 중복 실행 방지를 위해 플래그 설정
        isRecycling = true;

        // 활성 장애물이 남아 있는 동안 반복
        while (activeObstacles.Count > 0)
        {
            GameObject frontObstacle = activeObstacles.Peek();
            // 플레이어가 장애물을 충분히 지나가면 재활용
            if (player.position.z - frontObstacle.transform.position.z > spawnIntervalZ * 1.5f)
            {
                GameObject removed = activeObstacles.Dequeue();
                GameObject prefab = FindOriginalPrefab(removed);
                ReturnObstacleToPool(removed, prefab);
                // 재활용된 자리와 상관없이 다음 그룹은 Update()에서 player + maxAheadDistance 기준으로 채워줌
            }
            else
            {
                break;
            }
            yield return null;
        }

        // 재활용 완료 후 플래그 해제
        isRecycling = false;
    }

    // obstaclePrefabs 리스트에서 랜덤으로 프리팹 하나를 반환
    GameObject GetRandomPrefab()
    {
        int randomIndex = Random.Range(0, obstaclePrefabs.Count);
        return obstaclePrefabs[randomIndex];
    }

    // 장애물을 비활성화하고 해당 프리팹 풀에 반환
    void ReturnObstacleToPool(GameObject obj, GameObject prefab)
    {
        obj.SetActive(false);
        obstaclePools[prefab].Enqueue(obj);
    }

    // 인스턴스된 장애물의 이름으로 원본 프리팹을 찾아 반환
    GameObject FindOriginalPrefab(GameObject instance)
    {
        foreach (var prefab in obstaclePrefabs)
        {
            if (instance.name.StartsWith(prefab.name))
            {
                return prefab;
            }
        }
        return obstaclePrefabs[0];
    }

    // 제네릭 리스트를 받아 Fisher-Yates 알고리즘으로 무작위 순서로 셔플
    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}