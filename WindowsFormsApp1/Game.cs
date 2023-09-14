using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Game : Form
    {
        #region Variable
        private const Keys up = Keys.Up;
        private const Keys right = Keys.Right;
        private const Keys down = Keys.Down;
        private const Keys left = Keys.Left;
        private const Keys undefined = Keys.Space;

        private const int xMax = 300;
        private const int xMin = 0;
        private const int yMax = 300;
        private const int yMin = 0;

        private int PosX = 60;
        private int PosY = 65;
        private int SnakeSize = 5;
        private int Speed = 120;

        private DirectionEnum direction = DirectionEnum.Right;
        private List<Point> snake;
        private Point Yem;
        private int YemCount = 0;

        Thread thread;
        #endregion


        public Game()
        {
            InitializeComponent();
        }


        #region Snake Metodları

        private void Ekran()
        {
            YemYarat();
            while (!isGameOver())
            {
                try
                {
                    Graphics g = panel1.CreateGraphics();
                    Brush br = new SolidBrush(Color.Red);
                    g.Clear(this.BackColor);
                    Pen pen = new Pen(br, SnakeSize);
                    int c = 0;
                    foreach (Point item in snake)
                    {
                        if (c < 1)
                        {
                            g.DrawRectangle(new Pen(Color.Black, SnakeSize), item.X, item.Y, SnakeSize, SnakeSize);
                            c++;
                        }
                        else
                        {
                            g.DrawRectangle(pen, item.X, item.Y, SnakeSize, SnakeSize);
                        }
                        g.DrawRectangle(new Pen(Color.Green, SnakeSize), Yem.X, Yem.Y, SnakeSize, SnakeSize);

                    }
                }
                catch (Exception)
                {
                    break;
                }

                //if ((Yem.X == PosX || (Yem.X > PosX + 8 && Yem.X < PosX - 8)) && (Yem.Y == PosY || (Yem.Y > PosY + 8 && Yem.Y < PosY - 8)))
                if (Math.Abs(PosX - Yem.X) <= 6 && Math.Abs(PosY - Yem.Y) <= 6)
                {
                    YemYe();
                }


                if (direction == DirectionEnum.Down)
                {
                    PosY += SnakeSize;
                }
                else if (direction == DirectionEnum.Up)
                {
                    PosY -= SnakeSize;
                }
                else if (direction == DirectionEnum.Left)
                {
                    PosX -= SnakeSize;
                }
                else if (direction == DirectionEnum.Right)
                {
                    PosX += SnakeSize;
                }

                Hareket();
                Thread.Sleep(Speed);
            }
            if (isGameOver())
            {
                MessageBox.Show("Oyun Bitti!\n\nPuanınız: " + YemCount);
                return;
            }
        }

        private void YemYarat()
        {
            Random random = new Random(DateTime.Now.TimeOfDay.Milliseconds);
            int x = 0;
            int y = 0;
            bool contains = true;

            while (contains)
            {
                x = random.Next(xMin, xMax / 10);
                y = random.Next(yMin, yMax / 10);
                x = x * 10;
                y = y * 10;
                contains = snake.Any(t => t.X == x && t.Y == y);
            }
            Yem = new Point(x, y);
        }

        private void YemYe()
        {
            Point lastPoint = snake[snake.Count - 1];
            snake.Add(new Point(lastPoint.X, lastPoint.Y));
            YemYarat();
            YemCount++;
            if (Speed > 30)
            {
                Speed -= 5;
            }
        }

        public void Hareket()
        {
            snake.Insert(0, new Point(PosX, PosY));
            snake.RemoveAt(snake.Count - 1);

            if (PosX > xMax)
            {
                PosX = xMin;
            }
            if (PosX < xMin)
            {
                PosX = xMax;
            }
            if (PosY > yMax)
            {
                PosY = yMin;
            }
            if (PosY < yMin)
            {
                PosY = yMax;
            }
        }

        private bool isGameOver()
        {
            //Kendine Yeme
            //if (snake.Any(t => t.X == posX && t.Y == posY))
            //{
            //    return true;
            //}

            return false;
        }

        private void StartGame()
        {
            snake = new List<Point>();

            snake.Add(new Point(55, 65));
            snake.Add(new Point(50, 65));
            snake.Add(new Point(45, 65));
            snake.Add(new Point(40, 65));
            snake.Add(new Point(35, 65));
            snake.Add(new Point(30, 65));
            snake.Add(new Point(25, 65));
            thread = new Thread(new ThreadStart(Ekran));
            thread.Start();
        }
        private void StopGame()
        {
            thread.Suspend();
        }
        private void StopStartGame()
        {
            thread.Resume();
        }
        #endregion

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == up)
            {
                if (direction != DirectionEnum.Down)
                    direction = DirectionEnum.Up;
            }
            else if (keyData == down)
            {
                if (direction != DirectionEnum.Up)
                    direction = DirectionEnum.Down;
            }
            else if (keyData == left)
            {
                if (direction != DirectionEnum.Right)
                    direction = DirectionEnum.Left;
            }
            else if (keyData == right)
            {
                if (direction != DirectionEnum.Left)
                    direction = DirectionEnum.Right;
            }
            else
            {
                direction = DirectionEnum.Undefined;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartGame();
            btnStop.Visible = true;
            btnStart.Visible = false;
            btnDevam.Visible = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopGame();
            btnStop.Visible = false;
            btnDevam.Visible = true;
        }

        private void btnDevam_Click(object sender, EventArgs e)
        {
            StopStartGame();
            btnStop.Visible = true;
            btnDevam.Visible = false;
        }
    }
}
public enum DirectionEnum
{
    Undefined,
    Up,
    Right,
    Down,
    Left
}