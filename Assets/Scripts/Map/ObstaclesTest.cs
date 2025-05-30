using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
// {
//     // 플레이어 위치
//     [SerializeField] private Transform player;
//     // 장애물 프리팹
//     [SerializeField] private List<GameObject> obstaclePrefabs;
//     // 최초 장애물 생성 시작 위치
//     [SerializeField] private float obstacleStartZ = 25f;
//     // 처음 배치할 장애물 수
//     [SerializeField] private int firstSpawnCount = 5;


//     // 장애물 생성 Z 간격
//     private float spawnIntervalZ = 5f;
//     // 다음 장애물 생성 위치
//     private float nextSpawnZ;

//     // 3개의 라인
//     private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f };

//     // 장애물 풀
//     private Queue<GameObject> obstaclePool = new Queue<GameObject>();
//     private Queue<GameObject> activeObstacles = new Queue<GameObject>();

//     // 풀에서 순서대로 뽑는 인덱스
//     private int currentPrefabIndex = 0;
//     // 재활용 중복 실행 방지
//     private bool isRecycling = false;

//     void Start()
//     {
//         nextSpawnZ = obstacleStartZ;

//         // 비활성화 상태로 장애물들을 생성 후 풀에 넣음
//         for (int i = 0; i < firstSpawnCount; i++)
//         {
//             GameObject obj = Instantiate(GetNextPrefab());
//             obj.SetActive(false);
//             obstaclePool.Enqueue(obj);
//         }
//         // 위에서 만든 장애물들을 앞에 배치하며 활성화
//         for (int i = 0; i < firstSpawnCount; i++)
//         {
//             SpawnNextObstacle();
//         }
//     }

//     void Update()
//     {
//         // 재활용 중이 아니고 활성 장애물이 있을 때
//         if (!isRecycling && activeObstacles.Count > 0)
//         {
//             // 가장 앞에 있는 장애물
//             GameObject frontObstacle = activeObstacles.Peek();
//             // 플레이어와 장애물 간 거리
//             float distanceBehind = player.position.z - frontObstacle.transform.position.z;

//             if (distanceBehind > spawnIntervalZ * 1.0f) // 지나친 거리 기준
//             {
//                 StartCoroutine(RecycleObstacleCoroutine());
//             }
//         }
//     }

//     void SpawnNextObstacle()
//     {
//         // 풀에서 비활성 장애물 꺼내기
//         GameObject obstacle = obstaclePool.Dequeue();

//         // x축 3개 중 랜덤
//         float x = spawnXPosition[Random.Range(0, spawnXPosition.Length)];
//         // z축은 다음 생성 위치
//         float z = nextSpawnZ;
//         // 장애물 위치 설정
//         obstacle.transform.position = new Vector3(x, 0f, z);
//         obstacle.SetActive(true);
//         // 활성 장애물 큐에 추가
//         activeObstacles.Enqueue(obstacle);
//         // 다음 장애물 z 위치 계산
//         nextSpawnZ += spawnIntervalZ;
//     }

