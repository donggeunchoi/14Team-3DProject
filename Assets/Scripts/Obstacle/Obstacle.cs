using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefab; // 장애물 프리팹 넣기
    [SerializeField] private GameObject[] longObstaclePrefab; // 장애물이 한 줄에 길게 나옴

    [Header("장애물 생성 설정")] private float[] spawnXPosition = { -2.6f, 0.1f, 2.6f }; // 장애물 가로 간격
    public float spawnYPosition; // 높이
    public float spawnZPosition; // 화면 뒷부분
    public float minSpawnDelay; // 장애물 소환되는데 걸리는 최소 시간
    public float maxSpawnDelay; // 장애물 소환되는데 걸리는 최대 시간

    [Header("긴 장애물 설정")] public float obstacleCount; // 한 번에 몇 개씩 나오게 할 건지
    public float betweenObstacle; // 길게 장애물이 나올 때 장애물간 간격 (겹치기 방지)
    private float LongSpawnDelay; // 긴 장애물 소환되는데 걸리는 시간


    void Start()
    {
        // 레인 별로 독립적으로 장애물 소환
        for (int i = 0; i < spawnXPosition.Length; i++)
        {
            // 코루틴 무한 반복 
            StartCoroutine(SpawnObstacle(i));
        }
    }

    IEnumerator SpawnObstacle(int laneIdx) // 장애물 소환 (여기서 소환 처리)
    {
        while (true)
        {
            // 장애물 소환되는 시간 간격
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            // 장애물 타입
            float obstacleType = Random.Range(1, 101);

            if (obstacleType <= 70) // 일반 장애물 소환 - 70% 확률로 소환
            {
                yield return StartCoroutine(RandomSpawnObstacle(laneIdx));
            }
            else // 긴 장애물 소환 - 30% 확률로 소환
            {
                yield return StartCoroutine(RandomSpawnLongObstacle(laneIdx));
            }
        }
    }

    IEnumerator RandomSpawnObstacle(int laneIdx) // 랜덤으로 일반 장애물 소환
    {
        // 장애물 모델링 중 하나를 랜덤으로 선택
        int randObstaclePrefabs = Random.Range(0, obstaclePrefab.Length);
        // 위 코드에서 뽑힌 장애물을 선택된 장애물로 지정
        GameObject selectedObstaclePrefab = obstaclePrefab[randObstaclePrefabs];
        // 장애물 생성 x좌표 가져오기 - 레인 별로 독립적으로 장애물 소환
        float spawnX = spawnXPosition[laneIdx];

        // 장애물 소환 위치 - 장애물 모델링이 90도 돌아간 상태여서 인스펙터에서 설정한 값으로 불러옴
        GameObject newObstacle = Instantiate(selectedObstaclePrefab,
            new Vector3(spawnX, spawnYPosition, spawnZPosition), selectedObstaclePrefab.transform.rotation);

        // 장애물 오브젝트들이 ObstacleSpawner 오브젝트의 안에 생성됨
        // 생성된 장애물 오브젝트들이 ObstacleSpawner를 부모 오브젝트로 설정 - hierarchy 창을 정리하는 용도
        newObstacle.transform.parent = this.transform;
        
        yield break; // 코루틴 종료
    }

    IEnumerator RandomSpawnLongObstacle(int laneIdx) // 랜덤으로 긴 장애물 소환
    {
        // 장애물 모델링 중 하나를 랜덤으로 선택
        int randObstaclePrefabs = Random.Range(0, longObstaclePrefab.Length);

        for (int i = 0; i < obstacleCount; i++) // 장애물을 연속으로 생성하는 조건문
        {
            // 위 코드에서 뽑힌 장애물을 선택된 장애물로 지정
            GameObject selectedObstaclePrefabs = longObstaclePrefab[randObstaclePrefabs];
            // 장애물 생성 x좌표 가져오기 - 레인 별로 독립적으로 장애물 소환
            float spawnX = spawnXPosition[laneIdx];

            // 장애물 소환 위치 - 장애물 모델링이 90도 돌아간 상태여서 인스펙터에서 설정한 값으로 불러옴
            GameObject newObstacle = Instantiate(selectedObstaclePrefabs,
                new Vector3(spawnX, spawnYPosition, spawnZPosition), selectedObstaclePrefabs.transform.rotation);

            // 장애물 오브젝트들이 ObstacleSpawner 오브젝트의 안에 생성됨
            // 생성된 장애물 오브젝트들이 ObstacleSpawner를 부모 오브젝트로 설정 - hierarchy 창을 정리하는 용도
            newObstacle.transform.parent = this.transform;

            ObstacleBehavior obstacleMove = newObstacle.GetComponent<ObstacleBehavior>();

            // 소환 시간 = 거리 / 속도 
            float LongSpawnDelay = betweenObstacle / obstacleMove.obstacleSpeed;

            if (i < obstacleCount - 1)
            {
                // 장애물이 하나씩 나오는데 걸리는 시간
                yield return new WaitForSeconds(LongSpawnDelay);
            }
        }
    }
}