using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Pijersi
{
    public void ResetMatch()
    {
        if (teams[0].Type != PlayerType.Human || teams[1].Type != PlayerType.Human)
            engine = new Engine();
        save = new Save(config.playerTypes);

        teams[0].score = 0;
        teams[1].score = 0;
        replayState   = ReplayState.None;
        replayType    = ReplayType.Action;
        currentTeamId = 1;
        board.ResetBoard();
        UI.ResetUI();
        cameraMovement.position = CameraMovement.positionType.White;

        SM.ChangeState(State.Turn);
    }

    public void TogglePause()
    {
        isPauseOn = !isPauseOn;
        Time.timeScale = 1 - Time.timeScale;
        UI.SetActivePause(isPauseOn);
    }

    public void Save()
    {
        save.Write();
    }

    public void Replay()
    {
        replaySave = new Save(this.save);
        ResetMatch();
        replayState = ReplayState.Play;
        UI.replayButtons["Play"].interactable = true;
    }

    public void PausePlay()
    {
        replayState = replayState == ReplayState.Play ? ReplayState.Pause : ReplayState.Play;
        if (SM.currentState.id == State.PlayerTurn)
            SM.ChangeState(State.Replay);
    }

    public void Back(bool isTurn)
    {
        if (replayState == ReplayState.Play)
            replayState = ReplayState.Pause;

        replayAt.Item1 = save.turns.Count - 1;
        if (save.turns[replayAt.Item1].actions.Count == 0)
            replayAt.Item1--;

        replayAt.Item2 = isTurn ? 0 : save.turns[replayAt.Item1].actions.Count - 1;

        UI.SetReplayButtonsInteractable(false);
        SM.ChangeState(State.Back);
    }
    
    public void Next(bool isTurn)
    {
        if (replayState == ReplayState.Play)
            replayState = ReplayState.Pause;

        replayAt.Item1 = save.turns.Count - 1;
        replayAt.Item2 = save.turns[replayAt.Item1].actions.Count;

        replayType = isTurn ? ReplayType.Turn : ReplayType.Action;
        UI.SetReplayButtonsInteractable(false);

        SM.ChangeState(State.Next);
    }
}
