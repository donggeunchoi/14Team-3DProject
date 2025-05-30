using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    [SerializeField] private InvincibleEffect _invincibleEffect; // InvincibleEffect스크립트 인스펙터에서 할당
    private bool isInvincible = false; // 무적상태 확인
    private Item _item;
    
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

        if (_invincibleEffect != null)
        {
            _invincibleEffect.StartInvincible(); // 무적 효과 시작
        }

        // 실제 무적 시간보다 2초 먼저 종료 시키고 무적 이펙트를 먼저 끔
        // 무적이 끝나고 2초 후에 이펙트가 꺼지기 때문에 무적 해제를 알아보기가 힘들음
        yield return new WaitForSeconds(duration - 2);

        if (_invincibleEffect != null)
        {
            _invincibleEffect.EndInvincible(); // 무적 효과 종료
        }

        // 2초 후 무적 종료
        yield return new WaitForSeconds(2);
        
        IsInvincible = false; // 무적 해제
    }
}