using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore
{
    public enum PossibleEndings
    {
        Checkmate, // checkmate is checkmate
        Stalemate, // stalemate occurs when a player has no legal moves left, and their king is not in check.
        InsufficientMaterial, // If both players have insufficient pieces to checkmate the opponent
        FiftyMoveRule, // If there have been 50 consecutive moves by each player without a pawn move or a piece capture, a player can claim a draw.
        ThreefoldRepetition, // If the same position occurs three times with the same player to move, a player can claim a draw.
    }
}
