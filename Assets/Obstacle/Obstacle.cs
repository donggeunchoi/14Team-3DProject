using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // 장애물 프리팹 넣기

    public float[] spawnXPosition = { -3f, 0f, 3f }; // 장애물 가로 간격
    public float spawnYPosition = 1f; // 높이
    public float spawnZPosition = 50f; // 화면 뒷부분

    public float minSpawnDelay = 1f; // 장애물 소환되는데 걸리는 최소 시간
    public float maxSpawnDelay = 3f; // 장애물 소환되는데 걸리는 최대 시간

    void Start()
    {
        StartCoroutine(RandomSpawnObstacle());
    }

    IEnumerator RandomSpawnObstacle() // 랜덤으로 장애물 소환
    {
        while (true)
        {
            // 다음 장애물 소환하는데 걸리는 시간
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));

            // 3개의 레인 중 하나를 랜덤으로 선택 (0 ~ 2)
            int randomXLaneIndex = Random.Range(0, spawnXPosition.Length);
            // 몇 번째 레인에서 소환될 것인지 정함
            float selectedXLane = spawnXPosition[randomXLaneIndex];

            // 장애물 소환 위치
            GameObject newObstacle = Instantiate(obstaclePrefab,
                new Vector3(selectedXLane, spawnYPosition, spawnZPosition), Quaternion.identity);


            newObstacle.transform.parent = this.transform;
            Destroy(newObstacle, 10f); // 10초 뒤에 장애물 제거
        }
    }
}