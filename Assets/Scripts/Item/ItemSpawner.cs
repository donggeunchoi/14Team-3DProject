using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 아이템을 먹든 안 먹든간에 다시 소환시킬거임 - 아이템을 재사용(오브젝트 풀링)하는 새로운 방법
public class ItemSpawner : MonoBehaviour
{
    private Queue<GameObject> itemPool = new Queue<GameObject>(); // 비활성화 아이템 보관하는 큐
    private Queue<GameObject> activeItems = new Queue<GameObject>(); // 활성화 아이템 보관하는 큐

    // 비활성화 된 아이템을 담는 리스트
    [SerializeField] private List<GameObject> itemPrefabs;

    [Header("아이템 소환 설정")]
    // 플레이어의 위치를 받아옴
    [SerializeField]
    private Transform playerTransform;

    // 3개의 라인
    [SerializeField] private float[] spawnXPosition = new float[] { -2.6f, 0f, 2.6f };

    [SerializeField] private int initialPoolSize = 1; // 미리 생성할 아이템 수

    // 플레이어보다 얼마나 앞서서 아이템이 생성될지 시작 거리
    [SerializeField] private float spawnZStartOffset = 50f;
    [SerializeField] private float spawnIntervalZ = 200f; // 생성될 아이템들 간의 Z축 간격

    public static ItemSpawner Instance { get; private set; } // 싱글톤

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject item = Instantiate(GetRandomItemPrefab(), transform);
            item.SetActive(false); // 비활성화 상태로 시작
            itemPool.Enqueue(item); // 풀 큐에 추가
        }
    }

    void Start()
    {
        // 게임 시작 시, 화면에 보일 초기 아이템들을 배치
        for (int i = 0; i < 1; i++) // 초기에 한 개 소환
        {
            SpawnNextItem();
        }

        // 아이템 재활용 코루틴을 시작 - 이 코루틴은 게임이 끝날 때까지 계속 실행
        StartCoroutine(RecycItemCoroutine());
    }

    // 아이템 프리팹 목록에서 무작위로 하나를 선택하여 반환
    private GameObject GetRandomItemPrefab()
    {
        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            Debug.LogError("[ItemSpawner] Item Prefabs 리스트가 비어있습니다! 프리팹을 할당해주세요.");
            return null;
        }

        return itemPrefabs[Random.Range(0, itemPrefabs.Count)];
    }

    void SpawnNextItem()
    {
        GameObject itemToSpawn;

        if (itemPool.Count == 0)
        {
            Debug.LogWarning("[ItemSpawner] 아이템 풀이 비었습니다. 새로운 아이템을 즉시 생성합니다.");
            itemToSpawn = Instantiate(GetRandomItemPrefab(), transform);
        }
        else
        {
            itemToSpawn = itemPool.Dequeue(); // 풀에서 아이템 꺼냄
        }

        // itemPool.Enqueue(itemPool.Dequeue()); // 풀에서 아이템 하나 꺼냄
        float spawnX = spawnXPosition[Random.Range(0, spawnXPosition.Length)]; // x축 랜덤 선택
        // spawnZStartOffset은 플레이어로부터의 시작 Z 거리
        // activeItems.Count * spawnIntervalZ는 현재 활성 아이템 개수에 따라 아이템 간 간격을 두어 순차적으로 배치
        float currentZ = playerTransform.position.z + spawnZStartOffset + (activeItems.Count * spawnIntervalZ);
        
        itemToSpawn.transform.position =
            new Vector3(spawnX, itemToSpawn.transform.position.y, currentZ);
        itemToSpawn.SetActive(true);

        activeItems.Enqueue(itemToSpawn);
    }

    private IEnumerator RecycItemCoroutine()
    {
        while (true)
        {
            if (activeItems.Count == 0) // 활성화 된 아이템 없으면
            {
                yield return null; // 한 프레임 대기
                continue;
            }

            GameObject frontItem = activeItems.Peek(); // 큐의 가장 앞에 있는 아이템 참조

            if (frontItem == null)
            {
                Debug.LogWarning("[ItemSpawner] 큐의 첫 번째 아이템이 예상치 못하게 파괴되었습니다. 큐에서 제거합니다.");
                activeItems.Dequeue(); // null 참조를 큐에서 제거
                SpawnNextItem(); // 새로운 아이템 생성

                yield return null;
                continue;
            }

            // 플레이어보다 뒤로 갔는지
            if (frontItem.transform.position.z < playerTransform.position.z - spawnZStartOffset)
            {
                ReturnItemToPool(activeItems.Dequeue()); // 아이템 제거하고 풀로 반환
                Debug.Log($"[ItemSpawner] 아이템 재활용 (화면 밖): {frontItem.name}");
            }

            yield return null;
        }
    }
    
    // 메서드가 호출될 때마다 새로운 아이템을 스폰합니다.
    public void ReturnItemToPool(GameObject item)
    {
        if (item == null) return; // 이미 파괴된 오브젝트는 처리하지 않음
    
        // 큐에서 해당 아이템을 찾아 제거하는 로직 (Queue는 중간 제거가 어려움)
        // 획득된 아이템이 큐의 맨 앞 아이템이 아닐 수 있으므로 List로 변환 후 제거
        List<GameObject> tempActiveList = new List<GameObject>(activeItems);
        if (tempActiveList.Remove(item)) // 리스트에서 item 제거 성공 시
        {
            activeItems = new Queue<GameObject>(tempActiveList); // 제거된 리스트로 큐를 재구성
        }
        else
        {
            // activeItems 큐에 없다는 것은 이미 재활용되었거나, 예상치 못한 문제.
            Debug.LogWarning($"[ItemSpawner] 획득된 아이템 {item.name}이 activeItems 큐에 없거나 이미 처리되었습니다.");
        }

        item.SetActive(false); // 아이템 비활성화
        itemPool.Enqueue(item); // 아이템 다시 큐에 넣음
        item.transform.parent = transform; // 부모 오브젝트에 아이템 소환하여 hierarchy 정리

        SpawnNextItem();
    }
}