//     IEnumerator RecycleObstacleCoroutine()
//     {
//         isRecycling = true;
//         // 활성 장애물이 남아있고 플레이어가 지나간 장애물이 있을 때 계속 재활용
//         while (activeObstacles.Count > 0)
//         {
//             GameObject frontObstacle = activeObstacles.Peek();
//             // 플레이어가 해당 장애물을 지나갔는지 확인 spawnIntervalZ 만큼
//             if (player.position.z - frontObstacle.transform.position.z > spawnIntervalZ)
//             {
//                 // 지나간 장애물 비활성화하고 풀에 반환
//                 ReturnObstacleToPool(activeObstacles.Dequeue());
//                 // 다음 장애물 생성
//                 SpawnNextObstacle();
//             }
//             else
//             {
//                 // 안지나갔음 재활용 종료
//                 break;
//             }
//             // 한 프레임 대기 후 계속 재활용
//             yield return null;
//         }
//         isRecycling = false;
//     }
//     /// <summary>
//     /// 장애물 프리팹에서 순서대로 가져오는 함수
//     /// </summary>
//     /// <returns></returns>
//     GameObject GetNextPrefab()
//     {
//         // 현재 인덱스 값을 가져오고
//         GameObject prefab = obstaclePrefabs[currentPrefabIndex];
//         // 순환하도록 인덱스 증가
//         currentPrefabIndex = (currentPrefabIndex + 1) % obstaclePrefabs.Count;
//         return prefab;
//     }
//     /// <summary>
//     /// 장애물 비활성화 및 풀에 넣는 함수
//     /// </summary>
//     /// <param name="obj"></param>
//     void ReturnObstacleToPool(GameObject obj)
//     {
//         obj.SetActive(false);
//         obstaclePool.Enqueue(obj);
//     }
// }
{
    // 플레이어 위치 참조
    [SerializeField] private Transform player;

    // 사용할 장애물 프리팹 리스트
    [SerializeField] private List<GameObject> obstaclePrefabs;

    // 장애물 초기 생성 시작 Z 위치
    [SerializeField] private float obstacleStartZ = 25f;

    // 시작할 때 생성할 장애물 개수
    [SerializeField] private int firstSpawnCount = 5;

    // 장애물 간 Z축 거리 간격
    private float spawnIntervalZ = 8f;

    // 장애물을 생성할 수 있는 X 위치 후보들 (좌, 중간, 우)
    private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f };

    // 장애물 재사용을 위한 풀 (비활성화된 장애물)
    private Queue<GameObject> obstaclePool = new Queue<GameObject>();

    // 현재 활성화되어 있는 장애물 목록
    private Queue<GameObject> activeObstacles = new Queue<GameObject>();

    // 프리팹 순환 인덱스
    private int currentPrefabIndex = 0;

    // 중복 재활용 방지를 위한 플래그
    private bool isRecycling = false;

    void Start()
    {
        // 미리 정해진 개수만큼 프리팹을 생성하고 풀에 넣음
        for (int i = 0; i < firstSpawnCount; i++)
        {
            GameObject obj = Instantiate(GetNextPrefab()); // 프리팹 순서대로 생성
            obj.SetActive(false); // 비활성화 상태로 풀에 보관
            obstaclePool.Enqueue(obj); // 풀에 저장
        }

        // 최초 장애물 배치: obstacleStartZ부터 일정 간격으로 배치
        for (int i = 0; i < firstSpawnCount; i++)
        {
            float z = obstacleStartZ + i * spawnIntervalZ; // i번째 장애물의 Z 위치 계산
            SpawnNextObstacle(z - spawnIntervalZ); // 이전 위치를 기준으로 생성
        }
    }

    void Update()
    {
        // 현재 재활용 중이 아니고 활성 장애물이 존재할 때만 체크
        if (!isRecycling && activeObstacles.Count > 0)
        {
            GameObject frontObstacle = activeObstacles.Peek(); // 맨 앞 장애물 가져옴
            float distanceBehind = player.position.z - frontObstacle.transform.position.z; // 얼마나 지나쳤는지 계산

            // 장애물을 일정 거리 이상 지나쳤다면 재활용 시작
            if (distanceBehind > spawnIntervalZ * 1.0f)
            {
                StartCoroutine(RecycleObstacleCoroutine()); // 재활용 코루틴 실행
            }
        }
    }

    void SpawnNextObstacle(float baseZ)
    {
        float z = baseZ + spawnIntervalZ;

        // 라인 인덱스 섞기 후 1~2개 선택
        List<int> lineIndices = new List<int> { 0, 1, 2 };
        ShuffleList(lineIndices);
        int spawnCount = Random.Range(1, 3); // 1 또는 2개 라인 선택

        for (int i = 0; i < spawnCount; i++)
        {
            float x = spawnXPosition[lineIndices[i]];

            GameObject obstacle = GetRandomAvailableObstacleFromPool();
            if (obstacle == null)
            {
                continue; // 사용할 장애물이 없으면 이 라인은 스킵
            }

            obstacle.transform.position = new Vector3(x, 0f, z);
            obstacle.SetActive(true);
            activeObstacles.Enqueue(obstacle);
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randIndex];
            list[randIndex] = temp;
        }
    }


    // 일정 거리 뒤처진 장애물을 비활성화하고 재활용하는 코루틴
    IEnumerator RecycleObstacleCoroutine()
    {
        isRecycling = true; // 재활용 중 상태 설정

        // 재활용할 장애물이 남아있을 때 반복
        while (activeObstacles.Count > 0)
        {
            GameObject frontObstacle = activeObstacles.Peek(); // 제일 앞 장애물 가져옴

            // 일정 거리 이상 지나쳤으면 재활용 진행
            if (player.position.z - frontObstacle.transform.position.z > spawnIntervalZ)
            {
                ReturnObstacleToPool(activeObstacles.Dequeue()); // 비활성화 후 풀에 넣기

                // 현재 큐에 있는 마지막 장애물의 z 위치를 기준으로 새로운 장애물 생성
                float lastZ = activeObstacles.Count > 0
                    ? activeObstacles.ToArray()[activeObstacles.Count - 1].transform.position.z // 마지막 장애물 위치
                    : obstacleStartZ; // 없으면 기본 위치 사용

                SpawnNextObstacle(lastZ); // 새 장애물 생성
            }
            else
            {
                break; // 아직 지나지 않았다면 루프 종료
            }

            yield return null; // 다음 프레임까지 대기
        }

        isRecycling = false; // 재활용 종료
    }

    // 프리팹 리스트에서 순서대로 다음 프리팹 반환
    GameObject GetNextPrefab()
    {
        GameObject prefab = obstaclePrefabs[currentPrefabIndex]; // 현재 인덱스 프리팹 선택
        currentPrefabIndex = (currentPrefabIndex + 1) % obstaclePrefabs.Count; // 인덱스 순환
        return prefab;
    }

    GameObject GetRandomAvailableObstacleFromPool()
    {
        // 프리팹 순서를 섞어서 시도
        List<int> indices = new List<int>();
        for (int i = 0; i < obstaclePrefabs.Count; i++) indices.Add(i);
        ShuffleList(indices);

        foreach (int index in indices)
        {
            GameObject prefab = obstaclePrefabs[index];

            // 풀에서 이 프리팹과 동일한 걸 찾음
            foreach (GameObject obj in obstaclePool)
            {
                if (obj.name.StartsWith(prefab.name) && !obj.activeInHierarchy)
                {
                    obstaclePool = new Queue<GameObject>(obstaclePool); // 복사해서 Dequeue 가능하게
                    GameObject found = null;

                    // 실제 꺼내기
                    int count = obstaclePool.Count;
                    for (int i = 0; i < count; i++)
                    {
                        GameObject dequeued = obstaclePool.Dequeue();
                        if (!found && dequeued.name.StartsWith(prefab.name))
                        {
                            found = dequeued;
                        }
                        else
                        {
                            obstaclePool.Enqueue(dequeued);
                        }
                    }

                    return found;
                }
            }
        }

        return null; // 모든 프리팹이 없으면 null 반환
    }


    // 장애물 비활성화하고 풀에 다시 넣는 함수
    void ReturnObstacleToPool(GameObject obj)
    {
        obj.SetActive(false); // 비활성화
        obstaclePool.Enqueue(obj); // 풀에 추가
    }
}