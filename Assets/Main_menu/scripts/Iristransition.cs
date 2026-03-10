using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class IrisTransition : MonoBehaviour
{
    public static IrisTransition Instance;

    [Header("Iris Setup")]
    public Transform irisCircle;
    public float transitionDuration = 1f;
    public AnimationCurve irisCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float maxScale = 30f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartCoroutine(IrisOpen());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(IrisOpen());
    }

    public void GoToScene(string sceneName)
    {
        StartCoroutine(IrisCloseAndLoad(sceneName));
    }

    IEnumerator IrisOpen()
    {
        irisCircle.localScale = Vector3.zero;
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = irisCurve.Evaluate(elapsed / transitionDuration);
            float scale = Mathf.Lerp(0f, maxScale, t);
            irisCircle.localScale = Vector3.one * scale;
            yield return null;
        }
        irisCircle.localScale = Vector3.one * maxScale;
    }

    IEnumerator IrisCloseAndLoad(string sceneName)
    {
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = irisCurve.Evaluate(elapsed / transitionDuration);
            float scale = Mathf.Lerp(maxScale, 0f, t);
            irisCircle.localScale = Vector3.one * scale;
            yield return null;
        }
        irisCircle.localScale = Vector3.zero;
        SceneManager.LoadScene(sceneName);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}