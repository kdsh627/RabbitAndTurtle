using System.Collections.Generic;
using UnityEngine;

public class WeaponPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> weaponPrefabs;
    [SerializeField] private int poolSize = 20;

    private Dictionary<int, Queue<GameObject>> poolDict = new();
    private Transform weaponParent;

    public static WeaponPool Instance;

    private void Awake()
    {
        Instance = this;

        weaponParent = new GameObject("WeaponContainer").transform;
        weaponParent.SetParent(this.transform);

        for (int i = 0; i < weaponPrefabs.Count; i++)
            CreatePool(i, weaponPrefabs[i]);
    }

    private void CreatePool(int index, GameObject prefab)
    {
        poolDict[index] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(prefab, weaponParent);
            go.SetActive(false);
            poolDict[index].Enqueue(go);
        }
    }

    public GameObject Get(int index)
    {
        if (!poolDict.ContainsKey(index) || poolDict[index].Count == 0)
        {
            Debug.LogWarning($"[WeaponPool] 풀 부족: {index}");
            return null;
        }

        GameObject obj = poolDict[index].Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(weaponParent);
        return obj;
    }

    public void Return(int index, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(weaponParent);
        poolDict[index].Enqueue(obj);
    }
}
