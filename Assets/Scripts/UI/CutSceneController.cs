using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public Image cutSceneImage;
    public List<Sprite> cutSceneImages = new List<Sprite>();
    public float displaytime = 2f;
    public float fadeTime = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayCutScene());
    }

    IEnumerator PlayCutScene()
    {
        foreach (Sprite image in cutSceneImages)
        {
            cutSceneImage.sprite = image;

            yield return StartCoroutine(Fade(0f, 1f));
            
            yield return new WaitForSeconds(displaytime);
            
            yield return StartCoroutine(Fade(1f, 0f));
        }
        
        cutSceneImage.gameObject.SetActive(false);
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
    
}
