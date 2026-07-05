using UnityEngine;

public class NavBar : MonoBehaviour
{
    // Wire each nav button's OnClick to this, typing the destination scene name.
    public void GoTo(string sceneName)
    {
        if (SceneLoader.I != null) SceneLoader.I.Load(sceneName);
    }
}
