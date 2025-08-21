using System.Collections.Generic;
using UnityEngine;

public class MonsterPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs; // 일반 + 엘리트 모두 포함
    [SerializeField] private int poolSize = 10;

    private Dictionary<int, Queue<GameObject>> poolDict = new();
    private Transform monsterParent; // 모든 몬스터의 부모

    public static MonsterPool Instance;

    private void Awake()
    {
        Instance = this;

        monsterParent = new GameObject("MonsterContainer").transform;
        monsterParent.SetParent(this.transform);

        for (int i = 0; i < monsterPrefabs.Count; i++)
            CreatePool(i, monsterPrefabs[i]);
    }

    private void Start()
    {

    }

    private void CreatePool(int index, GameObject prefab)
    {
        poolDict[index] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(prefab, monsterParent);
            go.SetActive(false);
            poolDict[index].Enqueue(go);
        }
    }

    public GameObject Get(int index)
    {
        if (!poolDict.ContainsKey(index) || poolDict[index].Count == 0)
        {
            Debug.LogWarning($"풀 부족: {index}");
            return null;
        }

        GameObject obj = poolDict[index].Dequeue();
        obj.SetActive(true);
        obj.transform.SetParent(monsterParent); // 사용 중에도 부모 유지
        return obj;
    }

    public void Return(int index, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(monsterParent);
        poolDict[index].Enqueue(obj);
    }
}
