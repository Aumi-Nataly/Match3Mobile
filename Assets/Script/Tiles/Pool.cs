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
        CreatePool(Prefab, Size);
        Debug.Log("Pool Awake");
    }


    private void CreatePool(Tile prefab, int size)
    {

        for (int i = 0; i < size; i++)
        {
            Tile tileObject = Instantiate(prefab);
            SceneManager.MoveGameObjectToScene(tileObject.gameObject, SceneManager.GetActiveScene());
            tileObject.gameObject.SetActive(false);
            pool.Enqueue(tileObject);
        }

    }

    public Tile GetFromPool()
    {
        var obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(obj.gameObject, SceneManager.GetActiveScene());
        return obj;
    }


    public void ReturnToPool(Tile prefab)
    {
        prefab.gameObject.SetActive(false);
        prefab.transform.SetParent(null);
        pool.Enqueue(prefab);

    }

}
