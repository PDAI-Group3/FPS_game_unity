using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public static string joinCode;
    public void LoadScene(string sceneName)
    {
        switch(sceneName) {
            case "MainMenu":
                SceneManager.LoadScene(sceneName);
                break;
            case "SPMenu":
                SceneManager.LoadScene(sceneName);
                break;
            case "MPMenu":
                SceneManager.LoadScene(sceneName);
                break;
            case "SettingsMenu":
                SceneManager.LoadScene(sceneName);
                break;
            case "SPGameScene":
                SceneManager.LoadScene(sceneName);
                break;
            case "SampleScene":
                SceneManager.LoadScene(sceneName);
                break;
            case "Host":
                SceneManager.LoadScene("MultiplayerScene");
                HostAndJoin.HostGame(4);
                break;
            case "Client":
                SceneManager.LoadScene("MultiplayerScene");
                HostAndJoin.JoinGame(joinCode);
                break;
        }
    }
}