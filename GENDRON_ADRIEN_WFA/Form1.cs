using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GENDRON_ADRIEN_WFA
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, jumping, isGameOver;
        
        //Pour le joueur
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;
        //Pour les plateforme
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        //Pour les ennemies
        int ennemyOneSpeed = 5;
        int ennemyTwoSpeed = 3;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtscore.Text = "Score: " + score;

            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            foreach(Control x in this.Controls)
            {
                if (x is PictureBox)
                {


                    if ((string)x.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {
                            force = 8;
                            player.Top = x.Top - player.Height;

                            if ((string)x.Name == "horizontalPlateform" && goLeft == false || (string)x.Name == "horizontalPlateform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }



                        }
                    }

                    x.BringToFront();


                }

                if ((string)x.Tag == "coin")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                    {
                        x.Visible = false;
                        score++;
                    }
                }

                if (((string)x.Tag == "ennemy"))
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        gameTimer.Stop();
                        isGameOver = true;
                        txtscore.Text = "score: " + score + Environment.NewLine + "You were killed in your journey !!";
                    }
                }
            }

            horizontalPlateform.Left -= horizontalSpeed;

            if (horizontalPlateform.Left < 0 || horizontalPlateform.Left + horizontalPlateform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlateform.Top += verticalSpeed;

            if (verticalPlateform.Top < 200 || verticalPlateform.Top > 609)
            {
                verticalSpeed = -verticalSpeed;
            }

            ennemyOne.Left -= ennemyOneSpeed;

            if (ennemyOne.Left < pictureBox5.Left || ennemyOne.Left + ennemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                ennemyOneSpeed = -ennemyOneSpeed;
            }

            ennemyTwo.Left += ennemyTwoSpeed;

            if (ennemyTwo.Left < pictureBox2.Left || ennemyTwo.Left + ennemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                ennemyTwoSpeed = -ennemyTwoSpeed;
            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtscore.Text = "Score: " + score;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }



            player.Left = 60;
            player.Top = 665;

            ennemyOne.Left = 371;
            ennemyTwo.Left = 384;

            horizontalPlateform.Left = 167;
            verticalPlateform.Top = 170;

            gameTimer.Start();


        }
    }
}
