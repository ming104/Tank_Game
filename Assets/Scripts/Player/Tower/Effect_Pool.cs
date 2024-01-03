using System.Collections.Generic;
using UnityEngine;

public class Effect_Pool : MonoBehaviour
{
    public static Effect_Pool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<BoomEffect> poolingObjectQueue = new Queue<BoomEffect>();

    private void Awake()
    {
        Instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private BoomEffect CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<BoomEffect>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static BoomEffect GetObject(Vector3 pos)
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.transform.position = pos;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            newObj.transform.position = pos;
            return newObj;
        }
    }

    public static void ReturnObject(BoomEffect obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}