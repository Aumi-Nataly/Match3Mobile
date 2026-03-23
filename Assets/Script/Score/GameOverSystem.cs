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
    }

    private void SelectTypeGame()
    {
        //  GameOverType randomType = (GameOverType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(GameOverType)).Length);

        GameOverType randomType = GameOverType.GetColor;


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

    public void ReloadLevel()
    {
        Time.timeScale = 1f;
        _gridSystem.ClearGrid();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
