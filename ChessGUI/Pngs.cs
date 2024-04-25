using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessCore;
using SuperSimpleTcp;

namespace ChessGUI
{
    public static class Pngs
    {

        


        private static readonly Dictionary<PieceVariant, ImageSource> whiteSource = new()
        {
            { PieceVariant.Pawn, LoadPng("Assets/pawnw.png") },
            { PieceVariant.Bishop, LoadPng("Assets/bishopw.png") },
            { PieceVariant.Knight, LoadPng("Assets/knightw.png") },
            { PieceVariant.Rook, LoadPng("Assets/rookw.png") },
            { PieceVariant.King, LoadPng("Assets/kingw.png") },
            { PieceVariant.Queen, LoadPng("Assets/queenw.png") }
        };

        private static readonly Dictionary<PieceVariant, ImageSource> blackSource = new()
        {
            { PieceVariant.Pawn, LoadPng("Assets/pawn.png") },
            { PieceVariant.Bishop, LoadPng("Assets/bischop.png") },
            { PieceVariant.Knight, LoadPng("Assets/knight.png") },
            { PieceVariant.Rook, LoadPng("Assets/rook.png") },
            { PieceVariant.King, LoadPng("Assets/king.png") },
            { PieceVariant.Queen, LoadPng("Assets/queen.png") }
        };
        private static ImageSource LoadPng(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

        public static ImageSource TakePng(Player colour, PieceVariant variant)
        {
            return colour switch
            {
                Player.White => whiteSource[variant],
                Player.Black => blackSource[variant],
                _ => null
            };

        }

        public static ImageSource TakePng(Piece piece)
        {
            if (piece == null)
            {
                return null;
            }
            return TakePng(piece.Colour, piece.Variant);
        }
    }
}
