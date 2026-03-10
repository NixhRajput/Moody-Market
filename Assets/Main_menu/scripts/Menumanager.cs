using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Scene name to load on Start")]
    public string gameSceneName = "SampleScene";
    public string creditsSceneName = "Credits";
    public string howToPlaySceneName = "HowToPlay";

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
        if (AudioManager.Instance != null)
        {
            musicOn = AudioManager.Instance.IsMusicOn();
            soundOn = AudioManager.Instance.IsSoundOn();
        }

        if (musicCrossLine != null) musicCrossLine.SetActive(!musicOn);
        if (soundCrossLine != null) soundCrossLine.SetActive(!soundOn);
    }

    public void OnButtonClicked(string buttonId)
    {
        switch (buttonId)
        {
            case "start":
                IrisTransition.Instance.GoToScene(gameSceneName);
                break;

            case "music":
                musicOn = !musicOn;
                if (musicCrossLine != null) musicCrossLine.SetActive(!musicOn);
                if (AudioManager.Instance != null) AudioManager.Instance.SetMusic(musicOn);
                break;

            case "sound":
                soundOn = !soundOn;
                if (soundCrossLine != null) soundCrossLine.SetActive(!soundOn);
                if (AudioManager.Instance != null) AudioManager.Instance.SetSound(soundOn);
                break;

            case "credit":
                IrisTransition.Instance.GoToScene(creditsSceneName);
                break;

            case "howtoplay":
                IrisTransition.Instance.GoToScene(howToPlaySceneName);
                break;

            case "continue":
                Debug.Log("[Menu] Continue clicked - implement later");
                break;
        }
    }
}