using System.Collections.Generic;

namespace LD43.Engine
{
    public class Entity
    {
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
            component.Entity = this;
            _components.Add(component);
            Transform = (component as Transform) ?? Transform;
        }

        public void RemoveComponent(Component component)
        {
            Transform = component == Transform ? null : Transform;
            _components.Remove(component);
        }
    }
}
