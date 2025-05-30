using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    private bool isInvincible = false; // 무적상태 확인

    public bool IsInvincible // 외부에서 무적 상태인지 확인할 수 있도록 함
    {
        get { return isInvincible; }
        private set { isInvincible = value; }
    }

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible) // 무적일 때
        {
            IsInvincible = true; // 무적상태
            // InvincibilityCoroutine 발동
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        // duration(무적 시간)동안 기다림
        yield return new WaitForSeconds(duration);
        IsInvincible = false; // 무적 해제
    }
}