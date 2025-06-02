using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform player;                     // 1) 플레이어 Transform 참조 (Inspector 연결)
    [SerializeField] private List<GameObject> obstaclePrefabs;     // 2) 장애물 프리팹 리스트 (Inspector 연결)
    [SerializeField] private float obstacleStartZ = 40f;            // 3) 장애물 최초 배치 시작 z 위치
    [SerializeField] private int firstSpawnCount = 4;               // 4) 프리팹별로 풀에 미리 생성할 개수

    private float spawnIntervalZ = 8f;                              // 5) 장애물 그룹 간 z축 간격
    private float nextSpawnZ;                                       // 6) 다음 생성할 장애물 z 위치
    private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f }; // 7) 장애물 x축 위치 후보 3개 (왼중오)

    private Dictionary<GameObject, Queue<GameObject>> obstaclePools = new Dictionary<GameObject, Queue<GameObject>>(); // 8) 프리팹별 장애물 풀 관리용 딕셔너리
    private Queue<GameObject> activeObstacles = new Queue<GameObject>();  // 9) 현재 활성화된 장애물들을 순서대로 보관하는 큐

    private bool isRecycling = false;                               // 10) 장애물 재활용 코루틴 실행 중 여부

    [SerializeField] private float maxAheadDistance = 70f;          // 11) 플레이어 앞까지 미리 생성해 둘 최대 거리

    void Start()
    {
        // 다음 장애물 생성 위치를 최초 시작 위치로 초기화
        nextSpawnZ = obstacleStartZ;                               

        // 프리팹별로 미리 풀에 장애물 생성하여 비활성 상태로 저장
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            // 현재 프리팹 참조
            GameObject prefab = obstaclePrefabs[i];               
            // 해당 프리팹에 대한 큐(풀) 초기화
            obstaclePools[prefab] = new Queue<GameObject>();      
            // firstSpawnCount만큼 미리 생성
            for (int j = 0; j < firstSpawnCount; j++)
            {
                // 장애물 인스턴스 생성
                GameObject obj = Instantiate(prefab);
                // 비활성화 상태로 설정
                obj.SetActive(false);
                // 이 매니저 오브젝트 하위에 넣기(정리용)
                obj.transform.SetParent(this.transform);
                // 풀에 등록
                obstaclePools[prefab].Enqueue(obj);
            }
        }
    }

    void Update()
    {
        // 재활용 코루틴이 실행중이 아니고 활성 장애물이 있을 때
        if (!isRecycling && activeObstacles.Count > 0)
        {
            // 가장 오래된 활성 장애물 확인
            GameObject frontObstacle = activeObstacles.Peek();    
            // 플레이어와 장애물 z 거리 계산
            float distanceBehind = player.position.z - frontObstacle.transform.position.z;
            // 일정 거리 이상 뒤로 가면 재활용 시작
            if (distanceBehind > spawnIntervalZ * 1.5f)
            {
                // 코루틴으로 장애물 재활용 실행
                StartCoroutine(RecycleObstacleCoroutine());
            }
        }
        // 플레이어 앞 maxAheadDistance 지점 계산
        float targetZ = player.position.z + maxAheadDistance;
        // 아직 장애물 생성 위치가 플레이어 앞까지 도달 안 했으면    
        if (nextSpawnZ < targetZ)
        {
            // 장애물 추가 생성
            SpawnNextObstacle();
        }
    }

    void SpawnNextObstacle()
    {
        // 1개 또는 2개의 장애물을 생성
        int obstacleCount = Random.Range(1, 3);                  
        // x축 위치 후보 리스트 복사
        List<float> shuffledLines = new List<float>(spawnXPosition); 
        // 위치 후보 섞기
        Shuffle(shuffledLines);                                   
        // 장애물 개수만큼 반복
        for (int i = 0; i < obstacleCount; i++)                  
        {
            // 섞인 x 위치에서 하나 선택
            float x = shuffledLines[i];                           
            // 랜덤으로 프리팹 선택
            GameObject prefab = GetRandomPrefab();
            // 해당 프리팹의 풀 가져오기                
            Queue<GameObject> pool = obstaclePools[prefab];       
            // 풀에 비활성화된 장애물이 있으면
            GameObject obstacle;
            if (pool.Count > 0)                                    
            {
                // 하나 꺼내기 (재활용)
                obstacle = pool.Dequeue();                         
            }
            else
            {
                // 없으면 새로 생성
                obstacle = Instantiate(prefab);                    
                // 비활성화 상태로 시작
                obstacle.SetActive(false);                          
                // 매니저 자식으로 정리
                obstacle.transform.SetParent(this.transform);      
            }
            // 회전 초기화
            obstacle.transform.rotation = Quaternion.identity;
            // 장애물 초기화 함수 호출
            obstacle.GetComponent<ObstacleBehavior>().InitObstacle(); 
            // 위치 설정 (x, y=0, z=nextSpawnZ)
            obstacle.transform.position = new Vector3(x, 0f, nextSpawnZ); 
            // 활성화
            obstacle.SetActive(true);                              
            // 활성 장애물 큐에 등록
            activeObstacles.Enqueue(obstacle);                     
        }
        // 다음 생성 위치 z값 증가
        nextSpawnZ += spawnIntervalZ;                              
    }

    IEnumerator RecycleObstacleCoroutine()
    {   
        // 재활용 시작 표시
        isRecycling = true;                                        

        while (activeObstacles.Count > 0)
        {
            // 가장 오래된 장애물 확인
            GameObject frontObstacle = activeObstacles.Peek(); 
            // 플레이어가 충분히 멀어졌으면   
            if (player.position.z - frontObstacle.transform.position.z > spawnIntervalZ)
            {
                // 큐에서 제거
                GameObject removed = activeObstacles.Dequeue();
                // 원본 프리팹 찾기
                GameObject prefab = FindOriginalPrefab(removed);
                // 장애물을 풀에 반환
                ReturnObstacleToPool(removed, prefab);
            }
            else
            {
                // 아직 멀지 않으면 중단
                break;
            }
            // 한 프레임 대기 후 계속
            yield return null;                                    
        }
        // 재활용 완료 표시
        isRecycling = false;                                       
    }

    GameObject GetRandomPrefab()
    {
        // 랜덤 인덱스 선택
        int randomIndex = Random.Range(0, obstaclePrefabs.Count);  
        // 해당 프리팹 반환
        return obstaclePrefabs[randomIndex];                       
    }

    void ReturnObstacleToPool(GameObject obj, GameObject prefab)
    {
        // 비활성화
        obj.SetActive(false);
        // 풀에 다시 넣기
        obstaclePools[prefab].Enqueue(obj);                        
    }

    GameObject FindOriginalPrefab(GameObject instance)
    {
        // 모든 프리팹 검사
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            GameObject prefab = obstaclePrefabs[i];
            // 인스턴스 이름이 프리팹 이름으로 시작하면
            if (instance.name.StartsWith(prefab.name))
                // 해당 프리팹 반환
                return prefab;
        }
        // 못 찾으면 첫 번째 프리팹 반환 (안전장치)
        return obstaclePrefabs[0];
    }

    void Shuffle<T>(List<T> list)
    {
        // 리스트 전체 돌면서
        for (int i = 0; i < list.Count; i++)
        {
            // i 이상 랜덤 인덱스 선택
            int rand = Random.Range(i, list.Count);
            // 값 교환
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
