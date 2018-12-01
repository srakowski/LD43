using Microsoft.Xna.Framework;

namespace LD43.Engine
{
    public class Transform : Component
    {
        private Transform Parent => Entity?.Parent?.Transform;

        public Vector2 Position
        {
            get { return Vector2.Transform(LocalPosition, Parent?.TransformationMatrix ?? Matrix.Identity); }
            set { LocalPosition = Vector2.Transform(value, Parent?.InvertedTransformationMatrix ?? Matrix.Identity); }
        }

        public Vector2 LocalPosition { get; set; } = Vector2.Zero;

        public float Rotation
        {
            get { return LocalRotation + (Parent?.Rotation ?? 0f); }
            set { LocalRotation = value - (Parent?.Rotation ?? 0f); }
        }

        public float LocalRotation { get; set; } = 0f;

        public float Scale
        {
            get { return LocalScale + (Parent?.Scale - 1f ?? 0f); }
            set { LocalScale = value - (Parent?.Scale - 1f ?? 0f); }
        }

        public float LocalScale { get; set; } = 1f;

        public Matrix TransformationMatrix =>
            Matrix.Identity *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Scale) *
            Matrix.CreateTranslation(Position.X, Position.Y, 0f);

        internal Matrix InvertedTransformationMatrix =>
            Matrix.Invert(TransformationMatrix);
    }
}
