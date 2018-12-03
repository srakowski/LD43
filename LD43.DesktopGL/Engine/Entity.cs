using System;
using System.Collections.Generic;
using System.Linq;
using LD43.Gameplay.Behaviors;

namespace LD43.Engine
{
    public class Entity
    {
        private IServiceProvider _services = null;

        private List<Component> _components = new List<Component>();

        public IEnumerable<Component> Components => _components.ToArray();

        public Entity Parent { get; set; }

        public Transform Transform { get; set; }

        public Entity()
        {
            AddComponent(new Transform());
        }

        public void AddComponent(Component component)
        {
            _components.Add(component);
            Transform = (component as Transform) ?? Transform;
            if (_services != null) component.Activate(this, _services);
        }

        public void RemoveComponent(Component component)
        {
            Transform = component == Transform ? null : Transform;
            _components.Remove(component);
        }

        internal void Activate(IServiceProvider services)
        {
            _services = services;
            var componentsToActivate = _components.ToList();
            componentsToActivate.ForEach(c => c.Activate(this, _services));
        }
    }
}
