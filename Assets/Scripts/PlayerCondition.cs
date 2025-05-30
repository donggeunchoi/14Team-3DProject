using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public InvincibleEffect invincibleEffect; // InvincibleEffect스크립트 인스펙터에서 할당
    private bool isInvincible = false; // 무적상태 확인

    public bool IsInvincible // 외부에서 무적 상태인지 확인할 수 있도록 함
    {
        get { return isInvincible; }
        private set { isInvincible = value; }
    }

    // 무적 메서드
    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible) // 무적일 때
        {
            IsInvincible = true; // 무적상태
            // 무적 코루틴 시작
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }

    /// <summary>
    /// 무적활성화 코루틴, durataion에 따라 효과가 지속됨
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator InvincibilityCoroutine(float duration)
    {
        if (invincibleEffect != null)
        {
            invincibleEffect.StartInvincible(); // 무적 효과 시작
        }

        // duration만큼 대기
        yield return new WaitForSeconds(duration);

        if (invincibleEffect != null)
        {
            invincibleEffect.EndInvincible(); // 무적 효과 종료
        }

        IsInvincible = false; // 무적 해제
    }
}