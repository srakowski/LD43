namespace LD43.Gameplay.Models
{
    public class Sacrifice
    {
        private GameplayState _gs;

        public Sacrifice(int timeInMS, int goldReq, int soulsReq, GameplayState gs)
        {
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
                timeInMS: 3 * 60 * 1000, 
                goldReq: 100, 
                soulsReq: 3,
                gs: gs
            );
        }
    }
}
