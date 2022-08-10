using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wise : Piece
{
    public override Dictionary<ActionType, List<Cell>> GetValideMoves(bool canMove, bool canStack)
    {
        Dictionary<ActionType, List<Cell>> result = new Dictionary<ActionType, List<Cell>>();
        result.Add(ActionType.move, new List<Cell>());
        result.Add(ActionType.attack, new List<Cell>());
        result.Add(ActionType.stack, new List<Cell>());
        result.Add(ActionType.unstack, new List<Cell>());

        foreach (Cell target in cell.nears)
        {
            if (target == null) continue;

            if (canMove && target.isEmpty)
                result[ActionType.move].Add(target);

            if (canStack)
            {
                if (cell.isFull && target.isEmpty)
                    result[ActionType.unstack].Add(target);
                else if (!target.isEmpty && !target.isFull && target.pieces[0].team == team && target.pieces[0].type == type)
                    result[ActionType.stack].Add(target);
            }
        }

        if (!canMove)
            return result;

        foreach (Cell target in cell.GetFarNears())
        {
            if (target == null) continue;

            if (target.isEmpty)
                result[ActionType.move].Add(target);
        }

        return result;
    }
}
