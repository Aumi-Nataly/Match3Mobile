using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pool : MonoBehaviour
{
    [SerializeField]
    private int Size;

    [SerializeField]
    private Tile Prefab;

    private Queue<Tile> pool = new Queue<Tile>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CreatePool(Prefab, Size);
    }

    private void CreatePool(Tile prefab, int size)
    {

        for (int i = 0; i < size; i++)
        {
            Tile tileObject = Instantiate(prefab);
            DontDestroyOnLoad(tileObject);
            tileObject.gameObject.SetActive(false);
            pool.Enqueue(prefab);
        }

    }

    public Tile GetFromPool()
    {
        var obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        SceneManager.MoveGameObjectToScene(obj.gameObject, SceneManager.GetActiveScene());
        return obj;
    }


    public void ReturnToPool(Tile prefab)
    {
        prefab.gameObject.SetActive(false);
        pool.Enqueue(prefab);

    }

}
