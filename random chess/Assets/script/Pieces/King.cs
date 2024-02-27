    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Pieces
{
    private int[] king_y = { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] king_x = { 0, 1, 1, 1, 0, -1, -1, -1 };

    public static bool isMove = false;

    public override void SetMovePoint(EColor _color, int y, int x)
    {
        board = ReverseBoard(_color, PlayManager.instance.board);
        tempBoard = board;

        for (int idx = 0; idx < 8; idx++)
        {
            int ny = king_y[idx] + y;
            int nx = king_x[idx] + x;

            if (OverCheck(ny, nx) && board[ny, nx].color != _color)
            {
                TempMove((this, _color), y, x, ny, nx);
                if (AllConfirmPos(_color, 8, 8)) movePoint[ny, nx] = true;
                TempRecovery((this, _color), ny, nx, y, x);
            }
        }

        if (!isMove)
        {
            bool isCastling;
            if (Rook.moveCheck[(7, 0)])
            {
                isCastling = true;
                for (int pos = x - 1; pos > 0; pos--)
                {
                    if (board[y, pos].color != EColor.none && AllConfirmPos(_color, y, pos))
                    {
                        isCastling = false;
                        break;
                    }
                }

                if (isCastling) movePoint[y, x - 2] = true;
            }

            if (Rook.moveCheck[(7, 7)])
            {
                isCastling = true;
                for (int pos = x + 1; pos < 7; pos++)
                {
                    if (board[y, pos].color != EColor.none && AllConfirmPos(_color, y, pos))
                    {
                        isCastling = false;
                        break;
                    }
                }

                if (isCastling) movePoint[y, x + 2] = true;
            }
        }
    }

    public override bool PiecesConfirmPos(EColor _color, int py, int px, int ay, int ax)
    {
        for (int idx = 0; idx < 8; idx++)
        {
            int ny = king_y[idx] + py;
            int nx = king_x[idx] + px;

            if (OverCheck(ny, nx))
            {
                if (ny == ay && nx == ax) return true;
                if (tempBoard[ny, nx] == (PlayManager.instance._king, _color)) return true;
            }
        }

        return false;
    }
}
