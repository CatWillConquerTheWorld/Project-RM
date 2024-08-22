using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBMS : MonoBehaviour
{
    public static LoadBMS Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(Instance!= this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        centerFrame = FindObjectOfType<CenterFrame>();
        noteManager = FindObjectOfType<NoteManager>();
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public List<string> deads = new List<string>();
    public List<string> tutorialLevel1 = new List<string>();
    public List<string> tutorialLevel2 = new List<string>();

    private string bmsFilePath = "";
    public string[] lineData;
    public string title, BPM;
    public List<string> notesData = new List<string>();
    int noteNum = 0;
    public List<string> twonotesDatas = new List<string>();
    double currentTime = 0d;

    public NoteManager noteManager;
    public CenterFrame centerFrame;

    public int notes;
    void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        centerFrame = FindObjectOfType<CenterFrame>();

        deads = notesetting("deads");
        tutorialLevel1 = notesetting("tutorialLevel1");
        tutorialLevel2 = notesetting("tutorialLevel2");

        NoteManager.isMusicStarted = true;
    }

    public void play_song(string name)
    {
        if (name == "deads")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = deads;
            centerFrame.ChangeMusic(3);
        }
        else if (name == "WTF")
        {
            bpmManager.instance.bpm = 150;

        }
        else if (name == "tutorialLevel1")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = tutorialLevel1;
            centerFrame.ChangeMusic(0);

        }else if(name == "tutorialLevel2")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = tutorialLevel2;
            centerFrame.ChangeMusic(1);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            currentTime = 0d;
            noteNum = 0;
            play_song("deads");
        }else if (Input.GetKeyDown(KeyCode.U))
        {
            currentTime = 0d;
            noteNum = 0;
            play_song("tutorialLevel2");
        }
        else if(Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Stage1");
        }

        if (twonotesDatas.Count == 0) return;

        currentTime += Time.deltaTime;
        double BeatTime = bpmManager.instance.bpmInterval / 4; // 16�� ��ǥ �ð� ������ ���

        if (currentTime >= BeatTime)
        {
            if (noteNum < twonotesDatas.Count)
            {
                noteManager.NoteMaker(twonotesDatas[noteNum]);
                noteNum++;
            }
            else
            {
                noteNum = 0;
                // Debug.Log("�뷡�� �������ϴ�.");
                
                twonotesDatas.Clear();

                Debug.Log(twonotesDatas.Count);
            }
            currentTime -= BeatTime;
        }

    }

    public List<string> notesetting(string bms_name)
    {
        List<string> noteDatalist = new List<string>();
        string bmsFilePath = "Assets/Jiseon/BMS/" + bms_name + ".bms"; // ��Ʈ �̸� �޾ƿ���

        string[] lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();
        int noteNum = 0;

        bool isNotesSection = false;
        string line1 = "", line2 = ""; // ���� ���� ���������� Ȯ��

        for (int i = 0; i < lineData.Length; i++)
        {
            if (lineData[i] == "#00001:01")
            {
                isNotesSection = true;
            }

            if (isNotesSection && lineData[i] != "#00001:01")
            {
                line1 = lineData[i].Substring(1, 3);

                if (i + 1 < lineData.Length)
                {
                    line2 = lineData[i + 1].Substring(1, 3);
                }

                if (line1 == line2 && i + 1 < lineData.Length)
                {
                    // Debug.Log(lineData[i].Substring(7) + " , " + lineData[i + 1].Substring(7));
                    noteDatalist.Add(mergeLine(lineData[i].Substring(7), lineData[i + 1].Substring(7)));
                    i++;
                }
                else
                {
                    noteDatalist.Add(lineData[i].Substring(7));
                }
            }
        }

        noteDatalist = changeNoteData(noteDatalist);
        noteDatalist = initNoteData(noteDatalist);
        return noteDatalist;
    }

    public List<string> initNoteData(List<string> list)
    {
        List<string> convertwoNoteData = new List<string>();
        for (int j = 0; j < list.Count; j++)
        {
            string data = list[j];
            if (list[j].Length == 32)
            {
                for (int i = 0; i < 32; i += 2)
                {
                    string two = "";
                    two = list[j].Substring(i, 2);
                    convertwoNoteData.Add(two);
                }
            }
        }
        return convertwoNoteData;
    }

    public void initNoteData()
    {
        for (int j = 0; j < notesData.Count; j++)
        {
            string data = notesData[j];
            if (notesData[j].Length == 32)
            {
                for (int i = 0; i < 32; i += 2)
                {
                    string two = "";
                    two = data.Substring(i, 2);
                    twonotesDatas.Add(two);
                }
            }
        }
    }

    public string mergeLine(string line_1, string line_2)
    {
        string merge = "";
        string convert1 = "", convert2 = "";

        if (line_1.Length == 4)
        {
            for (int i = 0; i < line_1.Length; i += 2) // i < data.Length�� ����
            {
                convert1 += line_1.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert1 += "00000000000000"; // 16����ǥ�� ���� '00' �߰�
            }
            line_1 = convert1;
        }
        else if (line_1.Length == 8)
        {
            for (int i = 0; i < line_1.Length; i += 2) // i < data.Length�� ����
            {
                convert1 += line_1.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert1 += "000000"; // 16����ǥ�� ���� '00' �߰�
            }
            line_1 = convert1;
        }
        else if (line_1.Length == 16)
        {
            for (int i = 0; i < line_1.Length; i += 2) // i < data.Length�� ����
            {
                convert1 += line_1.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert1 += "00"; // 16����ǥ�� ���� '00' �߰�
            }
            line_1 = convert1;
        }

        if (line_2.Length == 4)
        {
            for (int i = 0; i < line_2.Length; i += 2) // i < data.Length�� ����
            {
                convert2 += line_2.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert2 += "00000000000000"; // 16����ǥ�� ���� '00' �߰�
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 8)
        {
            for (int i = 0; i < line_2.Length; i += 2) // i < data.Length�� ����
            {
                convert2 += line_2.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert2 += "000000"; // 16����ǥ�� ���� '00' �߰�
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 16)
        {
            for (int i = 0; i < line_2.Length; i += 2) // i < data.Length�� ����
            {
                convert2 += line_2.Substring(i, 2); // 2���ھ� �߶� �߰�
                convert2 += "00"; // 16����ǥ�� ���� '00' �߰�
            }
            line_2 = convert2;
        }

        // Debug.Log(line_1.Length + " , " + line_2.Length);

        for (int i = 0; i < 32; i += 2)
        {
            string twoNum1 = "", twoNum2 = "";

            twoNum1 = line_1.Substring(i, 2);
            twoNum2 = line_2.Substring(i, 2);

            if (twoNum1 != "00" && twoNum2 != "00")
            {
                Debug.Log(twoNum1 + " , " + twoNum2);
            }
            else if (twoNum1 == "00" && twoNum2 != "00")
            {
                merge += twoNum2;
            }
            else if (twoNum1 != "00" && twoNum2 == "00")
            {
                merge += twoNum1;
            }
            else
            {
                merge += "00";
            }
        }
        return merge;
    }

    public List<string> changeNoteData(List<string> list) // 16����ǥ�� ǥ��
    {
        // Debug.Log(notesData.Count);
        List<string> convertedNotesData = new List<string>();

        for (int j = 0; j < list.Count; j++) // notesData�� �ε����� ����Ͽ� �ݺ�
        {
            string data = list[j]; // ���� ������ ��������
            string convert = "";

            if (data.Length == 4)
            {
                for (int i = 0; i < data.Length; i += 2) // i < data.Length�� ����
                {
                    convert += data.Substring(i, 2); // 2���ھ� �߶� �߰�
                    convert += "00000000000000"; // 16����ǥ�� ���� '00' �߰�
                }
                list[j] = convert;
            }
            else if (data.Length == 8)
            {
                for (int i = 0; i < data.Length; i += 2) // i < data.Length�� ����
                {
                    convert += data.Substring(i, 2); // 2���ھ� �߶� �߰�
                    convert += "000000"; // 16����ǥ�� ���� '00' �߰�
                }
                list[j] = convert; // ��ȯ�� �����͸� notesData�� ����
            }
            else if (data.Length == 16)
            {
                for (int i = 0; i < data.Length; i += 2) // i < data.Length�� ����
                {
                    convert += data.Substring(i, 2); // 2���ھ� �߶� �߰�
                    convert += "00"; // 16����ǥ�� ���� '00' �߰�
                }
                list[j] = convert; // ��ȯ�� �����͸� notesData�� ����
            }
            else
            {
                convert = data;
            }

            convertedNotesData.Add(convert);
        }

        list = convertedNotesData;

        return list;
    }

    public void changeNoteData()
    {
        // Debug.Log(notesData.Count);
        List<string> convertedNotesData = new List<string>();

        for (int j = 0; j < notesData.Count; j++)
        {
            string data = notesData[j];
            string convert = "";
            if (data.Length == 8)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "000000";
                }
                notesData[j] = convert;
            }
            else if (data.Length == 16)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "00";
                }
                notesData[j] = convert;
            }
            else
            {
                convert = data;
            }

            convertedNotesData.Add(convert);
        }

        notesData = convertedNotesData;
    }

    public void readNote()
    {
        // ���Ͽ��� ��� ���� �о����, �� ���� �����մϴ�.
        lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();

        // ���� �����͸� ó���մϴ�.
        bool isNotesSection = false;
        foreach (string bmsLine in lineData)
        {
            // Ÿ��Ʋ�� BPM ����
            if (bmsLine.StartsWith("#TITLE"))
            {
                title = bmsLine.Substring("#TITLE".Length).Trim();
            }
            else if (bmsLine.StartsWith("#BPM"))
            {
                BPM = bmsLine.Substring("#BPM".Length).Trim();
            }
            // MAIN DATA FIELD�� ������ ����
            else if (bmsLine.StartsWith("#00001:01")) // ����: ��Ʈ ���� ���� ����
            {
                isNotesSection = true;
                Debug.Log(bmsLine);
            }
            // # ������ ������ ������ notesData �迭�� ���� ����
            else if (isNotesSection && bmsLine.StartsWith("#"))
            {
                notes += 1;
                // ���λ� ����
                string noteData = bmsLine.Substring(7).Trim();
                notesData.Add(noteData);
            }
        }
    }
}
