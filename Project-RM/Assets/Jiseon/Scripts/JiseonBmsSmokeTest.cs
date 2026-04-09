using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(LoadBMS))]
public class JiseonBmsSmokeTest : MonoBehaviour
{
    [SerializeField] private string songName = "tutorialLevel1";
    [SerializeField] private bool runOnStart;
    [SerializeField] private int previewCount = 16;

    private LoadBMS loadBms;

    private void Awake()
    {
        loadBms = GetComponent<LoadBMS>();
    }

    private void Start()
    {
        if (runOnStart)
        {
            RunConfiguredSmokeTest();
        }
    }

    [ContextMenu("Run Jiseon BMS Smoke Test")]
    private void RunSmokeTestFromInspector()
    {
        RunConfiguredSmokeTest();
    }

    public void RunConfiguredSmokeTest()
    {
        if (loadBms == null)
        {
            loadBms = GetComponent<LoadBMS>();
        }

        if (loadBms == null)
        {
            Debug.LogError("[JiseonBmsSmokeTest] LoadBMS component is required.");
            return;
        }

        List<string> parsedNotes = loadBms.notesetting(songName);
        if (parsedNotes == null || parsedNotes.Count == 0)
        {
            Debug.LogWarning("[JiseonBmsSmokeTest] No parsed notes were returned for song: " + songName);
            return;
        }

        int shortCount = 0;
        int longCount = 0;
        int emptyCount = 0;

        for (int i = 0; i < parsedNotes.Count; i++)
        {
            string code = parsedNotes[i];
            if (code == "AA")
            {
                shortCount++;
            }
            else if (code == "AB")
            {
                longCount++;
            }
            else if (code == "00")
            {
                emptyCount++;
            }
        }

        int countToPreview = Mathf.Min(previewCount, parsedNotes.Count);
        StringBuilder previewBuilder = new StringBuilder();
        for (int i = 0; i < countToPreview; i++)
        {
            if (i > 0)
            {
                previewBuilder.Append(", ");
            }

            previewBuilder.Append(parsedNotes[i]);
        }

        Debug.Log(
            "[JiseonBmsSmokeTest]\n" +
            "Song: " + songName + "\n" +
            "Total Steps: " + parsedNotes.Count + "\n" +
            "Short Notes (AA): " + shortCount + "\n" +
            "Long Note Markers (AB): " + longCount + "\n" +
            "Empty Steps (00): " + emptyCount + "\n" +
            "Preview: " + previewBuilder);
    }
}
