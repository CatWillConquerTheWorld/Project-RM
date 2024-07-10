using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // �ν����� â�� ���� �� �ִ�.
public class ObjectInfo
{
    public GameObject goPrefab; // ��Ʈ ������
    public int count; // ������ ������ ����
    public Transform tPoolParent; // ��� �θ� �ֳ�?
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance; // ��𼭵� ���� ��������

    [SerializeField] ObjectInfo[] objectInfo = null;
    
    public Queue<GameObject> noteQueue = new Queue<GameObject>();


    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);

    }


    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();

        for(int i = 0; i < p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);

            if(p_objectInfo.tPoolParent != null)
            {
                t_clone.transform.SetParent(p_objectInfo.tPoolParent);
            }
            else
            {
                t_clone.transform.SetParent(this.transform);
            }

            t_queue.Enqueue(t_clone);
        }

        return t_queue;
    }
}
