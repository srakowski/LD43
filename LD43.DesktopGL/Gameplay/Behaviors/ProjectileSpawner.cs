using System;
using System.Collections;
using LD43.Engine;
using Microsoft.Xna.Framework;

namespace LD43.Gameplay.Behaviors
{
    public class ProjectileSpawner : Behavior
    {
        private ProjectileSpawnerStrategy _spawnerStrategy;

        private SceneManager _sceneManager;

        private Func<Vector2, Vector2, Entity> _projectileFactory;

        public ProjectileSpawner(ProjectileSpawnerStrategy spawnerStrategy, Func<Vector2, Vector2, Entity> projectileFactory)
        {
            _spawnerStrategy = spawnerStrategy;
            _projectileFactory = projectileFactory;
        }

        public override void Initialize()
        {
            base.Initialize();
            _sceneManager = Services.GetService<SceneManager>();
            StartCoroutine(_spawnerStrategy.Begin(this));
        }

        public void Spawn(Vector2 direction)
        {
            _sceneManager.ActiveScene.AddEntity(_projectileFactory.Invoke(Entity.Transform.Position, direction));
        }
    }

    public abstract class ProjectileSpawnerStrategy
    {
        public abstract IEnumerator Begin(ProjectileSpawner projectileSpawner);
    }

    public class SequencedCircularProjectileSpawnerStrategy : ProjectileSpawnerStrategy
    {
        private int _count;

        private int _delay;

        private int _sequenceFrequencyInMS;

        private int _frequencyInMS;

        public SequencedCircularProjectileSpawnerStrategy(
            int count,
            int initialDelayInMS,
            int sequenceFrequencyInMS,
            int frequencyInMS)
        {
            _count = count;
            _delay = initialDelayInMS;
            _sequenceFrequencyInMS = sequenceFrequencyInMS;
            _frequencyInMS = frequencyInMS;
        }

        public override IEnumerator Begin(ProjectileSpawner projectileSpawner)
        {
            yield return WaitYieldInstruction.Create(_delay);
            while (true)
            {
                var degreesPerSpawn = 360 / _count;
                for (int r = 0; r < _count; r++)
                {
                    var a = degreesPerSpawn * r;
                    projectileSpawner.Spawn(Vector2Helper.FromAngle(MathHelper.ToRadians(a)));
                    yield return WaitYieldInstruction.Create(_sequenceFrequencyInMS);
                }
                yield return WaitYieldInstruction.Create(_frequencyInMS);
            }
        }
    }
}
