using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media; // Pour utiliser SoundPlayer

namespace GENDRON_ADRIEN_WFA
{
    public partial class Form1 : Form 
    {

        bool goLeft, goRight, jumping, isGameOver, onLadder;

        //Pour le joueur
        int jumpSpeed; // Vitesse de saut
        int force; // Force de saut
        int score = 0;
        int playerSpeed = 7;
        bool canJump = true; // Pour empêcher le joueur de sauter plusieurs fois en l'air
        //Pour les plateformes
        int horizontalSpeed = 5;
        int verticalSpeed = 3;
        //Pour les ennemis
        int ennemyOneSpeed = 5;
        int ennemyTwoSpeed = 3;
        bool ennemyOneReverseImg = false; // Pour inverser l'image de l'ennemi
        bool ennemyTwoReverseImg = false;
        bool music = false;
        


        // Ajoutez une variable de classe pour représenter le lecteur de musique SoundPlayer
        SoundPlayer backgroundMusicPlayer;

        public Form1()
        {
            InitializeComponent(); // Initialisez les composants de la fenêtre

            // Initialisez le lecteur de musique
            backgroundMusicPlayer = new SoundPlayer(Properties.Resources.musicpokemon);
           // Répétez la musique en continu
            backgroundMusicPlayer.PlayLooping();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Ferme l'application lorsque l'utilisateur ferme la fenêtre
        }

        private void MainGameTimerEvent(object sender, EventArgs e)
        {
            txtscore.Text = "Score: " + score; // Affichez le score

            player.Top += jumpSpeed; 

            if (goLeft == true) // Si le joueur va à gauche
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true) // Si le joueur va à droite
            {
                player.Left += playerSpeed;
            }

            if (jumping == true && force < 0) 
            {
                jumping = false;
            }

