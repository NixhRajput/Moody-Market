using UnityEngine;

public class SceneBack : MonoBehaviour
{
    public string menuSceneName = "MainMenu";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            IrisTransition.Instance.GoToScene(menuSceneName);
    }
}