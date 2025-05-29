# 🎮 Commute from Hell
장애물을 피하며 점수를 획득하는 간단하고 직관적인 액션 게임입니다.


도시에서 나타나는 장애물을 피하며 생존하는 캐쥬얼 액션 게임
회사원이 늦잠을 자버려 지각하지 않으려고 온갖 장애물들을 피해 출근하는 게임입니다.




## 🧾 프로젝트 소개


### 장르: 
캐주얼 액션

### 기능: 
점프, 충돌 판정, UI 조작, 게임 상태 관리, 오디오

### 목표: 
Unity의 기초 기능을 활용한 게임 로직 및 UI 연동 학습

## 🛠 사용한 기술 및 시스템


### ✅ Unity 기반 시스템 구성
시스템 구성 요소	사용 기술 / 설명

### 플레이어 (Player)	

1. 이동 


A D - 좌, 우 이동


Space - 점프


S - 구르기



2. 애니메이션


이동 애니메이션


점프 애니메이션


구르기 애니메이션


충돌 애니메이션 구현


### 장애물 (Obstacle)	

1. 장애물 종류

점프 장애물
슬라이딩 장애물


2. 움직임

velocity.z 값만 이동


3. 충돌

OnCollisionEnter 와 CompareTag를 통해 구현


### UI (사용자 인터페이스)	

1. 게임 시작


2. 게임 진행 중

   
시간, 점수, 콤보 관리


3. 게임 중지

   
볼륨 조절 관리


4. 게임 오버 & 다시 시작


5. 버튼 관리


### 오디오 (Audio)	
1. AudioSource 사용 


2. BGM 및 효과음 


3. 볼륨 조절 기능


#### 맵 (Map / 배경)
1. 무한 스크롤 배경 


2. 에셋 저작권확인


3. 씬 전환 기반 맵 구조화


## 🧩 트러블 슈팅 (문제 해결 경험)


### 1. UI 버튼이 2회차부터 작동하지 않는 문제
원인: DontDestroyOnLoad로 인해 GameManager와 UIManager가 씬 전환 시 중복 생성됨

해결: RestartGame() 내에서 기존 매니저 객체 수동 파괴 → 재생성 유도

if (UIManager.Instance != null) 


    Destroy(UIManager.Instance.gameObject);

    
if (GameManager.Instance != null && GameManager.Instance != this) 


    Destroy(GameManager.Instance.gameObject);

    
    
SceneManager.LoadScene("Map_Asset");


### 2. 게임 오버 시 Player의 Dead Animation이 실행되지 않음.
문제 : 코루틴으로 종료 자체를 애니메이션 종료 이후 시간으로 넘겼을 경우 그 시간 사이에 또 다른 장애물과 부딪히면 종료UI가 실행되지않음


원인 : Time.timeScale = 0으로 만들기 때문에 애니메이션이 실행되는 시점에 time이 0이되어 실행되는 모습이 보이지 않음


해결법 : Animation Inspector의 UpdateMode를 UnscaledTime으로 설정하여 Time.timeScale의 영향을 받지 않게 하고 Coroutine의 WaitForSecondsRealtime으로 구현



### 3. 장애물이 이동 중 잠깐 멈추거나 완전히 멈춰 버리는 현상
원인 - 바닥 콜라이더와 장애물 콜라이더가 충돌하고 있었습니다

해결 - Layer Collision Matrix 에서 바닥 레이어와 장애물 레이어의 충돌을 꺼주는 것으로 해결

## 👤 만든이
이름	역할

최동근 UI/게임매니저 구조 설계, 트러블슈팅 정리,리드미 작성
<https://github.com/donggeunchoi>


윤지민 Player,Animation,Camera
<https://github.com/jjmm-del>


정광훈 장애물, 게임오버, 점수시스템
<https://github.com/JKH201020>


정재훈 배경음악, 효과음, 오디오 전반
<https://github.com/BEEZZLE>


김하빈 Map, 전반적인 전반적 코딩기술 담당
<https://github.com/Habin-Kim>

## ✅ 실행 방법
Unity 프로젝트 열기

Map_Asset 씬 실행

UI에서 Start → 조작 시작

장애물을 피하고 점수 획득

게임 오버 후 Restart로 반복 플레이

## 📌 참고사항
Unity 2021.3.17 이상 권장

TextMeshPro, Unity 기본 패키지 사용

모바일 확장 또는 난이도 조절 기능은 추후 구현 고려

