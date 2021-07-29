using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHESS
{
    public partial class Form1 : Form
    {
        public const int MapSize = 8;
        public Image Pieces;
        public bool ButtonIsPressed = false;
        public bool CurrentPlayer;
        public int[,] field = new int[MapSize, MapSize];
        public Button[,] buttons = new Button[MapSize,MapSize];
        public enum Piece
        {
            King = 1,
            Queen = 2,
            Bishop = 3,
            Knight = 4,
            Rook = 5,
            Pawns = 6
        }


        public Form1()
        {
            InitializeComponent();
            Pieces = new Bitmap("F:\\Programs\\CHESS\\Pieces3.png");
            Init();
        }


        private void FillField()
        {
            int DefPlayerOne = 10;
            int DefPlayerTwo = 20;
            int EmptyCell = 0;
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    if (i == 0)
                    {
                        switch (j)
                        {
                            case 0: field[i, j] = DefPlayerTwo + (int)Piece.Rook;
                                break;
                            case 1:
                                field[i, j] = DefPlayerTwo + (int)Piece.Knight;
                                break;
                            case 2:
                                field[i, j] = DefPlayerTwo + (int)Piece.Bishop;
                                break;
                            case 3:
                                field[i, j] = DefPlayerTwo + (int)Piece.Queen;
                                break;
                            case 4:
                                field[i, j] = DefPlayerTwo + (int)Piece.King;
                                break;
                            case 5:
                                field[i, j] = DefPlayerTwo + (int)Piece.Bishop;
                                break;
                            case 6:
                                field[i, j] = DefPlayerTwo + (int)Piece.Knight;
                                break;
                            case 7:
                                field[i, j] = DefPlayerTwo + (int)Piece.Rook;
                                break;
                        }
                    }

                    if (i == 1)
                        field[i, j] = DefPlayerTwo + (int)Piece.Pawns;

                    if (i > 1 && i < 6)
                        field[i, j] = EmptyCell;

                    if (i == 6)
                        field[i, j] = DefPlayerOne + (int)Piece.Pawns;
                    if (i == 7)
                    {
                        switch (j)
                        {
                            case 0:
                                field[i, j] = DefPlayerOne + (int)Piece.Rook;
                                break;
                            case 1:
                                field[i, j] = DefPlayerOne + (int)Piece.Knight;
                                break;
                            case 2:
                                field[i, j] = DefPlayerOne + (int)Piece.Bishop;
                                break;
                            case 3:
                                field[i, j] = DefPlayerOne + (int)Piece.Queen;
                                break;
                            case 4:
                                field[i, j] = DefPlayerOne + (int)Piece.King;
                                break;
                            case 5:
                                field[i, j] = DefPlayerOne + (int)Piece.Bishop;
                                break;
                            case 6:
                                field[i, j] = DefPlayerOne + (int)Piece.Knight;
                                break;
                            case 7:
                                field[i, j] = DefPlayerOne + (int)Piece.Rook;
                                break;
                        }
                    }

                }
            }
        }

        public void Init()
        {
            FillField();
            CurrentPlayer = false;
            CreateField();
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer)
                CurrentPlayer = false;
            else
                CurrentPlayer = true;

        }

        public void OnButtons()
        {
            foreach (var item in buttons)
                item.Enabled = true;
        }

        public void OffButtons()
        {
            foreach (var item in buttons)
            {
                item.Enabled = false;
            }
        }

        public void EndOfStep()
        {
            foreach (var item in buttons)
            {
                item.BackColor = Color.LightGray;
            }   
        }

        public static bool CheckOutOfRange(int x, int y)
        {
            bool res = true;
            if (x < 0 || y < 0 || x >= 8 || y >= 8)
                res = false;
            return res;
        }

        private void PushImage(int i, int j, ref Button button, bool check_position)
        {
            Image image = new Bitmap(50, 50);
            Graphics graphics = Graphics.FromImage(image);
            if (check_position) 
                graphics.DrawImage(Pieces, new Rectangle(0, 0, 50, 50), 0 + 150 * (field[i, j] % 10 - 1), 0, 150, 150, GraphicsUnit.Pixel);
            if (!check_position) 
                graphics.DrawImage(Pieces, new Rectangle(0, 0, 50, 50), 0 + 150 * (field[i, j] % 10 - 1), 150, 150, 150, GraphicsUnit.Pixel);
            button.BackgroundImage = image;
        }

        private void FillButton(int i, int j, ref Button button)
        {
            if ((field[i, j] / 10) == 1) 
                PushImage(i, j, ref button, true);
            if ((field[i, j] / 10) == 2) 
                PushImage(i, j, ref button, false);
        }

        public void CreateField()
        {
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    buttons[i,j] = new Button();
                    Button button = new()
                    {
                        Size = new Size(50, 50),
                        Location = new Point(j * 50, i * 50)
                    };
                    FillButton(i, j, ref button);
                    button.Click += new EventHandler(PressButton);
                    button.BackColor = Color.LightGray;
                    buttons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
        }

        private static int DefineNumberPlayer(bool currentPlayer)
        {
            int res = 1;
            if (currentPlayer)
                res = 2;
            return res;
        }

        Button pressed_button = new();
        public void PressButton(object obj, EventArgs events)
        {
            if (pressed_button != null)
                pressed_button.BackColor = Color.LightGray;

            Button inButton = obj as Button;
            int ButLoc = field[inButton.Location.Y / 50, inButton.Location.X / 50];
           
            if (inButton.HaveImage() && ButLoc/10 == DefineNumberPlayer(CurrentPlayer))
            {
                EndOfStep();
                inButton.BackColor = Color.Aquamarine;
                OffButtons(); 
                ButtonIsPressed = true;
                ShowPosToStep(inButton);
                /*
                   must be cancel selection method
                */
            }else
            {
                if (ButtonIsPressed)
                {
                    int x = field[inButton.Location.Y / 50, inButton.Location.X / 50];
                    field[inButton.Location.Y / 50, inButton.Location.X / 50] = field[pressed_button.Location.Y / 50, pressed_button.Location.X / 50];
                    field[pressed_button.Location.Y / 50, pressed_button.Location.X / 50] = 0;
                    inButton.BackgroundImage = pressed_button.BackgroundImage;
                    pressed_button.BackgroundImage = null;
                    ButtonIsPressed = false;
                    EndOfStep();
                    OnButtons();
                    ChangePlayer();
                }
            }
        
            pressed_button = inButton;
            
        }

        public void ShowPawnsStep(Button button)
        {
            int CheckPlayer = field[button.Location.Y / 50, button.Location.X / 50] / 10;
            int VectorPlayer = CheckPlayer == 1 ? -1 : 1;
            int x = button.Location.Y / 50;
            int y = button.Location.X / 50;

            if (CheckOutOfRange(x + 1 * VectorPlayer,y))
            {
                if (field[x + 1 * VectorPlayer, y] == 0)
                {
                    buttons[x + 1 * VectorPlayer, y].BackColor = Color.LightPink;
                    buttons[x + 1 * VectorPlayer, y].Enabled = true;
                }
                
            }
            
            if (CheckOutOfRange(x + 1 * VectorPlayer, y + 1))
            {
                bool CheckEnemyRight = (field[x + 1 * VectorPlayer, y + 1] != 0) && (field[x + 1 * VectorPlayer, y + 1] / 10 != CheckPlayer);
                if (CheckEnemyRight)
                {
                    buttons[x + 1 * VectorPlayer, y + 1].BackColor = Color.LightPink;
                    buttons[x + 1 * VectorPlayer, y + 1].Enabled = true;
                }
                
            }

           
            if (CheckOutOfRange(x + 1 * VectorPlayer, y - 1))
            {
                bool CheckEnemyLeft = (field[x + 1 * VectorPlayer, y - 1] != 0) && (field[x + 1 * VectorPlayer, y - 1] / 10 != CheckPlayer);
                if (CheckEnemyLeft)
                {
                    buttons[x + 1 * VectorPlayer, y - 1].BackColor = Color.LightPink;
                    buttons[x + 1 * VectorPlayer, y - 1].Enabled = true;
                }
                
            }
        }

        public void ShowQueenStep(Button button)
        {
            OrthoStep(button);
            DiagonalStep(button);
        }

        public void ShowKingStep(Button button)
        {
            OrthoStep(button, true);
            DiagonalStep(button,true);
        }

        public void ShowBishopStep(Button button)
        {
            DiagonalStep(button);
        }

        public void ShowKnightStep(Button button)
        {
            int x = button.Location.Y / 50;
            int y = button.Location.X / 50;

            if (CheckOutOfRange(x - 2, y + 1))
                DefineCell(x - 2, y + 1);

            if (CheckOutOfRange(x - 2, y - 1))
                DefineCell(x - 2, y - 1);

            if (CheckOutOfRange(x + 2, y + 1))
                DefineCell(x + 2, y + 1);

            if (CheckOutOfRange(x + 2, y - 1))
                DefineCell(x + 2, y - 1);

            if (CheckOutOfRange(x - 2, y + 1))
                DefineCell(x - 2, y + 1);

            if (CheckOutOfRange(x + 2, y + 1))
                DefineCell(x + 2, y + 1);

            if (CheckOutOfRange(x - 2, y - 1))
                DefineCell(x - 2, y - 1);

            if (CheckOutOfRange(x + 2, y - 1))
                DefineCell(x + 2, y - 1);
           
        }

        public void ShowRookStep(Button button)
        {
            OrthoStep(button);
        }

        public void OrthoStep(Button button, bool ShowOneCell = false)
        {
            int x = button.Location.Y / 50;
            int y = button.Location.X / 50;

            for (int i = x + 1; i < MapSize; i++)
            {
                if (CheckOutOfRange(i, y))
                    if (!DefineCell(i, y))
                        break;
                if (ShowOneCell)
                    break;
            }

            for (int i = x - 1; i >= 0; i--)
            {
                if (CheckOutOfRange(i, y))
                    if (!DefineCell(i, y))
                        break;
                if (ShowOneCell)
                    break;
            }

            for (int i = x + 1; i < MapSize; i++)
            {
                if (CheckOutOfRange(x, i))
                    if (!DefineCell(x, i))
                        break;
                if (ShowOneCell)
                    break;
            }

            for (int i = x - 1; i >= 0; i++)
            {
                if (CheckOutOfRange(x, i))
                    if (!DefineCell(x, i))
                        break;
                if (ShowOneCell)
                    break;
            }
        }

        public void DiagonalStep(Button button, bool ShowOneCell = false)
        {
            int x = button.Location.Y / 50;
            int y = button.Location.X / 50;

            int y_pos = y + 1;
            for (int i = x; i >= 0; i--)
            {
                if (CheckOutOfRange(i, y_pos))
                    if (!DefineCell(i, y_pos))
                        break;

                if (y_pos < 7)
                    y_pos++;
                else
                    break;
                if (ShowOneCell)
                    break;
            }

            y_pos = y - 1;
            for (int i = x - 1; i >= 0; i--)
            {
                if (CheckOutOfRange(i, y_pos))
                    if (!DefineCell(i, y_pos))
                        break;

                if (y_pos > 0)
                    y_pos--;
                else
                    break;
                if (ShowOneCell)
                    break;
            }

            y_pos = y - 1;
            for (int i = x + 1; i < MapSize; i++)
            {
                if (CheckOutOfRange(i, y_pos))
                    if (!DefineCell(i, y_pos))
                        break;

                if (y_pos > 0)
                    y_pos--;
                else
                    break;
                if (ShowOneCell)
                    break;
            }

            y_pos = y + 1;
            for (int i = x + 1; i < MapSize; i++)
            {
                if (CheckOutOfRange(i, y_pos))
                    if (!DefineCell(i, y_pos))
                        break;

                if (y_pos < 7)
                    y_pos++;
                else
                    break;
                if (ShowOneCell)
                    break;
            }
        }

        public bool DefineCell(int x, int y)
        {
            int Player = field[x, y] / 10;
            int CurPlayer = CurrentPlayer ? 2 : 1;
            bool res = true;
            if (field[x,y] == 0)
            {
                buttons[x, y].BackColor = Color.LightPink;
                buttons[x, y].Enabled = true;
            }
            else
            {
                if (Player!=CurPlayer)
                {
                    buttons[x, y].BackColor = Color.LightPink;
                    buttons[x, y].Enabled = true;
                }
                res = false;
            }
            return res;
        }

        public void ShowPosToStep(Button button)
        {
            int TypePiece = field[button.Location.Y / 50, button.Location.X / 50]%10;
            switch (TypePiece)
            {
                case (int)Piece.Pawns:
                    ShowPawnsStep(button);
                    break;
                case (int)Piece.King:
                    ShowKingStep(button);
                    break;
                case (int)Piece.Queen:
                    ShowQueenStep(button);
                    break;
                case (int)Piece.Bishop:
                    ShowBishopStep(button);
                    break;
                case (int)Piece.Knight:
                    ShowKnightStep(button);
                    break;
                case (int)Piece.Rook:
                    ShowRookStep(button);
                    break;
            }
        }
       

    }

}
