using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraSway : MonoBehaviour
{
    [Tooltip("좌우 이동 진폭(미터)")] public float posAmp = 0.6f;
    [Tooltip("좌우 회전(도)")]       public float yawAmp = 5f;
    [Tooltip("왕복 주기(초)")]       public float period = 10f;

    Vector3 basePos;
    Quaternion baseRot;
    float t0;

    void Awake()
    {
        basePos = transform.position;
        baseRot = transform.rotation;
        t0 = Time.unscaledTime; // pause에도 진행
    }

    void LateUpdate()
    {
        float w = (Time.unscaledTime - t0) * (Mathf.PI * 2f) / Mathf.Max(0.001f, period);
        float s = Mathf.Sin(w);

        // 좌우 이동(카메라 오른쪽 축 기준)
        transform.position = basePos + transform.right * (s * posAmp);
        // Yaw 회전
        transform.rotation = baseRot * Quaternion.Euler(0f, s * yawAmp, 0f);
    }

    public void StopSway(bool reset = true)
    {
        enabled = false;
        if (reset) { transform.position = basePos; transform.rotation = baseRot; }
    }
}

