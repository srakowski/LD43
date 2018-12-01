namespace LD43
{
    class Player
    {
        public int HP { get; private set; }

        public int MaxHP { get; private set; }

        public PlayerState State { get; private set; }

        public Transform Transform { get; private set; }

        public Direction Facing { get; private set; }

        public void Hit(Direction fromSide, int forThisMuchDamage) => State.Hit(fromSide, forThisMuchDamage);

        public void Move(Direction direction) => State.Move(direction);

        public void SwingWeapon() => State.SwingWeapon();

        public sealed class Invulnerable : PlayerState
        {
            public Invulnerable(Player player) : base(player) { }
        }
    }

    enum Direction
    {
        Left,
        Right
    }

    abstract class PlayerState
    {
        private readonly Player _player;

        protected PlayerState(Player player)
        {
            _player = player;
        }

        public virtual void Hit(Direction fromSide, int forThisMuchDamage) { }

        public virtual void Move(Direction direction) { }

        public virtual void SwingWeapon() { }
    }
}
