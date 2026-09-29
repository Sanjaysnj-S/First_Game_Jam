using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadScene("Level_1");
        Time.timeScale = 1f;
    }
}
