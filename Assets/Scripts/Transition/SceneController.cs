using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public SceneFader sceneFaderPrefab;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        SceneFader fade = Instantiate(sceneFaderPrefab);
        yield return StartCoroutine(fade.FadeOut(1.5f));
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(fade.FadeIn(1.0f));
        yield break;
    }
}
