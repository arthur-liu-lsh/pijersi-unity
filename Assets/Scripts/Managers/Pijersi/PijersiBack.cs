public partial class Pijersi
{
    private void OnEnterBack()
    {
        engine = null;
        replaySave ??= new Save(save);

        int turnId = save.turns.Count - 1;
        if (save.turns[turnId].actions.Count == 0)
        {
            save.turns.RemoveAt(turnId);
            turnId--;
            currentTeamId = 1 - currentTeamId;
            bool IsActionPossible = save.turns[turnId].actions.Count == 1;
            canMove = IsActionPossible;
            canStack = IsActionPossible;
            UI.SetGameState(currentTeamId, CurrentTeam.type, CurrentTeam.number);
            UpdateCameraPosition();
        }

        Save.Turn turn = save.turns[turnId];
        int actionId = turn.actions.Count - 1;

        selectedCell = turn.cells[actionId + 1];
        pointedCell = turn.cells[actionId];

        switch (turn.actions[actionId])
        {
            case ActionType.Move:
            case ActionType.StackMove:
            case ActionType.Attack:
            case ActionType.StackAttack:
                board.Move(selectedCell, pointedCell);
                canMove = true;
                break;
            case ActionType.Stack:
                board.Unstack(selectedCell, pointedCell);
                canStack = true;
                break;
            case ActionType.Unstack:
            case ActionType.UnstackAttack:
                board.Stack(selectedCell, pointedCell);
                canStack = true;
                break;
            default:
                break;
        }
        
        turn.actions.RemoveAt(actionId);
        turn.cells.RemoveAt(actionId + 1);
        if (actionId == 0)
            turn.cells.RemoveAt(actionId);

        UI.UndoRecord(currentTeamId, actionId == 0);
    }

    private void OnUpdateBack()
    {
        if (board.UpdateMove(pointedCell)) return;

        board.ReviveLastPieces(selectedCell);

        int turnId = save.turns.Count - 1;
        int actionId = save.turns[turnId].actions.Count - 1;

        if (replayTo.turnId <= turnId && replayTo.actionId <= actionId)
        {
            SM.ChangeState(State.Back);
            return;
        }

        replayTo = (-1, -1);

        if (turnId > 0 || actionId > -1)
            UI.ReplayButtons["Back"].interactable = true;

        if (config.playerTypes[currentTeamId] == PlayerType.Human)
        {
            if (actionId == -1)
            {
                SM.ChangeState(State.PlayerTurn);
                return;
            }

            SM.ChangeState(State.Selection);
            return;
        }

        SM.ChangeState(State.Replay);
    }
}
