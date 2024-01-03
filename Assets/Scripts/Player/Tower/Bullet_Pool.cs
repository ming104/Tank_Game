using System.Collections.Generic;
using UnityEngine;

public class Bullet_Pool : MonoBehaviour
{
    public static Bullet_Pool Instance;

    [SerializeField]
    private GameObject poolingObjectPrefab;

    Queue<Bullet> poolingObjectQueue = new Queue<Bullet>();

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

    private Bullet CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        newObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        return newObj;
    }

    public static Bullet GetObject()
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            newObj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return newObj;
        }
    }

    public static void ReturnObject(Bullet obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}