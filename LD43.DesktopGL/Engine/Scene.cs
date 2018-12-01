using System;
using System.Collections.Generic;
using System.Linq;

namespace LD43.Engine
{
    public class Scene
    {
        private IServiceProvider _services = null;

        private List<Entity> _entities = new List<Entity>();

        public IEnumerable<Entity> Entities => _entities.ToArray();

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
            if (_services != null)
            {
                entity.Activate(_services);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        internal void Activate(IServiceProvider services)
        {
            _services = services;
            var entitiesToActivate = _entities.ToList();
            entitiesToActivate.ForEach(e => e.Activate(_services));
        }
    }
}
