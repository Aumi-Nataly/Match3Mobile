using TMPro;
using UnityEngine;
using VContainer;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtTaskName;

    [SerializeField]
    private TMP_Text txtTaskValue;


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
    }
    private void OnDestroy()
    {
        _detectGame.OnGameOver -= GameOver;
    }

    private void SelectTypeGame()
    {
        //  GameOverType randomType = (GameOverType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(GameOverType)).Length);

        GameOverType randomType = GameOverType.MaxCount;

        switch (randomType)
        {
            case GameOverType.MaxCount:
                _detectGame = new MaxCountService(_gridSystem);
                break;
            case GameOverType.GetColor:
                _detectGame = new GetColorService();
                break;
        }

    }

    private void GameOver()
    {
        Debug.Log("Игра окончена");
    }
}
