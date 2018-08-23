using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Tanks
{
    public delegate void STREEP();

    class Model
    {
        public event STREEP changeStreep;

        int collectedApples;
        int sizeField;
        int amountTanks;
        int amountApples;
        public int speedGame;

        int step;

        public GameStatus gameStatus;

        Random r;

        Projectile projectile;
        Packman packman;

        List<Tank> tanks;
        List<FireTank> fireTanks;
        List<Apple> apples;

        public Wall wall;

        internal List<Tank> Tanks
        {
            get
            {
                return tanks;
            }

        }

        internal List<Apple> Apples
        {
            get
            {
                return apples;
            }
        }

        internal Packman Packman
        {
            get
            {
                return packman;
            }
        }

        internal Projectile Projectile
        {
            get
            {
                return projectile;
            }
        }

        internal List<FireTank> FireTanks
        {
            get
            {
                return fireTanks;
            }
        }

        public Model(int sizeField, int amountTanks, int amountApples, int speedGame)
        {
            r = new Random();

            this.sizeField = sizeField;
            this.amountTanks = amountTanks;
            this.amountApples = amountApples;
            this.speedGame = speedGame;

            NewGame();

        }
                                                                
        private void CreatApples()
        {
            CreatApples(0);
        }
        private void CreatApples(int newApples)          // Создание оъектов яблок
        {
            int x, y;
            while (apples.Count < amountApples+newApples)
            {
                x = r.Next(6) * 40;
                y = r.Next(6) * 40;
                bool flag = true;

                foreach (Apple a in apples)
                    if (a.X == x && a.Y == y)
                    {
                        flag = false;
                        break;
                    }

                if (flag)
                    apples.Add(new Apple(x, y));
                }
            }

        private void CreatTanks()                        // Создание вражеских танков и охотника
        {
            int x, y;
            while (tanks.Count < amountTanks + 1)
            {
                do
                {
                    x = r.Next(6) * 40;
                    y = r.Next(6) * 40;
                }
                while ((x == 120 && y == 200) || (x == 80 && y == 200) || (x == 120 && y == 160) || (x == 160 && y == 200));

                bool flag;

                if (tanks.Count == 0)
                {
                    tanks.Add(new Hunter(sizeField, x, y));
                    flag = false;
                }
                else
                {
                    flag = true;

                    foreach (Tank t in tanks)
                        if (t.X == x && t.Y == y)
                        {
                            flag = false;
                            break;
                        }
                }

                if (flag)
                    tanks.Add(new Tank(sizeField, x, y));

            }
        }
                                                                
        public void Play()                               // Запуск работы логики игры
        {
            while (gameStatus==GameStatus.playing)
            {
                Thread.Sleep(speedGame);

                TryTurnAroundPackman();

                RunAllObjectsOnField();             
              
                TryDestroyTank();                   
                                
                IfCollisionOfTanks();               
                                
                IfCollisionOfTankAndPackman();      
                                
                IfPickApple();                      

                if (collectedApples > 4)
                {
                    gameStatus = GameStatus.winner;

                    if (changeStreep != null)
                        changeStreep();
                }


            }
        }
                                                                 
        private void TryTurnAroundPackman()              // Проверка необходимости резкого разворота пакмена
        {
            if (
                (packman.Direct_x == 0 && packman.PriorityNextDirect_x == 0 && packman.Direct_y != packman.PriorityNextDirect_y)
                ||
                (packman.Direct_y == 0 && packman.PriorityNextDirect_y == 0 && packman.Direct_x != packman.PriorityNextDirect_x)
               )
                packman.TurnAround();
        }
                                                              
        private void RunAllObjectsOnField()              // Запуск движения всех объектов на поле
        {
            projectile.Run();
            packman.Run();

            ((Hunter)tanks[0]).Run(packman.X, packman.Y);

            for (int i = 1; i < tanks.Count; i++)
                tanks[i].Run();

            for (int i = 0; i < fireTanks.Count; i++)
                fireTanks[i].Fire();
        }
                                                                
        private void IfPickApple()                       // Проверка подбора яблока
        {
            for (int i = 0; i < apples.Count; i++)
                if (Math.Abs(packman.X - apples[i].X) < 2 && Math.Abs(packman.Y - apples[i].Y) < 2)
                {
                    apples[i] = new Apple(step += 30, 270);
                    CreatApples(++collectedApples);
                }
        }
                                                                
        private void IfCollisionOfTankAndPackman()       // Проверка столкновения танка и пакмена
        {
            for (int i = 0; i < tanks.Count; i++)
                if (
                       ((Math.Abs(tanks[i].X - packman.X) <= 19) && (tanks[i].Y == packman.Y))
                       ||
                       ((Math.Abs(tanks[i].Y - packman.Y) <= 19) && (tanks[i].X == packman.X))
                       ||
                       ((Math.Abs(tanks[i].X - packman.X) <= 19) && (Math.Abs(tanks[i].Y - packman.Y) <= 19))
                    )
                {
                    gameStatus = GameStatus.loozer;

                    if (changeStreep != null)
                        changeStreep();
                }
        }
                                                                
        private void IfCollisionOfTanks()                // Проверка столкновения танков
        {
            for (int i = 0; i < tanks.Count - 1; i++)
                for (int j = i + 1; j < tanks.Count; j++)
                {
                    if (
                        ((Math.Abs(tanks[i].X - tanks[j].X) <= 20) && (tanks[i].Y == tanks[j].Y)) //Лобовое столкновение по горизонтали
                        ||
                        ((Math.Abs(tanks[i].Y - tanks[j].Y) <= 20) && (tanks[i].X == tanks[j].X)) //Лобовое столкновение по ветрикали
                       )
                    {
                        if (i == 0)
                            ((Hunter)tanks[i]).TurnAround();
                     
                        else
                            tanks[i].TurnAround();

                        tanks[j].TurnAround();
                        continue;
                    }

                    if (
                        ((Math.Abs(tanks[i].X - tanks[j].X) <= 20) && (Math.Abs(tanks[i].Y - tanks[j].Y) <= 20)) // Угловое столкновение
                       )
                    {
                        if (i == 0)
                        {
                            ((Hunter)tanks[i]).TurnAround();
                            ((Hunter)tanks[i]).Run();
                        }

                        else
                        {
                            tanks[i].TurnAround();
                            tanks[i].Run();
                        }

                        tanks[j].TurnAround();
                        tanks[j].Run();
                    }
                }

        }
                                                                
        private void TryDestroyTank()                    // Проверка столкновения танков и снаряда
        {
            for (int i = 1; i < tanks.Count; i++)
                if ((projectile.X - tanks[i].X) < 17 && (projectile.Y - tanks[i].Y) < 17
                    &&
                    (projectile.X - tanks[i].X) > 0 && (projectile.Y - tanks[i].Y) > 0
                    )
                {
                    fireTanks.Add(new FireTank(tanks[i].X, tanks[i].Y));
                    tanks.RemoveAt(i);
                    projectile.DefaultSettrigs();
                }
        }
                                                                
        internal void NewGame()                          // Создание или пересоздание объектов на поля для новой игры
        {
            collectedApples = 0;

            step = -30;

            projectile = new Projectile();
            packman = new Packman(sizeField);
            tanks = new List<Tank>();
            fireTanks = new List<FireTank>();
            apples = new List<Apple>();

            CreatTanks();
            CreatApples();
            wall = new Wall();



            gameStatus = GameStatus.stoping;
        }
    }
}
