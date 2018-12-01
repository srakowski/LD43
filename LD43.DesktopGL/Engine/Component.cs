using System;

namespace LD43.Engine
{
    public abstract class Component
    {
        public Entity Entity { get; private set; }

        public IServiceProvider Services { get; private set; }

        internal void Activate(Entity entity, IServiceProvider services)
        {
            Entity = entity;
            Services = services;
            Initialize();
        }

        public virtual void Initialize() { }
    }
}
