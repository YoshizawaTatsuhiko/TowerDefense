using UnityEngine;
using UnityEngine.SceneManagement;

// 日本語対応
public class DebugReload : MonoBehaviour
{
    [SerializeField] private KeyCode _key = KeyCode.None;

    private void Update()
    {
        if (Input.GetKeyDown(_key))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
