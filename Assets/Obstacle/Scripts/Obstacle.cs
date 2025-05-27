using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // 장애물 프리팹 넣기
    [Header("장애물 생성 설정")]
    public float[] spawnXPosition = { -5f, 0f, 5f }; // 장애물 가로 간격
    public float spawnYPosition; // 높이
    public float spawnZPosition; // 화면 뒷부분

    public float minSpawnDelay; // 장애물 소환되는데 걸리는 최소 시간
    public float maxSpawnDelay; // 장애물 소환되는데 걸리는 최대 시간
    
    void Start()
    {
        // 레인 별로 독립적으로 장애물 소환
        for (int i = 0; i < spawnXPosition.Length; i++)
        {
            StartCoroutine(RandomSpawnObstacle(i));  
        }
    }

    IEnumerator RandomSpawnObstacle(int laneIdx) // 랜덤으로 장애물 소환
    {
        while (true)
        {
            // 다음 장애물 소환하는데 걸리는 시간
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            
            // // 3개의 레인 중 하나를 랜덤으로 선택 (0 ~ 2)
            // int randomXLaneIndex = Random.Range(0, spawnXPosition.Length);
            // // 몇 번째 레인에서 소환될 것인지 정함
            // float selectedXLane = spawnXPosition[randomXLaneIndex];
            
            // 장애물 생성 x좌표 가져오기 - 레인 별로 독립적으로 장애물 소환
            float spawnX = spawnXPosition[laneIdx];
            
            // 장애물 소환 위치
            GameObject newObstacle = Instantiate(obstaclePrefab,
                new Vector3(spawnX, spawnYPosition, spawnZPosition), Quaternion.identity);
            
            // 장애물 오브젝트들이 ObstacleSpawner 오브젝트의 안에 생성됨
            // 생성된 장애물 오브젝트들이 ObstacleSpawner를 부모 오브젝트로 설정 - hierarchy 창을 정리하는 용도
            newObstacle.transform.parent = this.transform;
        }
    }
}