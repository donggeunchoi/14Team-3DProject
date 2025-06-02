using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 아이템을 먹든 안 먹든간에 다시 소환시킬거임 - 아이템을 재사용(오브젝트 풀링)하는 새로운 방법
// 실질적인 아이템의 개수는 하나임
public class ItemSpawner : MonoBehaviour
{
    private Queue<GameObject> _itemPool = new Queue<GameObject>(); // 비활성화 아이템 보관하는 큐
    private Queue<GameObject> _activeItems = new Queue<GameObject>(); // 활성화 아이템 보관하는 큐

    // 비활성화 된 아이템을 담는 리스트
    [SerializeField] private List<GameObject> itemPrefabs;

    [Header("아이템 소환 설정")] [SerializeField] private Transform playerTransform; // 플레이어 위치
    [SerializeField] private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f }; // 3개의 라인
    [SerializeField] private int initialPoolSize = 1; // 미리 생성할 아이템 수

    // 플레이어보다 얼마나 앞서서 아이템이 생성될지 시작 거리
    [SerializeField] private float spawnZStartOffset = 50f;

    // 생성될 아이템들 간의 Z축 간격
    [SerializeField] private float spawnIntervalZ = 200f;

    [Header("겹침 방지")] [SerializeField] private float overlapCheckRadius = 2.0f; // 겹침 검사 시 사용할 반경
    [SerializeField] private LayerMask obstacleLayer;  // 장애물 오브젝트들의 레이어 마스크
    [SerializeField] private int maxSpawnAttempts = 5;  // 유효한 위치를 찾기 위한 최대 시도 횟수

    private float respawnDelay = 15f; // 아이템을 먹으면 15초 뒤에 재생성
    // 다음 아이템 스폰을 기다리는 코루틴이 현재 실행 중인지 확인
    private bool isRespawning = false;
    
    public static ItemSpawner Instance { get; private set; } // 싱글톤

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 미리 일정 개수의 아이템을 만들어 놓고 필요할 때마다 꺼내쓰고,
        // 쓴 후에는 다시 풀에 넣어 재활용하는 방식
        for (int i = 0; i < initialPoolSize; i++)
        {
            // GetRandomItemPrefab()을 통해 무작위 아이템을 가져와서 생성
            // transform: 생성된 아이템의 부모(Parent)를 현재 스크립트가 붙어있는 GameObject로 설정
            GameObject item = Instantiate(GetRandomItemPrefab(), transform);

            item.SetActive(false); // 아이템 비활성화 상태로 시작
            _itemPool.Enqueue(item); // 생성된 아이템을 itemPool 큐에 추가 (재활용 대기 상태)
        }
    }

    void Start()
    {
        // 게임 시작 시, 화면에 보일 초기 아이템들을 배치
        // 초기에 한 개만 소환 - 너무 자주 나와도 안 됨
        for (int i = 0; i < 1; i++)
        {
            // 메서드가 한 번만 호출되어 아이템이 단 한 개만 초기 스폰함
            SpawnNextItem();
        }

        // 아이템 재활용 코루틴을 시작
        // 이 코루틴은 게임이 끝날 때까지 계속 실행
        StartCoroutine(RecycItemCoroutine());
    }

    // 아이템 목록에서 무작위로 아이템 하나를 선택하여 반환
    private GameObject GetRandomItemPrefab()
    {
        // itemPrefabs리스트에 아무것도 할당이 되지 않았거나(인스펙터 창에서),
        // 리스트에 아아템이 추가되지 않은 경우(코드적으로)
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            return null; // 에러가 나지 않게 방어코드
        }

        // 여러 아이템 중에서 무작위로 하나를 선택하여 반환
        return itemPrefabs[Random.Range(0, itemPrefabs.Count)];
    }

    // 오브젝트 풀에서 아이템을 가져와서 적절한 위치에 배치하고 활성화하는 역할
    private void SpawnNextItem()
    {
        GameObject itemToSpawn;

        if (_itemPool.Count == 0)
        {
            // ItemSpawner 오브젝트의 자식으로 설정해서 Hierarchy를 깔끔하게 유지하는 역할
            itemToSpawn = Instantiate(GetRandomItemPrefab(), transform);
            itemToSpawn.SetActive(false); // 새로 생성된 아이템은 일단 비활성화 상태로 시작
        }
        else
        {
            itemToSpawn = _itemPool.Dequeue(); // 풀에서 아이템 꺼냄
        }

        Vector3 spawnPosition = Vector3.zero; // 아이템이 스폰될 위치
        bool positionFound = false; // 유효한 스폰 위치를 찾았는지 여부
        
        // 겹침 검사를 통한 유효한 위치 찾기
        // maxSpawnAttempts 횟수만큼 유효한 위치를 찾을 때까지 시도
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            float spawnX = spawnXPosition[Random.Range(0, spawnXPosition.Length)]; // x축 랜덤 선택
            // 플레이어의 현재 Z 위치를 기준으로 아이템의 Z 위치를 계산
            // spawnZStartOffset: 플레이어로부터 아이템이 생성될 최소 앞쪽 거리
            // activeItems.Count * spawnIntervalZ는 현재 활성 아이템 개수에 따라 아이템 간 간격을 두어 순차적으로 배치
            float currentZ = playerTransform.position.z + spawnZStartOffset + (_activeItems.Count * spawnIntervalZ);

            // 유효한 아이템 스폰 위치
            spawnPosition = new Vector3(spawnX, itemToSpawn.transform.position.y, currentZ);

            // 해당 위치에 장애물(obstacleLayer로 지정된 레이어)이 겹치는지 검사
            // Physics.OverlapSphere: 3D 환경에서 특정 위치(spawnPosition)와 반경(overlapCheckRadius) 내에 있는 모든 콜라이더를 감지
            Collider[] hitColliders = Physics.OverlapSphere(spawnPosition, overlapCheckRadius, obstacleLayer);

            // 겹치는 콜라이더가 없다면, 이 소환 위치는 유효
            if (hitColliders.Length == 0)
            {
                positionFound = true;  // 유효한 위치를 찾음
                break; // 유효한 위치찾기 반복문 종료
            }
        }
        
        // 유효한 위치를 찾았다면 아이템을 해당 위치에 배치하고 활성화
        if (positionFound)
        {
            Debug.Log("아이템 재배치");
            
            itemToSpawn.transform.position = spawnPosition; // 아이템 스폰 위치
            itemToSpawn.SetActive(true); // 아이템 활성화
            _activeItems.Enqueue(itemToSpawn); // 활성화된 아이템 목록에 등록
        }
        else
        {
            // 유효한 위치를 찾지 못했을 경우 경고 메시지 출력
            Debug.LogWarning(
                $"{maxSpawnAttempts} 최대 시도 횟수 초과. 아이템 생성 불가.");

            // // 아이템을 활성화하지 못했으므로 다시 풀에 반환하거나 비활성화 상태로 유지
            // 혹시 활성화된 상태였다면 비활성화
            if (itemToSpawn.activeSelf)
            {
                itemToSpawn.SetActive(false);
            }

            _itemPool.Enqueue(itemToSpawn); // 사용하지 못했으니 다시 풀에 넣음
            
            isRespawning = false;
        }
    }

    // 아이템 재사용 관리하는 코루틴
    private IEnumerator RecycItemCoroutine()
    {
        while (true)
        {
            if (_activeItems.Count == 0) // 활성화 된 아이템 없으면
            {
                yield return null; // 한 프레임 대기
                continue;
            }

            GameObject frontItem = _activeItems.Peek(); // 큐의 가장 앞에 있는 아이템 참조

            if (frontItem == null)
            {
                _activeItems.Dequeue(); // null 참조를 큐에서 제거
                SpawnNextItem(); // 새로운 아이템 생성

                yield return null;
                continue;
            }

            // 아이템이 플레이어보다 뒤로 갔는지
            if (frontItem.transform.position.z < playerTransform.position.z - spawnZStartOffset)
            {
                ReturnItemToPool(_activeItems.Dequeue()); // 아이템 제거하고 풀로 반환
            }

            yield return null;
        }
    }

    // 메서드가 호출될 때마다 새로운 아이템을 스폰합니다.
    public void ReturnItemToPool(GameObject item)
    {
        if (item == null) return; // 이미 파괴된 오브젝트는 처리하지 않음

        item.SetActive(false); // 아이템 비활성화
        _itemPool.Enqueue(item); // 아이템 다시 큐에 넣음
        item.transform.parent = transform; // 부모 오브젝트에 아이템 소환하여 hierarchy 정리


        // 이미 재생성 대기 중인 코루틴이 있다면 중복 실행을 방지
        if (!isRespawning)
        {
            StartCoroutine(RespawnItemDelay(respawnDelay)); // 새로운 코루틴 시작
        }
    }

    // 아이템 소환 위치를 당기는 대신 너무 자주 나와도 안 되니 아이템 소환 딜레이를 만듦
    private IEnumerator RespawnItemDelay(float delay)
    {
        isRespawning = true; // 재생성 대기 중
        
        yield return new WaitForSeconds(delay); // 아이템 스폰 대기 시간
        
        SpawnNextItem(); // 다음 아이템 소환
        isRespawning = false; // 재생성 대기 상태 해제 
    }
}