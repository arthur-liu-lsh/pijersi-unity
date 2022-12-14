using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Pijersi
{
    private void OnEnterEnd()
    {
        teams[currentTeamId].score++;
        int[] scores = { teams[0].score, teams[1].score };
        UI.ShowEnd(currentTeamId, scores, config.winMax);
        TogglePause();
        replayState = ReplayState.None;
    }

    private void OnExitEnd()
    {
    }

    private void OnUpdateEnd()
    {
        if (config.playerTypes[0] != PlayerType.Human || config.playerTypes[1] != PlayerType.Human)
            engine = new Engine();
        save = new Save(config.playerTypes);
        replayState = ReplayState.None;

        // inversion des ?quipes
        Team firstTeam = teams[0];
        teams[0] = teams[1];
        teams[1] = firstTeam;

        currentTeamId = 1;
        board.ResetBoard();
        UI.ResetUI();

        SM.ChangeState(State.Turn);
    }
}
