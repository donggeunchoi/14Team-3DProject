🎮 Unity 액션 게임 프로젝트
장애물을 피하며 점수를 획득하는 간단하고 직관적인 액션 게임입니다.
플레이어는 조작을 통해 다양한 장애물을 회피하고, 콤보와 점수를 쌓아 최고 기록에 도전합니다!

🧾 프로젝트 소개
#장르: 
캐주얼 액션

기능: 
점프, 충돌 판정, UI 조작, 게임 상태 관리

목표: 
Unity의 기초 기능을 활용한 게임 로직 및 UI 연동 학습

🛠 사용한 기술 및 시스템
✅ Unity 기반 시스템 구성
시스템 구성 요소	사용 기술 / 설명
플레이어 (Player)	Rigidbody2D + Collider2D / 키 입력 제어 / 방향 반전 처리
장애물 (Obstacle)	스폰 시스템 + 충돌 감지 / 자동 이동 / 재사용을 위한 오브젝트 풀 고려
UI (사용자 인터페이스)	TextMeshPro + Canvas / 점수, 콤보, 정지·재시작 버튼 등 구현
오디오 (Audio)	AudioSource 사용 / BGM 및 효과음 / 볼륨 조절 기능
맵 (Map / 배경)	무한 스크롤 배경 / 패럴럭스 효과 / 씬 전환 기반 맵 구조화

🧩 트러블 슈팅 (문제 해결 경험)
1. UI 버튼이 2회차부터 작동하지 않는 문제
원인: DontDestroyOnLoad로 인해 GameManager와 UIManager가 씬 전환 시 중복 생성됨

해결: RestartGame() 내에서 기존 매니저 객체 수동 파괴 → 재생성 유도

if (UIManager.Instance != null) Destroy(UIManager.Instance.gameObject);
if (GameManager.Instance != null && GameManager.Instance != this) Destroy(GameManager.Instance.gameObject);
SceneManager.LoadScene("Map_Asset");
2. 점수 및 콤보가 씬 리로드 후 초기화되지 않음
원인: UIManager의 ResetUI()가 실행되었으나 실제 필드 값 초기화 누락

해결: ResetUI() 내 score, combo 초기화 및 Update 호출 확실히 처리

3. 게임 재시작 시 UI, 상태관리, 버튼 재연결이 꼬이는 문제
원인: GameManager/Singleton 패턴으로 인한 중복, 타이밍 꼬임

해결: 싱글톤 유지 최소화, 필요한 경우 DontDestroyOnLoad 제거 후 재등록

👤 만든이
이름	역할
최동근	UI/게임매니저 구조 설계, 트러블슈팅 정리,리드미 작성
윤지민 Player,Animation,Camera
정광훈 장애물, 게임오버, 점수시스템
정재훈 배경음악, 효과음, 오디오 전반
김하빈 Map, 전반적인 전반적 코딩기술 담당

✅ 실행 방법
Unity 프로젝트 열기

Map_Asset 씬 실행

UI에서 Start → 조작 시작

장애물을 피하고 점수 획득

게임 오버 후 Restart로 반복 플레이

📌 참고사항
Unity 2021.x 이상 권장

TextMeshPro, Unity 기본 패키지 사용

모바일 확장 또는 난이도 조절 기능은 추후 구현 고려

