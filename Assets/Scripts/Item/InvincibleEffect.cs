using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleEffect : MonoBehaviour
{
    public Renderer characterRenderer; // 캐릭터 모델의 Renderer 컴포넌트를 연결
    public Color color = Color.yellow;
    public float intensity = 1.0f; // 발광 강도 (인스펙터에서 조절 가능)
    public float duration = 0.5f; // 색상 / 강도가 변하는 시간

    private Color originalEmissionColor;
    private bool isInvincible = false;
    private Material[] materials; // 여러 개의 재질이 있을 수 있으므로 배열로 처리

    void Start()
    {
        if (characterRenderer == null)
        {
            Debug.LogError("InvincibleEffect: Character Renderer가 할당되지 않았습니다. 인스펙터에서 연결해주세요.", this);
            enabled = false;
            return;
        }

        materials = characterRenderer.materials;

        if (materials.Length == 0)
        {
            Debug.LogWarning("InvincibleEffect: 캐릭터에 할당된 재질이 없습니다.", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < materials.Length; i++)
        {
            Material mat = materials[i];

            if (mat.HasProperty("_EmissionColor"))
            {
                if (i == 0)
                {
                    originalEmissionColor = mat.GetColor("_EmissionColor");
                }

                mat.SetColor("_EmissionColor", Color.black);
                mat.DisableKeyword("_EMISSION");
            }
            else
            {
                Debug.LogWarning(
                    $"InvincibleEffect: 재질 '{mat.name}'의 쉐이더 '{mat.shader.name}'에는 '_EmissionColor' 속성이 없습니다. Standard 쉐이더인지 확인해주세요. (인덱스: {i})",
                    this);
            }
        }
    }

    public void StartInvincible() // 무적 상태 시작
    {
        if (isInvincible) return;
        isInvincible = true;

        if (materials == null || materials.Length == 0) return;
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                StartCoroutine(FadeEmission(mat, mat.GetColor("_EmissionColor"), color * intensity, duration));
            }
        }
    }

    public void EndInvincible() // 무적 상태 종료
    {
        if (!isInvincible) return;
        isInvincible = false;

        if (materials == null || materials.Length == 0) return;
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                StartCoroutine(FadeEmission(mat, mat.GetColor("_EmissionColor"), originalEmissionColor, duration));
            }
        }
    }

    IEnumerator FadeEmission(Material mat, Color startColor, Color endColor, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            Color currentColor = Color.Lerp(startColor, endColor, timer / duration);
            mat.SetColor("_EmissionColor", currentColor);
            mat.EnableKeyword("_EMISSION");
            timer += Time.deltaTime;
            yield return null;
        }

        mat.SetColor("_EmissionColor", endColor);

        if (!isInvincible && endColor == originalEmissionColor)
        {
            mat.DisableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", Color.black);
        }
    }
}