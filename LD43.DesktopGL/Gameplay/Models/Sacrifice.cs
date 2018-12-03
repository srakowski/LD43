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
                timeInMS: 2 * 60 * 1000, 
                goldReq: 2, 
                soulsReq: 2,
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
                timeInMS: TimeRequiredInMilleseconds + 2,
                goldReq: this.GoldRequired + (int)(this.GoldRequired),
                soulsReq: this.SoulsRequired + (int)(this.SoulsRequired),
                gs: this._gs);
        }
    }
}
