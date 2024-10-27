using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextFade : MonoBehaviour
{

    public Text text;
    public float fadeInDuration = 0f;
    public float visibleDuration = 0f;
    public float fadeOutDuration = 0f;
    public float invisibleDuration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeText());
    }



    private IEnumerator FadeText()
    {
        while (true)
        {
        yield return Fade(0f, 1f, fadeInDuration);

        yield return new WaitForSeconds(visibleDuration);

        yield return Fade(1f, 0f, fadeOutDuration);

        yield return new WaitForSeconds(invisibleDuration);
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = text.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            text.color = color;
            yield return null;
        }
    }

}
