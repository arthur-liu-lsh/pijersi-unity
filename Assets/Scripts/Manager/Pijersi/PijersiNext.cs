public partial class Pijersi
{
    private void OnEnterNext()
    {
        int turnId   = save.turns.Count - 1;
        int actionId = save.turns[turnId].actions.Count;

        Save.Turn turn = replaySave.turns[turnId];
        selectedCell = turn.cells[actionId];
        pointedCell  = turn.cells[actionId + 1];

        switch (turn.actions[actionId])
        {
            case ActionType.Move:
                SM.ChangeState(State.Move);
                break;
            case ActionType.Stack:
                SM.ChangeState(State.Stack);
                break;
            case ActionType.Unstack:
                SM.ChangeState(State.Unstack);
                break;
            default:
                break;
        }

        if (replayAt.Item1 == turnId && replayAt.Item2 == save.turns[turnId].actions.Count - 1)
        {
            if (replayAt.Item1 == replaySave.turns.Count - 1 && replayAt.Item2 == replaySave.turns[turnId].actions.Count - 1)
                replaySave = null;
            replayAt   = (-1, -1);
        }
    }
    private void OnUpdateNext() {}
}
