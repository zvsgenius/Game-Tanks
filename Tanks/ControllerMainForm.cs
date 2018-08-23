using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

[assembly: CLSCompliant(true)]
namespace Tanks
{
    delegate void Invoker();

    public partial class ControllerMainForm : Form
    {
        View view;
        Model model;

        bool soundMode;                 // Содержит режим работы звука

        SoundPlayer sp;

        Thread modelPlay;

        static string textAbout = "Игра:    \t\t\"Танки\"" + Environment.NewLine +
                                  "Разработчик: \tВоробьев Александр" + Environment.NewLine + Environment.NewLine+

                                  "Цель игры:" + Environment.NewLine +
                                                   "\t\tДля победы нужно собрать 5 яблок" + Environment.NewLine + Environment.NewLine +

                                  "Управление:" + Environment.NewLine +
                                                   "\t\tНаправление движения изменяется кливишами" + Environment.NewLine +
                                                             "\t\t\t\"W\" \"S\" \"A\" \"D\"" + Environment.NewLine +
                                                   "\t\tВыстрел из пушки производится на клавиши " + Environment.NewLine +
                                                             "\t\t\t\"L\" или \"Q\"" + Environment.NewLine + Environment.NewLine +

                                  "Условия игры:" + Environment.NewLine +
                                                     "\t\tНельзя сталкиваться с вражескими танками."+ Environment.NewLine +
                                                     "\t\tКрасные танки можно уничтожать из пушки."+ Environment.NewLine +
                                                     "\t\t\"Охотника\" нельзя уничтожить." + Environment.NewLine +
                                                     "\t\tПри пересечении границы игрового поля танки"+ Environment.NewLine +
                                                     "\t\tпоявляются с другой стороны этого поля."; 

        public ControllerMainForm(): this(260) { }
        public ControllerMainForm(int sizeField) : this(sizeField, 5) { }
        public ControllerMainForm(int sizeField, int amountTanks) : this(sizeField, amountTanks, 5) { }
        public ControllerMainForm(int sizeField, int amountTanks, int amountApples) : this(sizeField, amountTanks, amountApples, 40) { }
        public ControllerMainForm(int sizeField, int amountTanks, int amountApples, int speedGame)
        {
            InitializeComponent();

            model = new Model(sizeField, amountTanks, amountApples, speedGame);

            
            model.changeStreep += new STREEP(ChangerStatusStripLbl);    //Подписываемся на событие изменения статуса игры

            view = new View(model);
            this.Controls.Add(view);

            soundMode = true;

            sp = new SoundPlayer(Properties.Resources.TankMov);
        }


        void ChangerStatusStripLbl()                    //Изменения статуса игры
        {
            Invoke(new Invoker(SetValueToStrLbl));

        }

        
        private void SetProjectileFromStart()           // Установка направления полёта снаряда
        {
            model.Projectile.Direct_x = model.Packman.Direct_x;
            model.Projectile.Direct_y = model.Packman.Direct_y;

            model.Projectile.Km = 0;

            if (model.Packman.Direct_y == -1)
            {
                model.Projectile.X = model.Packman.X + 8;
                model.Projectile.Y = model.Packman.Y;
            }

            if (model.Packman.Direct_y == 1)
            {
                model.Projectile.X = model.Packman.X + 8;
                model.Projectile.Y = model.Packman.Y + 20;
            }
            if (model.Packman.Direct_x == -1)
            {
                model.Projectile.X = model.Packman.X;
                model.Projectile.Y = model.Packman.Y + 8;
            }

            if (model.Packman.Direct_x == 1)
            {
                model.Projectile.X = model.Packman.X + 20;
                model.Projectile.Y = model.Packman.Y + 8;
            }
        }
        private void SetValueToStrLbl()                 //Переключает надпись в строке состояния и изменяет состояние звука
        {
            GameStatus_lbl_ststrp.Text = model.gameStatus.ToString();

            switch (model.gameStatus.ToString())
            {
                case "playing":
                    {
                        GameStatus_lbl_ststrp.ForeColor = SystemColors.ControlText;
                        GameStatus_lbl_ststrp.Text = "Идёт игра...";
                    }
                    break;
                case "loozer":
                    {
                        GameStatus_lbl_ststrp.ForeColor = Color.Red;
                        GameStatus_lbl_ststrp.Text = "Вы проиграли!";
                        OffStartPauseButton();
                    } break;
                case "stoping":
                    {
                        GameStatus_lbl_ststrp.ForeColor = SystemColors.ControlText;
                        GameStatus_lbl_ststrp.Text = "Игра приостановлена.";
                    }
                    break;
                case "winner":
                    {
                        GameStatus_lbl_ststrp.ForeColor = Color.Green;
                        GameStatus_lbl_ststrp.Text = "Вы победили!!!";
                        OffStartPauseButton();
                    }
                    break;
                default: break;
            }
                

            SoundChange();
        }
        private void OffStartPauseButton()              // Выключает видимость кнопки Start/Pause
        {
            StartStop_pcbx.Image = Properties.Resources.EndGameImg;
        }

