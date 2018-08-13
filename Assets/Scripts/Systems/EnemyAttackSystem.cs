using Components;
using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class EnemyAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameComponent _game;
        private PlayerComponent _player;
        private EcsWorld _world = null;
        private EcsFilterSingle<GameComponent> _gameFilter = null;
        private EcsFilterSingle<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<EnemyComponent> _enemyFilter = null;

        public void Initialize()
        {
            _game = _gameFilter.Data;
            _player = _playerFilter.Data;
        }

        public void Run()
        {
//            for (int i = 0; i < _collisionEvents.EntitiesCount; i++)
//            {
//                CollisionEvent collision = _collisionEvents.Components1[i];
//                if (!collision.Sender.CompareTag(Tags.EnemyTag)) continue;
//                for (int k = 0; k < _enemyFilter.EntitiesCount; k++)
//                {
//                    if (_collisionEvents.Components1[i].Sender != _enemyFilter.Components1[k].Head) continue;
//                    Attack(_enemyFilter.Components1[k]);
//                }
//            }
        }

        private void Attack(EnemyComponent enemy)
        {
//            HexaCoords coords = HexMath.Pixel2Hexel(enemy.Head.localPosition, _game.S.HexSize, 1);
//            var hexComponent = _game.Map[coords];
//            switch (hexComponent.HexType)
//            {
//                case HexTypes.Diamond:
//                    _player.Hp -= 1;
//                    break;
//                default:
//                    break;
//            }
        }

        public void Destroy()
        {
            _game = null;
            _player = null;
        }
    }
}