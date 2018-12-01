using System.Collections.Generic;

namespace LD43.Engine
{
    public class Scene
    {
        private List<Entity> _entities = new List<Entity>();

        public IEnumerable<Entity> Entities => _entities.ToArray();

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}
