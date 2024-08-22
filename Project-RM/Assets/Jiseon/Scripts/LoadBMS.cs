using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadBMS : MonoBehaviour
{
    private string bmsFilePath = "Assets/Jiseon/BMS/deads.bms";
    public string[] lineData, nownoteData;
    public string title, BPM;
    public List<string> notesData = new List<string>();
    int noteNum = 0;
    public List<string> twonotesDatas = new List<string>();
    double currentTime = 0d;
    NoteManager noteManager;

    public int notes;
    public string fuck = "";
    void Start()
    {
        

        // 변수 초기화
        title = "";
        BPM = "";


        string[] lineData = File.ReadAllLines(bmsFilePath);
        nownoteData = new string[91];
        int nowint = 0;
        bool isNotesSection = false;
        int madi = 0;
        string line1 = "", line2 = "";

        for (int i = 0; i < lineData.Length; i++)
        {
            if (lineData[i] == "#00001:01")
            {
                isNotesSection = true;
            }

            if (isNotesSection && lineData[i] != "#00001:01")
            {
                line1 = lineData[i].Substring(1,3);

                if (i + 1 < lineData.Length)
                {
                    line2 = lineData[i + 1].Substring(1,3);
                }

                if(line1 == line2 && i+1 < lineData.Length)
                {
                    notesData.Add(mergeLine(lineData[i].Substring(7), lineData[i+1].Substring(7)));
                    i++;
                    madi++;
                }
                else
                {
                    notesData.Add(lineData[i].Substring(7));
                    madi++;
                }
                //nownoteData[nowint] = lineData[i].Substring(7);
                //notesData.Add(lineData[i].Substring(7));
                nowint++;
            }
        }

        foreach(string Notess in notesData)
        {
            fuck += Notess + ",";
        }
        changeNoteData();
        Debug.Log("Notesdata count" + notesData.Count);
        initNoteData();
        noteManager = FindObjectOfType<NoteManager>();
        NoteManager.isMusicStarted = true;
    }


    void Update()
    {
        currentTime += Time.deltaTime;
        double BeatTime = bpmManager.instance.bpmInterval / 4;

        if (currentTime >= BeatTime)
        {
            if(noteNum < twonotesDatas.Count)
            {
                noteManager.NoteMaker(twonotesDatas[noteNum]);
                noteNum++;
            }
            currentTime -= BeatTime;
        }

    }


    public void initNoteData()
    {
        for(int j = 0; j < notesData.Count; j++)
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

    public string mergeLine(string line_1 , string line_2)
    {
        string merge = "";
        string convert1 = "", convert2 = "";


        if(line_1.Length == 8)
        {
            for (int i = 0; i < line_1.Length; i += 2) // i < data.Length로 수정
            {
                convert1 += line_1.Substring(i, 2); // 2글자씩 잘라서 추가
                convert1 += "000000"; // 16분음표를 위해 '00' 추가
            }
            line_1 = convert1;
        }else if (line_1.Length == 16)
        {
            for (int i = 0; i < line_1.Length; i += 2) // i < data.Length로 수정
            {
                convert1 += line_1.Substring(i, 2); // 2글자씩 잘라서 추가
                convert1 += "00"; // 16분음표를 위해 '00' 추가
            }
            line_1 = convert1;
        }

        if (line_2.Length == 8)
        {
            for (int i = 0; i < line_2.Length; i += 2) // i < data.Length로 수정
            {
                convert2 += line_2.Substring(i, 2); // 2글자씩 잘라서 추가
                convert2 += "000000"; // 16분음표를 위해 '00' 추가
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 16)
        {
            for (int i = 0; i < line_2.Length; i += 2) // i < data.Length로 수정
            {
                convert2 += line_2.Substring(i, 2); // 2글자씩 잘라서 추가
                convert2 += "00"; // 16분음표를 위해 '00' 추가
            }
            line_2 = convert2;
        }

        for(int i = 0; i < 32; i += 2)
        {
            string twoNum1 = "", twoNum2 = "";

            twoNum1 = line_1.Substring(i, 2);
            twoNum2 = line_2.Substring(i, 2);

            if(twoNum1 != "00" && twoNum2 != "00")
            {
                Debug.Log(twoNum1 + " , " + twoNum2);
            }else if(twoNum1 == "00" && twoNum2 != "00")
            {
                merge += twoNum2;
            }else if (twoNum1 != "00" && twoNum2 == "00")
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

    public void changeNoteData() // 16분음표로 표기
    {
        // Debug.Log(notesData.Count);
        List<string> convertedNotesData = new List<string>();

        for (int j = 0; j < notesData.Count; j++) // notesData의 인덱스를 사용하여 반복
        {
            string data = notesData[j]; // 현재 데이터 가져오기
            string convert = "";
            if (data.Length == 8)
            {
                for (int i = 0; i < data.Length; i += 2) // i < data.Length로 수정
                {
                    convert += data.Substring(i, 2); // 2글자씩 잘라서 추가
                    convert += "000000"; // 16분음표를 위해 '00' 추가
                }
                notesData[j] = convert; // 변환된 데이터를 notesData에 저장
            }else if (data.Length == 16)
            {
                for (int i = 0; i < data.Length; i += 2) // i < data.Length로 수정
                {
                    convert += data.Substring(i, 2); // 2글자씩 잘라서 추가
                    convert += "00"; // 16분음표를 위해 '00' 추가
                }
                notesData[j] = convert; // 변환된 데이터를 notesData에 저장
            }
            else
            {
                convert = data;
            }

            convertedNotesData.Add(convert);
        }

        notesData = convertedNotesData;
    }

    public void changeSong(string filename)
    {
        bmsFilePath = "Assets/Jiseon/BMS/" + filename + ".bms";
        lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();

        // 변수 초기화
        title = "";
        BPM = "";

        // 라인 데이터를 처리합니다.
        bool isNotesSection = false;
        foreach (string bmsLine in lineData)
        {
            // 타이틀과 BPM 추출
            if (bmsLine.StartsWith("#TITLE"))
            {
                title = bmsLine.Substring("#TITLE".Length).Trim();
            }
            else if (bmsLine.StartsWith("#BPM"))
            {
                BPM = bmsLine.Substring("#BPM".Length).Trim();
            }
            // MAIN DATA FIELD의 시작을 감지
            else if (bmsLine.StartsWith("#00001:01")) // 수정: 노트 섹션 시작 감지
            {
                isNotesSection = true;
                Debug.Log(bmsLine);
            }
            // # 형식의 라인이 나오면 notesData 배열에 저장 시작
            else if (isNotesSection)
            {
                // 접두사 제거
                string noteData = bmsLine.Substring(7).Trim();

                notesData.Add(noteData);
            }
        }
    }

    public void readNote()
    {
        // 파일에서 모든 줄을 읽어오고, 빈 줄을 제외합니다.
        lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();

        // 라인 데이터를 처리합니다.
        bool isNotesSection = false;
        foreach (string bmsLine in lineData)
        {
            // 타이틀과 BPM 추출
            if (bmsLine.StartsWith("#TITLE"))
            {
                title = bmsLine.Substring("#TITLE".Length).Trim();
            }
            else if (bmsLine.StartsWith("#BPM"))
            {
                BPM = bmsLine.Substring("#BPM".Length).Trim();
            }
            // MAIN DATA FIELD의 시작을 감지
            else if (bmsLine.StartsWith("#00001:01")) // 수정: 노트 섹션 시작 감지
            {
                isNotesSection = true;
                Debug.Log(bmsLine);
            }
            // # 형식의 라인이 나오면 notesData 배열에 저장 시작
            else if (isNotesSection && bmsLine.StartsWith("#"))
            {
                notes += 1;
                // 접두사 제거
                string noteData = bmsLine.Substring(7).Trim();
                notesData.Add(noteData);
            }
        }
    }
}
