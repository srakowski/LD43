using System;

namespace LD43.Gameplay.Models
{
    public class Sacrifice
    {
        private GameplayState _gs;

        private int _level;

        public Sacrifice(int level, int timeInMS, int goldReq, int soulsReq, GameplayState gs)
        {
            _level = level;
            TimeRequiredInMilleseconds = timeInMS;
            GoldRequired = goldReq;
            SoulsRequired = soulsReq;
            _gs = gs;
        }

        public int TimeRequiredInMilleseconds { get; }

        public int GoldRequired { get; }

        public int SoulsRequired { get; }

        public static Sacrifice CreateFirst(GameplayState gs)
        {
            return new Sacrifice(
                level: 1,
                timeInMS: 3 * 60 * 1000, 
                goldReq: 100, 
                soulsReq: 10,
                gs: gs
            );
        }

        public Sacrifice Next()
        {
            var newLevel = _level + 1;

            if (newLevel == 5)
            {
                return null;
            }

            return new Sacrifice(
                newLevel,
                timeInMS: (3 + _level) * 60 * 1000,
                goldReq: this.GoldRequired + (int)(this.GoldRequired * (newLevel / 100f)),
                soulsReq: this.SoulsRequired + (int)(this.SoulsRequired * (newLevel / 100f)),
                gs: this._gs);
        }
    }
}
