using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image cutSceneImage;
    public List<Sprite> cutSceneImages = new List<Sprite>();
    public float displaytime = 2f;
    public float fadeTime = 1f;

   private static CutSceneManager instance;
   public bool isReady = false;

   private void Awake()
   {
       if (instance == null)
       {
           instance = this;
           DontDestroyOnLoad(gameObject);

           isReady = true;
       }
       else
       {
           Destroy(gameObject);
       }
   }
   
    public IEnumerator PlayCutSceneCoroutine()
    {
        yield return new WaitUntil(() => isReady);
        
        yield return StartCoroutine(PlayCutScene());
    }

    IEnumerator PlayCutScene()
    {
        if (cutSceneImage != null)
        {
            cutSceneImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("컷신이미지 없어요");
            yield break;
        }
        
        Debug.Log("CutSceneCoroutine 실행됨!");
        Debug.Log("CutSceneManager 인스턴스 ID: " + this.GetInstanceID());
        Debug.Log("CutSceneImages 리스트 개수: " + cutSceneImages.Count);
        
        foreach (Sprite sprite in cutSceneImages)
        {
            cutSceneImage.sprite = sprite;

            Debug.Log(sprite?.name);
            yield return StartCoroutine(Fade(0f, 1f));
            
            yield return new WaitForSeconds(displaytime);
            
            yield return StartCoroutine(Fade(1f, 0f));
        }
        
        image.gameObject.SetActive(false);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = cutSceneImage.color;
        color.a = startAlpha;
        cutSceneImage.color = color;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeTime);
            color.a = alpha;
            cutSceneImage.color = color;
            yield return null;
        }
        color.a = endAlpha;
        cutSceneImage.color = color;
    }

    public void ShowCutScene()
    {
        if (image != null)
        {
            image.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("배경없는데요?");
        }
    }
    
}