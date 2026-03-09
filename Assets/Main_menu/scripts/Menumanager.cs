using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Scene name to load on Start")]
    public string gameSceneName = "SampleScene";

    [Header("Toggle cross line objects")]
    public GameObject musicCrossLine;
    public GameObject soundCrossLine;

    private bool musicOn = true;
    private bool soundOn = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (musicCrossLine != null) musicCrossLine.SetActive(false);
        if (soundCrossLine != null) soundCrossLine.SetActive(false);
    }

    public void OnButtonClicked(string buttonId)
    {
        switch (buttonId)
        {
            case "start":
                SceneManager.LoadScene(gameSceneName);
                break;

            case "music":
                musicOn = !musicOn;
                if (musicCrossLine != null) musicCrossLine.SetActive(!musicOn);
                AudioListener.pause = !musicOn;
                Debug.Log($"[Menu] Music: {(musicOn ? "ON" : "OFF")}");
                break;

            case "sound":
                soundOn = !soundOn;
                if (soundCrossLine != null) soundCrossLine.SetActive(!soundOn);
                AudioListener.volume = soundOn ? 1f : 0f;
                Debug.Log($"[Menu] Sound: {(soundOn ? "ON" : "OFF")}");
                break;

            case "credit":
                Debug.Log("[Menu] Credits clicked - implement later");
                break;

            case "howtoplay":
                Debug.Log("[Menu] How To Play clicked - implement later");
                break;

            case "continue":
                Debug.Log("[Menu] Continue clicked - implement later");
                break;
        }
    }
}