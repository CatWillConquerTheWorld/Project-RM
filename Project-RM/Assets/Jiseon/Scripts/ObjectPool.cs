using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // 인스펙터 창에 보일 수 있다.
public class ObjectInfo
{
    public GameObject goPrefab; // 노트 프리팹
    public int count; // 생성할 프리팹 갯수
    public Transform tPoolParent; // 어느 부모에 있나?
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance; // 어디서든 접근 가능해짐

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
