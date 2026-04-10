using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadBMS : MonoBehaviour
{
    public static LoadBMS Instance = null;

    private const string EnemyBannerName = "ENEMY";
    private const string YoursBannerName = "YOURS";

    [System.Serializable]
    private class RhythmStartBannerTarget
    {
        public Graphic graphic;
        public SpriteRenderer spriteRenderer;
        public float visibleAlpha;

        public RhythmStartBannerTarget(Graphic graphic)
        {
            this.graphic = graphic;
            visibleAlpha = graphic.color.a > 0f ? graphic.color.a : 1f;
        }

        public RhythmStartBannerTarget(SpriteRenderer spriteRenderer)
        {
            this.spriteRenderer = spriteRenderer;
            visibleAlpha = spriteRenderer.color.a > 0f ? spriteRenderer.color.a : 1f;
        }

        public void SetAlpha(float alpha)
        {
            if (graphic != null)
            {
                Color color = graphic.color;
                color.a = alpha;
                graphic.color = color;
            }
            else if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }
    }

    [Header("Rhythm Start Banner")]
    [SerializeField] private float startBannerFadeInDuration = 0.35f;
    [SerializeField] private float startBannerVisibleDuration = 3f;
    [SerializeField] private float startBannerFadeOutDuration = 0.35f;

    private readonly List<RhythmStartBannerTarget> rhythmStartBanners = new List<RhythmStartBannerTarget>();
    private Coroutine rhythmStartBannerRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        centerFrame = FindObjectOfType<CenterFrame>();
        noteManager = FindObjectOfType<NoteManager>();
        enemyNoteManager = FindObjectOfType<EnemyNoteManager>();
        CacheRhythmStartBanners();
        SetRhythmStartBannerAlpha(0f);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public List<string> tutorialLevel1 = new List<string>();
    public List<string> tutorialLevel2 = new List<string>();
    public List<string> tutorialLevel2Enemy = new List<string>();
    public List<string> WTF = new List<string>();
    public List<string> WTFEnemy = new List<string>();
    public List<string> deads = new List<string>();
    public List<string> deadsEnemy = new List<string>();
    public List<string> C_Diminished = new List<string>();
    public List<string> C_DiminishedEnemy = new List<string>();
    public List<string> C_Monstered = new List<string>();
    public List<string> C_MonsteredEnemy = new List<string>();
    public List<string> Venomous = new List<string>();
    public List<string> VenomousEnemy = new List<string>();

    private string bmsFilePath = "";
    public string[] lineData;
    public string title, BPM;
    public List<string> notesData = new List<string>();
    public int noteNum = 0;
    public List<string> twonotesDatas = new List<string>();
    public List<string> twonotesEnemyDatas = new List<string>();
    public static double currentTime = 0d;

    public NoteManager noteManager;
    public CenterFrame centerFrame;
    public EnemyNoteManager enemyNoteManager;

    public bool isEnded;

    public int notes;
    void Start()
    {
        noteManager = FindObjectOfType<NoteManager>();
        centerFrame = FindObjectOfType<CenterFrame>();

        deads = notesetting("deads");
        deadsEnemy = notesetting("deads_enemy");
        tutorialLevel1 = notesetting("tutorialLevel1");
        tutorialLevel2 = notesetting("tutorialLevel2");
        tutorialLevel2Enemy = notesetting("enemyTutorialLevel2");
        WTF = notesetting("WTF");
        WTFEnemy = notesetting("enemyWTF");
        C_Diminished = notesetting("C-Diminished");
        C_DiminishedEnemy = notesetting("C-DiminishedEnemy");
        C_Monstered = notesetting("C-Monstered");
        C_MonsteredEnemy = notesetting("C-MonsteredEnemy");
        Venomous = notesetting("Venomous");
        VenomousEnemy = notesetting("VenomousEnemy");
        NoteManager.isMusicStarted = true;
        isEnded = false;
    }

    public void play_song(string name)
    {
        isEnded = false;
        noteNum = 0;
        currentTime = 0d;
        ComboManager.Instance?.ResetCombo();
        noteManager?.ResetRhythmState();
        enemyNoteManager?.ResetRhythmState();

        if (name == "deads")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = deads;
            twonotesEnemyDatas = deadsEnemy;
            centerFrame.ChangeMusic(3);
        }
        else if (name == "WTF")
        {
            bpmManager.instance.bpm = 150;
            twonotesEnemyDatas = WTFEnemy;
            twonotesDatas = WTF;
            centerFrame.ChangeMusic(2);
        }
        else if (name == "tutorialLevel1")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = tutorialLevel1;
            twonotesEnemyDatas.Clear();
            centerFrame.ChangeMusic(0);
        }
        else if (name == "tutorialLevel2")
        {
            bpmManager.instance.bpm = 120;
            twonotesDatas = tutorialLevel2;
            twonotesEnemyDatas = tutorialLevel2Enemy;
            centerFrame.ChangeMusic(1);
        }
        else if (name == "C-Diminished")
        {
            bpmManager.instance.bpm = 105;
            twonotesDatas = C_Diminished;
            twonotesEnemyDatas = C_DiminishedEnemy;
            centerFrame.ChangeMusic(4);
        }
        else if (name == "C-Monstered")
        {
            bpmManager.instance.bpm = 105;
            twonotesDatas = C_Monstered;
            twonotesEnemyDatas = C_MonsteredEnemy;
            centerFrame.ChangeMusic(5);
        }
        else if (name == "Venomous")
        {
            bpmManager.instance.bpm = 180;
            twonotesDatas = Venomous;
            twonotesEnemyDatas = VenomousEnemy;
            centerFrame.ChangeMusic(6);
        }
        else if (name == "")
        {
            centerFrame.music_change = true;
        }
    }

    void Update()
    {
        if (twonotesDatas.Count == 0) return;

        currentTime += Time.deltaTime;
        double beatTime = bpmManager.instance.bpmInterval / 4;

        if (currentTime >= beatTime)
        {
            if (noteNum < twonotesDatas.Count && noteNum < twonotesEnemyDatas.Count)
            {
                noteManager.NoteMaker(twonotesDatas[noteNum]);
                enemyNoteManager.NoteMaker(twonotesEnemyDatas[noteNum]);
                noteNum++;
            }
            else if (noteNum < twonotesDatas.Count)
            {
                noteManager.NoteMaker(twonotesDatas[noteNum]);
                noteNum++;
            }
            else
            {
                noteNum = 0;
                isEnded = true;
                twonotesDatas.Clear();
                twonotesEnemyDatas.Clear();
                Debug.Log(twonotesDatas.Count);
            }
            currentTime -= beatTime;
        }
    }

    public List<string> notesetting(string bms_name)
    {
        List<string> noteDatalist = new List<string>();
        string bmsFilePath = Application.streamingAssetsPath + "/" + bms_name + ".bms";

        string[] lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();

        bool isNotesSection = false;
        string line1 = "", line2 = "";

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
            if (list[j].Length == 32)
            {
                for (int i = 0; i < 32; i += 2)
                {
                    string two = list[j].Substring(i, 2);
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
                    string two = data.Substring(i, 2);
                    twonotesDatas.Add(two);
                }
            }
        }
    }

    public string mergeLine(string line_1, string line_2)
    {
        string merge = "";
        string convert1 = "", convert2 = "";

        if (line_1.Length == 2)
        {
            for (int i = 0; i < line_1.Length; i += 2)
            {
                convert1 += line_1.Substring(i, 2);
                convert1 += "000000000000000000000000000000";
            }
            line_1 = convert1;
        }
        else if (line_1.Length == 4)
        {
            for (int i = 0; i < line_1.Length; i += 2)
            {
                convert1 += line_1.Substring(i, 2);
                convert1 += "00000000000000";
            }
            line_1 = convert1;
        }
        else if (line_1.Length == 8)
        {
            for (int i = 0; i < line_1.Length; i += 2)
            {
                convert1 += line_1.Substring(i, 2);
                convert1 += "000000";
            }
            line_1 = convert1;
        }
        else if (line_1.Length == 16)
        {
            for (int i = 0; i < line_1.Length; i += 2)
            {
                convert1 += line_1.Substring(i, 2);
                convert1 += "00";
            }
            line_1 = convert1;
        }

        if (line_2.Length == 2)
        {
            for (int i = 0; i < line_2.Length; i += 2)
            {
                convert2 += line_2.Substring(i, 2);
                convert2 += "000000000000000000000000000000";
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 4)
        {
            for (int i = 0; i < line_2.Length; i += 2)
            {
                convert2 += line_2.Substring(i, 2);
                convert2 += "00000000000000";
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 8)
        {
            for (int i = 0; i < line_2.Length; i += 2)
            {
                convert2 += line_2.Substring(i, 2);
                convert2 += "000000";
            }
            line_2 = convert2;
        }
        else if (line_2.Length == 16)
        {
            for (int i = 0; i < line_2.Length; i += 2)
            {
                convert2 += line_2.Substring(i, 2);
                convert2 += "00";
            }
            line_2 = convert2;
        }

        for (int i = 0; i < 32; i += 2)
        {
            string twoNum1 = line_1.Substring(i, 2);
            string twoNum2 = line_2.Substring(i, 2);

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

    public List<string> changeNoteData(List<string> list)
    {
        List<string> convertedNotesData = new List<string>();

        for (int j = 0; j < list.Count; j++)
        {
            string data = list[j];
            string convert = "";

            if (data.Length == 2)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "000000000000000000000000000000";
                }
                list[j] = convert;
            }
            else if (data.Length == 4)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "00000000000000";
                }
                list[j] = convert;
            }
            else if (data.Length == 8)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "000000";
                }
                list[j] = convert;
            }
            else if (data.Length == 16)
            {
                for (int i = 0; i < data.Length; i += 2)
                {
                    convert += data.Substring(i, 2);
                    convert += "00";
                }
                list[j] = convert;
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
        lineData = File.ReadAllLines(bmsFilePath)
                       .Where(line => !string.IsNullOrWhiteSpace(line))
                       .ToArray();

        bool isNotesSection = false;
        foreach (string bmsLine in lineData)
        {
            if (bmsLine.StartsWith("#TITLE"))
            {
                title = bmsLine.Substring("#TITLE".Length).Trim();
            }
            else if (bmsLine.StartsWith("#BPM"))
            {
                BPM = bmsLine.Substring("#BPM".Length).Trim();
            }
            else if (bmsLine.StartsWith("#00001:01"))
            {
                isNotesSection = true;
                Debug.Log(bmsLine);
            }
            else if (isNotesSection && bmsLine.StartsWith("#"))
            {
                notes += 1;
                string noteData = bmsLine.Substring(7).Trim();
                notesData.Add(noteData);
            }
        }
    }

    private void CacheRhythmStartBanners()
    {
        rhythmStartBanners.Clear();
        TryAddRhythmStartBanner(GameObject.Find(EnemyBannerName));
        TryAddRhythmStartBanner(GameObject.Find(YoursBannerName));
    }

    private void TryAddRhythmStartBanner(GameObject bannerObject)
    {
        if (bannerObject == null)
        {
            return;
        }

        Graphic graphic = bannerObject.GetComponent<Graphic>();
        if (graphic != null)
        {
            rhythmStartBanners.Add(new RhythmStartBannerTarget(graphic));
            return;
        }

        SpriteRenderer spriteRenderer = bannerObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            rhythmStartBanners.Add(new RhythmStartBannerTarget(spriteRenderer));
        }
    }

    public void ShowRhythmStartBanner()
    {
        if (rhythmStartBannerRoutine != null)
        {
            StopCoroutine(rhythmStartBannerRoutine);
        }

        if (rhythmStartBanners.Count == 0)
        {
            CacheRhythmStartBanners();
        }

        if (rhythmStartBanners.Count == 0)
        {
            return;
        }

        rhythmStartBannerRoutine = StartCoroutine(RhythmStartBannerRoutine());
    }

    private IEnumerator RhythmStartBannerRoutine()
    {
        yield return FadeRhythmStartBanner(0f, 1f, startBannerFadeInDuration);
        yield return new WaitForSeconds(startBannerVisibleDuration);
        yield return FadeRhythmStartBanner(1f, 0f, startBannerFadeOutDuration);
        rhythmStartBannerRoutine = null;
    }

    private IEnumerator FadeRhythmStartBanner(float fromNormalizedAlpha, float toNormalizedAlpha, float duration)
    {
        if (duration <= 0f)
        {
            SetRhythmStartBannerAlpha(toNormalizedAlpha);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetRhythmStartBannerAlpha(Mathf.Lerp(fromNormalizedAlpha, toNormalizedAlpha, t));
            yield return null;
        }

        SetRhythmStartBannerAlpha(toNormalizedAlpha);
    }

    private void SetRhythmStartBannerAlpha(float normalizedAlpha)
    {
        for (int i = 0; i < rhythmStartBanners.Count; i++)
        {
            RhythmStartBannerTarget banner = rhythmStartBanners[i];
            banner.SetAlpha(banner.visibleAlpha * normalizedAlpha);
        }
    }
}
