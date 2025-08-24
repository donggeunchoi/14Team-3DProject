using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraSway : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("좌우 위치 이동 진폭(미터)")]
    public float positionAmplitude = 0.6f;
    [Tooltip("좌우 회전(요) 진폭(도)")]
    public float yawAmplitude = 5f;
    [Tooltip("초당 왕복 빈도(Hz). 0.1이면 10초에 한 왕복")]
    public float frequency = 0.1f;

    [Tooltip("위치 스웨이 사용 여부")]
    public bool usePosition = true;
    [Tooltip("회전 스웨이(요) 사용 여부")]
    public bool useYaw = true;

    Vector3 _startPos;
    Quaternion _startRot;
    float _t0;

    void Awake()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
        _t0 = Time.unscaledTime;   // timeScale=0에서도 진행
    }

    void LateUpdate()
    {
        // 사인파 시간값 (언스케일드)
        float w = (Time.unscaledTime - _t0) * Mathf.PI * 2f * frequency;
        if (usePosition)
        {
            float x = Mathf.Sin(w) * positionAmplitude;
            // 현재 카메라의 오른쪽 방향 기준으로 좌우 이동
            transform.position = _startPos + transform.right * x;
        }
        if (useYaw)
        {
            float yaw = Mathf.Sin(w) * yawAmplitude;
            transform.rotation = _startRot * Quaternion.Euler(0f, yaw, 0f);
        }
    }

    // 시작 버튼 누를 때 호출하면 스웨이 정지/초기화
    public void StopSway(bool reset = true)
    {
        enabled = false;
        if (reset)
        {
            transform.position = _startPos;
            transform.rotation = _startRot;
        }
    }
}
