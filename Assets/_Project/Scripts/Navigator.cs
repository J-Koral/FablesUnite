using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigator : MonoBehaviour
{
    public void GoTo(string sceneName) => SceneManager.LoadScene(sceneName);
}
