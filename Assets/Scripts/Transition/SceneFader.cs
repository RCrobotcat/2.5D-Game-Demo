using System.Collections;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    public float fadeInDuration;
    public float fadeOutDuration;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOutThenIn(float time)
    {
        yield return FadeOut(fadeOutDuration);
        yield return FadeIn(fadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        Destroy(gameObject);
    }
}
