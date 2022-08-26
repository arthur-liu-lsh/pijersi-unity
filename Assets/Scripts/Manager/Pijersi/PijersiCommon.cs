using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Pijersi
{
    private bool CheckPointedCell()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out hit, 50f, cellLayer))
        {
            pointedCell = null;
            return false;
        }

        pointedCell = hit.transform.GetComponentInParent<Cell>();
        return true;
    }

    private bool IsWin(Cell cell)
    {
        if (cell.lastPiece.type == PieceType.Wise) return false;
        if (cell.x == 0 && cell.pieces[0].team == 0 || cell.x == board.LineCount - 1 && cell.pieces[0].team == 1)
            return true;

        return false;
    }

    private void ToNextActionState()
    {
        if (isReplayOn)
        {
            SM.ChangeState(State.Replay);
            return;
        }

        if (config.playerTypes[currentTeamId] != PlayerType.Human)
        {
            SM.ChangeState(State.PlayAuto);
            return;
        }

        SM.ChangeState(State.Selection);
    }
}
