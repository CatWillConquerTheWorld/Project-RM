
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
    public Queue<GameObject> longNoteQueue = new Queue<GameObject>();

    void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
        longNoteQueue = InsertQueue(objectInfo[1]);
    }


    Queue<GameObject> InsertQueue(ObjectInfo p_objectInfo)
    {
        Queue<GameObject> t_queue = new Queue<GameObject>();

        for(int i = 0; i < p_objectInfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objectInfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            
            RectTransform rectTransform = t_clone.GetComponent<RectTransform>();


            if (t_clone.name == "LONGNOTE(Clone)") // �ճ�Ʈ ��ġ
            {
                rectTransform.SetParent(p_objectInfo.tPoolParent, false); // UI �θ� ����
                rectTransform.anchoredPosition = Vector2.zero; // ���ϴ� ��ġ�� ����
            }
            else // �Ϲ� ��Ʈ ��ġ
            {
                // �Ϲ� ������Ʈ�� ���
                if (p_objectInfo.tPoolParent != null)
                {
                    t_clone.transform.SetParent(p_objectInfo.tPoolParent);
                }
                else
                {
                    t_clone.transform.SetParent(this.transform);
                }
            }

            t_queue.Enqueue(t_clone);
        }

        return t_queue;
    }
}