            if (jumping == true) // Si le joueur saute
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            // Vérification des collisions avec les plateformes
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform") // Si le contrôle est un PictureBox et a le tag "platform"
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // Si le joueur touche la plate-forme
                    {
                        force = 8; 

                        // Empêcher le joueur de passer à travers la plate-forme en haut
                        if (player.Bottom > x.Top && player.Top < x.Top) 
                        {
                            player.Top = x.Top - player.Height;
                            jumping = false; // Arrêtez le saut lorsque le joueur touche la plate-forme
                            canJump = true; // Le joueur peut sauter à nouveau
                        }

                        // Réinitialisez la vitesse de saut lorsque le joueur touche une plate-forme
                        if (jumping == false) // Si le joueur ne saute pas
                        {
                            force = 8; 
                        }
                        else // Si le joueur touche la plate-forme en sautant
                        {
                            force = -8; 
                        }
                       
                    }
                }
            }

            // Vérification des collisions avec les pièces (coins)
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox) 
                {
                    if ((string)x.Tag == "coin") 
                    {
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true) // Si le joueur touche la pièce
                        {
                            x.Visible = false;
                            score++;
                        }
                    }
                }
            }

            // Vérification des collisions avec les ennemis
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ennemy") 
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // Si le joueur touche l'ennemis
                    {
                        // Vérifiez si le joueur est en train de descendre (pour tuer l'ennemi) et s'il est au-dessus de l'ennemi
                        if (jumpSpeed > 0 && player.Bottom < x.Bottom)
                        {
                            // L'ennemi est tué, vous pouvez le supprimer
                            x.Tag = "dead";
                            x.Hide();
                           // Augmentez le score
                            score++;
                            // Réinitialisez la vitesse de saut lorsque le joueur saute sur l'ennemi
                            jumpSpeed = -8;
                        }
                        else
                        {
                            // Le joueur n'a pas sauté au-dessus de l'ennemi, il est touché par l'ennemi
                            gameTimer.Stop();
                            isGameOver = true;
                            txtscore.Text = "Score: " + score + Environment.NewLine + "You were killed in your journey !!";
                        }
                    }
                }
            }


            // Vérification des collisions avec l'échelle (ladder)
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ladder")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds)) // Si le joueur touche l'échelle
                    {
                        onLadder = true; // Le joueur est sur l'échelle
                        playerSpeed = 7; // Réglez la vitesse de montée/descente
                    }
                    else
                    {
                        onLadder = false; // Le joueur n'est pas sur l'échelle
                        playerSpeed = 7; // Réglez la vitesse du joueur en dehors de l'échelle
                    }
                }
            }

            horizontalPlateform.Left -= horizontalSpeed;

            if (horizontalPlateform.Left < 0 || horizontalPlateform.Left + horizontalPlateform.Width > this.ClientSize.Width) // Si la plate-forme horizontale touche le bord de la fenêtre
            {
                horizontalSpeed = -horizontalSpeed;
            }

            verticalPlateform.Top += verticalSpeed;

            if (verticalPlateform.Top < 200 || verticalPlateform.Top > 609) // Si la plate-forme verticale touche le bord de la fenêtre
            {
                verticalSpeed = -verticalSpeed;
            }

            ennemyOne.Left -= ennemyOneSpeed;

            if (ennemyOne.Left < pictureBox5.Left || ennemyOne.Left + ennemyOne.Width > pictureBox5.Left + pictureBox5.Width) // Si l'ennemi touche le bord de la plate-forme
            {
                ennemyOneSpeed = -ennemyOneSpeed;
                ennemyOneReverseImg = !ennemyOneReverseImg;
                if (ennemyOneReverseImg) // Inversez l'image de l'ennemi
                {
                    ennemyOne.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                else // Inversez l'image de l'ennemi
                {
                    ennemyOne.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
            }

            ennemyTwo.Left += ennemyTwoSpeed;

            if (ennemyTwo.Left < pictureBox2.Left || ennemyTwo.Left + ennemyTwo.Width > pictureBox2.Left + pictureBox2.Width) // Si l'ennemi touche le bord de la plate-forme
            {
                ennemyTwoSpeed = -ennemyTwoSpeed; 
                ennemyTwoReverseImg = !ennemyTwoReverseImg;
                if (ennemyTwoReverseImg)
                {
                    ennemyTwo.Image.RotateFlip(RotateFlipType.RotateNoneFlipX); // Inversez l'image de l'ennemi
                }
                else
                {
                    ennemyTwo.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
            }
            if (player.Bounds.IntersectsWith(door.Bounds) && score >= 26) // Si le joueur touche la porte et a collecté toutes les pièces
            {
                gameTimer.Stop();
                isGameOver = true; 
                txtscore.Text = "Score: " + score + Environment.NewLine + "You won the game !!";
            }
            else
            {
                txtscore.Text = "Score: " + score + Environment.NewLine + "You need to collect all the coins !!";
                if (player.Top + player.Height > this.ClientSize.Height + 50) // Si le joueur tombe dans le vide
                {
                    txtscore.Text = "Score: " + score + Environment.NewLine + "You fell in the void !!";
                    gameTimer.Stop();
                    isGameOver = true;
                }

            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e) // Lorsque l'utilisateur appuie sur une touche
        {
            if (e.KeyCode == Keys.Left) // Si la touche est la flèche gauche
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right) // Si la touche est la flèche droite
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && canJump) // Si la touche est la barre d'espace
            {
                jumping = true;
                canJump = false;
                jumpSpeed = -8;
            }

            // Si le joueur est sur l'échelle et maintient la touche d'espace enfoncée, il monte.
            if (onLadder && e.KeyCode == Keys.Space)
            {
                jumpSpeed = -8; // Ajustez la vitesse de montée si nécessaire.
            }
        }


        private void KeyIsUp(object sender, KeyEventArgs e) // Lorsque l'utilisateur relâche une touche
        {
            if (e.KeyCode == Keys.Left) 
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                jumping = false;
                jumpSpeed = 10;

                
            }

            if (e.KeyCode == Keys.Enter && isGameOver == true) // Si l'utilisateur appuie sur la touche Entrée et que le jeu est terminé
            {
                RestartGame();
            }
        }


        private void RestartGame() // Méthode pour redémarrer le jeu
        {
            jumping = false;
            goLeft = false;
            goRight = false;
            isGameOver = false;
            score = 0;

            txtscore.Text = "Score: " + score;

            foreach (Control x in this.Controls) // Réinitialisez la position de toutes les pièces
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

            // Réinitialisez la position de la plateforme verticale à un emplacement approprié.
            verticalPlateform.Left = 524; // Changez cette valeur en fonction de votre jeu.
            verticalPlateform.Top = 609;  // Changez cette valeur en fonction de votre jeu.

            // Redémarrez le gameTimer ici
            gameTimer.Start();

            // Arrêtez la musique lorsque le jeu se termine
            backgroundMusicPlayer.Stop();

            ennemyOne.Tag = "ennemy"; // Réinitialisez le tag de l'ennemi
            ennemyTwo.Tag = "ennemy"; // Réinitialisez le tag de l'ennemi
        }
    }
}
