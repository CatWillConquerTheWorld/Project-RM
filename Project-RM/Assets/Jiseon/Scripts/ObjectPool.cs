
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


            if (t_clone.name == "LONGNOTE(Clone)") // 롱노트 배치
            {
                rectTransform.SetParent(p_objectInfo.tPoolParent, false); // UI 부모 설정
                rectTransform.anchoredPosition = Vector2.zero; // 원하는 위치로 조정
            }
            else // 일반 노트 배치
            {
                // 일반 오브젝트인 경우
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
