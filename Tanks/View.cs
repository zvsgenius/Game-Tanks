using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Tanks
{
    partial class View : UserControl
    {
        Model model;
        public View(Model model)
        {
            InitializeComponent();
            this.model = model;


        }

        void Draw(PaintEventArgs e)
        {
            DrawWall(e);
            DrawFiretank(e);
            DrawApple(e);
            DrawProjectile(e);
            DrawTank(e);
            DrawPackman(e);

            if (model.gameStatus != GameStatus.playing)
                return;
            Thread.Sleep(model.speedGame);
            Invalidate();
        }

        private void DrawFiretank(PaintEventArgs e)
        {
            foreach (FireTank ft in model.FireTanks)
                e.Graphics.DrawImage(ft.CurentImg, new Point(ft.X, ft.Y));
        }

        private void DrawProjectile(PaintEventArgs e)
        {
            e.Graphics.DrawImage(model.Projectile.Img, new Point(model.Projectile.X, model.Projectile.Y));
        }

        private void DrawPackman(PaintEventArgs e)
        {
            e.Graphics.DrawImage(model.Packman.CurrentImg, new Point(model.Packman.X, model.Packman.Y));
        }

        private void DrawApple(PaintEventArgs e)
        {
            for (int i = 0; i < model.Apples.Count; i++)
                e.Graphics.DrawImage(model.Apples[i].Img, new Point(model.Apples[i].X, model.Apples[i].Y));
        }

        private void DrawTank(PaintEventArgs e)
        {
            for (int i = 0; i < model.Tanks.Count; i++)
                e.Graphics.DrawImage(model.Tanks[i].CurrentImg, new Point(model.Tanks[i].X, model.Tanks[i].Y));
        }

        private void DrawWall(PaintEventArgs e)
        {
            for (int y = 20; y < 260; y += 40)
                for (int x = 20; x < 260; x += 40)
                    e.Graphics.DrawImage(model.wall.Img, new Point(x, y));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Draw(e);
        }

    }
}