        private void SoundChange()                      //Переключает состояние звука, в зависимости от режима звука и режима игры
        {
                if (soundMode && model.gameStatus == GameStatus.playing)
                    sp.PlayLooping();
                else
                    sp.Stop();

        }                                                                      

                                                                                  
        private void StopingGame()                                          // Остановка игры
        {
            model.gameStatus = GameStatus.stoping;
            modelPlay.Abort();
            StartStop_pcbx.Image = Properties.Resources.PlayButton;
        }
                                                                                                            
        private void Controller_MainForm_FormClosing(object sender, FormClosingEventArgs e)     // Обработка собятия закрытия окна игры
        {
            if (model.gameStatus == GameStatus.playing)
            {
                StopingGame();
                SoundChange();
                ChangerStatusStripLbl();
            }
            DialogResult dr = MessageBox.Show("Вы уверены, что хотите покинуть игру?", "Tanks", MessageBoxButtons.OKCancel,
                                              MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.OK)
                e.Cancel = false;
            else
                e.Cancel = true;
                
        }
                                                                                                           
        private void StartStop_pcbx_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)    // Отслеживание нажатия кнопок управления
        {
            switch (e.KeyData.ToString())
            {

                case "A":
                    {
                        model.Packman.PriorityNextDirect_x = model.Packman.NextDirect_x = -1;
                        model.Packman.PriorityNextDirect_y = model.Packman.NextDirect_y = 0; 
                    }
                    break;

                case "D":
                    {
                        model.Packman.PriorityNextDirect_x = model.Packman.NextDirect_x = 1;
                        model.Packman.PriorityNextDirect_y = model.Packman.NextDirect_y = 0;
                    }
                    break;

                case "W":
                    {
                        model.Packman.PriorityNextDirect_x = model.Packman.NextDirect_x = 0;
                        model.Packman.PriorityNextDirect_y = model.Packman.NextDirect_y = -1;
                    }
                    break;

                case "S":
                    {
                        model.Packman.PriorityNextDirect_x = model.Packman.NextDirect_x = 0;
                        model.Packman.PriorityNextDirect_y = model.Packman.NextDirect_y = 1;
                    }
                    break;

                case "Q":
                case "L": { SetProjectileFromStart(); } break;
                default: break;

            }
        } 

        private void StartPause_btn_Click(object sender, EventArgs e)// Обработка события нажатия кнопки Start/Pause
        {
            if (model.gameStatus == GameStatus.playing || model.gameStatus == GameStatus.stoping)
            {
                if (model.gameStatus == GameStatus.playing)
                {
                    StopingGame();
                }
                else
                {
                    StartStop_pcbx.Focus();
                    model.gameStatus = GameStatus.playing;
                    modelPlay = new Thread(model.Play);
                    modelPlay.Start();
                    StartStop_pcbx.Image = Properties.Resources.PauseButton;
                    view.Invalidate();
                }
                ChangerStatusStripLbl();
            }

        }
                                                                                                           

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)// Отслеживание нажатия 'Exit' в меню
        {
            Application.Exit();
        }
                                                                                                           
        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)// Отслеживание нажатия 'New Game' в меню
        {
            if (modelPlay != null)
            {
                StopingGame();
                SoundChange();
            }

            model.NewGame();
            GameStatus_lbl_ststrp.Text = null;
            view.Refresh();
        }
                                                                                                           
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)// Отслеживание нажатия 'About' в меню
        {
            if (model.gameStatus == GameStatus.playing)
            {
                StopingGame();
                SoundChange();
                ChangerStatusStripLbl();
            }
            MessageBox.Show(textAbout, "Tanks", MessageBoxButtons.OK,
                                              MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
        }
                                                                                                            
        private void soundToolStripMenuItem_Click(object sender, EventArgs e)// Отслеживание нажатия 'Sound' в меню
        {
            soundMode = !soundMode;
            if (soundMode)
            {
                soundToolStripMenuItem.Image = global::Tanks.Properties.Resources.SoundOn;
            }
            else
            {
                soundToolStripMenuItem.Image = global::Tanks.Properties.Resources.NoSound;
            }

            SoundChange();
        }
    }
}
