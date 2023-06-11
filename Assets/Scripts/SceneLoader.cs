using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneName;
    public void Start()
    {
        if(GameManager.Instance.endShopping)
        {
            GameManager.Instance.GameStart();
        }
    }
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}