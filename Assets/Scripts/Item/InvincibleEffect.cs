using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 모델링의 색상을 바꾸는 스크립트 (무적 상태일 때)
public class InvincibleEffect : MonoBehaviour
{
    [Header("무적 시각 효과 설정")] public Renderer characterRenderer; // 캐릭터 모델의 Renderer 컴포넌트를 연결
    public float intensity = 1.0f; // 발광 강도 (인스펙터에서 조절 가능)
    public float duration = 0.5f; // 색상 / 강도가 변하는 시간

    private bool _isInvincible = false; // 무적 상태 인지 확인
    private Material[] _materials; // 여러 개의 material로 모델링이 구성되어 있어서 배열로 함

    // 각 material별 원래 Emission 색상 및 키워드 상태를 저장할 Dictionary
    private Dictionary<Material, Color> _initEmsissions;

    // 원래의 발광 색상 저장 및 초기화
    void Start()
    {
        // 이 스크립트에서 캐릭터에 적용된 모든 material을 가져와 materials배열에 저장
        _materials = characterRenderer.materials;
        _initEmsissions = new Dictionary<Material, Color>(); // Dictionary 초기화

        // material을 하나씩 가져옴
        for (int i = 0; i < _materials.Length; i++)
        {
            Material mat = _materials[i]; // 현재 [i]번째의 material을 mat에 할당

            // 현재 material이 "_EmissionColor"속성을 가지고 있는다면
            if (mat.HasProperty("_EmissionColor"))
            {
                // 원래 Emission 색상을 저장
                _initEmsissions[mat] = mat.GetColor("_EmissionColor");
                
                mat.SetColor("_EmissionColor", Color.black); // Emission 색상을 검은색(기본값)으로 설정
                mat.DisableKeyword("_EMISSION"); // Emission 키워드(발광 기능) 비활성화

                // _EMISSION: 유니티 Standard 쉐이더에서 발광 기능을 활성화/비활성화하는 데 사용되는 쉐이더 키워드
                // 인스펙터에서 Emission 체크박스를 켜고 끄는 것과 동일한 역할을 코드로 수행
            }
        }
    }

    // 캐릭터가 현재 무적 상태가 아니라면, 무적 상태로 전환하고
    // material에 발광 효과를 부드럽게 페이드 인(서서히 밝아지는)시키는 코루틴을 시작
    public void StartInvincible() // 무적 상태 시작
    {
        // 현재 무적 상태인지 확인 (중복 실행 방지)
        // 무적이면 메서드 종료
        if (_isInvincible) return;
        _isInvincible = true; // 무적으로 전환

        // materials 배열이 유효한지 확인 (null이거나 비어있는 경우를 대비한 안전 장치)
        if (_materials == null || _materials.Length == 0) return;

        // materials 배열에 있는 모든 재질을 하나씩 순회
        foreach (Material mat in _materials)
        {
            // 현재 material이 쉐이더가 발광(Emission) 속성(_EmissionColor)을 지원하는지 다시 확인
            if (mat.HasProperty("_EmissionColor"))
            {
                Color selectedEmissionColor = Color.black; // 기본값은 검은색

                // 각 material의 원래(인스펙터에서 설정된) Emission 색상을 가져옴
                if (_initEmsissions.ContainsKey(mat))
                {
                    selectedEmissionColor = _initEmsissions[mat];
                }

                // 발광 효과를 부드럽게 페이드 인(서서히 밝아지는) 시키는 코루틴 시작
                StartCoroutine(FadeEmission(mat, mat.GetColor("_EmissionColor"), selectedEmissionColor * intensity,
                    duration, true));
            }
        }
    }

    // 캐릭터가 현재 무적 상태라면, 무적 상태를 해제하고
    // material의 발광 효과를 원래 색상으로 부드럽게 페이드 아웃(서서히 어두워지는)시키는 코루틴을 시작
    public void EndInvincible() // 무적 상태 종료
    {
        // 현재 무적 상태인지 확인 (중복 실행 방지)
        // 무적이 아니면 메서드 종료
        if (!_isInvincible) return;
        _isInvincible = false; // 무적 해제

        // materials 배열이 유효한지 확인 (null이거나 비어있는 경우를 대비한 안전 장치)
        if (_materials == null || _materials.Length == 0) return;

        // 'materials' 배열에 있는 모든 재질을 하나씩 순회
        foreach (Material mat in _materials)
        {
            // 현재 material이 쉐이더가 발광(Emission) 속성(_EmissionColor)을 지원하는지 다시 확인
            if (mat.HasProperty("_EmissionColor"))
            {
                Color originalColor = Color.black; // 기본값
                bool originalKeywordState = false; // 기본값

                // 각 material의 원래 Emission 색상 (Start에서 저장된 것)을 가져옴
                if (_initEmsissions.ContainsKey(mat))
                {
                    originalColor = _initEmsissions[mat];
                }

                // 발광 효과를 부드럽게 페이드 아웃(서서히 어두워지는) 시키는 코루틴 시작
                StartCoroutine(FadeEmission(mat, mat.GetColor("_EmissionColor"), originalColor, duration, false));
            }
        }
    }

    // 발광이 부드럽게 켜지고 꺼지는(페이드 인/아웃) 시각적 변화
    IEnumerator FadeEmission(Material mat, Color startColor, Color endColor, float duration, bool finalKeywordState = true)
    {
        float timer = 0f; // 페이드(서서히 색이 바뀌는) 타이머
        while (timer < duration)
        {
            // 현재 시간에 따른 페이드 진행률을 계산하여 색상 보간
            Color currentColor = Color.Lerp(startColor, endColor, timer / duration);
            // material의 Emission 색상을 currentColor으로 설정
            mat.SetColor("_EmissionColor", currentColor);
            // material의 Emission 키워드 활성화 (계속 발광 상태)
            mat.EnableKeyword("_EMISSION");

            timer += Time.deltaTime; // 프레임 시간만큼 타이머 증가
            yield return null; // 끝나면 다음 프레임까지 대기
        }

        // 무적 활성화 루프가 끝나면 endColor 색상으로 설정
        mat.SetColor("_EmissionColor", endColor);

        // finalKeywordState 적용 (페이드가 끝난 후에만 적용)
        if (finalKeywordState)
        {
            mat.EnableKeyword("_EMISSION"); // 키워드 활성화
        }
        else
        {
            mat.DisableKeyword("_EMISSION"); // 키워드 비활성화

            // 만약 원래 Emission이 없었거나, 효과를 끄는 것이라면 검은색으로 설정
            if (endColor.r + endColor.g + endColor.b == 0) // 색상값이 0이면 (검은색이면)
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}