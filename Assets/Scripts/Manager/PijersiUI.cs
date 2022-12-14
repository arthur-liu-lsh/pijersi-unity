using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PijersiUI : MonoBehaviour
{
    private const char moveSign = '-';
    private const char stackMoveSign = '=';
    private const char attackSign = '!';

    [Header("Overlay :")]
    [SerializeField] private TMP_Text Display;
    [SerializeField] private TextMeshProUGUI gameState;
    [SerializeField] private TextMeshProUGUI record;
    [SerializeField] private GameObject replay;
    [Header("Pause/End Menu :")]
    [SerializeField] private GameObject pause;
    [SerializeField] private Selectable resume;
    [SerializeField] private Selectable restart;
    [SerializeField] private GameObject endOptions;
    [SerializeField] private Button save;
    [SerializeField] private TMP_Text Title;

    private string[] teamColor = {"White", "Black"};
    private bool isFirstAction = true;
    private List<string> records = new List<string>();

    public Dictionary<string, BetterButton> replayButtons { get; private set; }

    private void Awake()
    {
        replayButtons = new Dictionary<string, BetterButton>();
        foreach (BetterButton button in replay.GetComponentsInChildren<BetterButton>())
            replayButtons.Add(button.name, button);
    }

    private void Start()
    {
        ResetUI();
    }

    public void SetActivePause(bool value)
    {
        pause.SetActive(value);
        resume.Select();
    }

    public void ShowEnd(int winTeamId, int[] teamWinCounts, int maxWinRound)
    {
        pause.SetActive(true);
        endOptions.SetActive(true);
        Title.text   = $"{teamColor[winTeamId]} win !";
        Display.text = $"{teamWinCounts[0]} - {teamWinCounts[1]} / {maxWinRound}";
        if (teamWinCounts[winTeamId] == maxWinRound)
        {
            resume.gameObject.SetActive(false);
            restart.Select();
            return;
        }

        resume.Select();
    }

    public void HideEnd()
    {
        pause.SetActive(false);
        endOptions.SetActive(false);
        resume.gameObject.SetActive(true);
        save.interactable = true;
        Title.text        = "Pause";
        Display.text      = "";
    }

    public void UpdateGameState(int teamId, string teamName)
    {
        gameState.text = (teamId == 0 ? "white" : "black") + " : " + teamName;
    }

    public void UpdateRecord(Cell start, Cell end, ActionType action)
    {
        string newRecord = isFirstAction ? start.name : "";

        if (action == ActionType.Move || action == ActionType.Attack)
            newRecord += end.isFull ? stackMoveSign : moveSign;
        else
            newRecord += moveSign;

        newRecord += end.name;

        if (action == ActionType.Attack)
            newRecord += attackSign;

        record.text  += newRecord;
        isFirstAction = false;
        records.Add(newRecord);
    }

    public void AddRecordColumnLine(int teamId)
    {
        if (record.text.Length == 0) return;
        string newRecord = teamId == 0 ? "\n" : "\t";
        record.text     += newRecord;
        isFirstAction    = true;
        records[records.Count -1] += newRecord;
    }

    public void UndoRecord()
    {
        int newRecordSize = records.Count - 1;
        records.RemoveAt(newRecordSize);
        if (newRecordSize == 0)
            isFirstAction = true;

        string newRecord = "";
        foreach (string record in records)
            newRecord += record;

        record.text = newRecord;
    }

    public void SetReplayButtonsInteractable(bool value)
    {
        foreach (KeyValuePair<string, BetterButton> replayButton in replayButtons)
            replayButton.Value.interactable = value;
    }

    public void ResetUI()
    {
        gameState.text = "";
        record.text    = "";
        isFirstAction  = true;
        records.Clear();
        HideEnd();
        SetReplayButtonsInteractable(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        GameManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        GameManager.Quit();
    }
}
