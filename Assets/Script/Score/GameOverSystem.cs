using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtTaskName;

    [SerializeField]
    private TMP_Text txtTaskValue;

    [SerializeField]
    private Image imgTaskIcon;

    [SerializeField]
    private GameObject GameOverPanel;

    private IDetectGameOver _detectGame;
    private GridSystem _gridSystem;

    [Inject]
    public void Construct(GridSystem grid)
    {
        _gridSystem = grid;
    }

    void Start()
    {
        SelectTypeGame(); 
        _detectGame.OnGameOver += GameOver;
        _detectGame.OnCount += ChangeCount;

        var desc = _detectGame.Description();
        txtTaskName.text = desc.TaskName;
        txtTaskValue.text = desc.TaskValue;
        imgTaskIcon.sprite = desc.TaskSprite;

        if (desc.TaskSprite != null) 
        {

            imgTaskIcon.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    private void OnDestroy()
    {
        _detectGame.OnGameOver -= GameOver;
        _detectGame.OnCount -= ChangeCount;
    }

    private void SelectTypeGame()
    {
        GameOverType randomType = (GameOverType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(GameOverType)).Length);
  
        switch (randomType)
        {
            case GameOverType.MaxCount:
                _detectGame = new MaxCountService(_gridSystem);
                break;
            case GameOverType.GetColor:
                _detectGame = new GetColorService(_gridSystem);
                break;
        }

    }

    private void GameOver()
    {
        Debug.Log("Игра окончена");
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }

    private void ChangeCount(int count)
    {
        txtTaskValue.text = count.ToString();
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GotToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
