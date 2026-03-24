using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{ 
    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
