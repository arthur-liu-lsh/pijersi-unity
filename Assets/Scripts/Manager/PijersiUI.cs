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

    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject resume;
    [SerializeField] private TMP_Text Title;
    [SerializeField] private TMP_Text Display;
    [SerializeField] private TextMeshProUGUI gameState;
    [SerializeField] private TextMeshProUGUI record;

    private string[] teamName = {"White", "Black"};

    #region base
    private void Start()
    {
        ResetUI();
    }
    #endregion

    #region pauseEnd
    public void SetActivePause(bool value)
    {
        pause.SetActive(value);
    }

    public void ShowEnd(int winTeamId, int[] teamWinCounts, int maxWinRound)
    {
        pause.SetActive(true);
        if (teamWinCounts[winTeamId] == maxWinRound)
            resume.SetActive(false);
        Title.text = $"{teamName[winTeamId]} win !";
        Display.text = $"{teamWinCounts[0]} - {teamWinCounts[1]} / {maxWinRound}";
    }

    public void HideEnd()
    {
        pause.SetActive(false);
        resume.SetActive(true);
        Title.text = "Pause";
        Display.text = "";
    }
    #endregion

    #region game state
    public void UpdateGameState(int teamId, bool isAi)
    {
        gameState.text = (teamId == 0 ? "white" : "black") + (isAi ? " (AI)" : "");
    }
    #endregion

    #region record
    public void UpdateRecord(Cell start, Cell end, ActionType action, bool isNewturn)
    {
        string newRecord = isNewturn ? start.name : "";

        if (action == ActionType.Move || action == ActionType.Attack)
            newRecord += end.isFull ? stackMoveSign : moveSign;
        else
            newRecord += moveSign;

        newRecord += end.name;

        if (action == ActionType.Attack)
            newRecord += attackSign;

        record.text += newRecord;
    }

    public void AddRecordColumnLine(int teamId)
    {
        if (record.text.Length == 0) return;

        record.text += teamId == 0 ? "\n" : "\t";
    }
    #endregion

    public void ResetUI()
    {
        gameState.text = "";
        record.text = "";
        HideEnd();
    }

    public void MainMenu()
    {
        GameManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        GameManager.Quit();
    }
}
