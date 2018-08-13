using Components;
using Events;
using Leopotam.Ecs;
using Misc;
using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    [EcsInject]
    public class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameComponent _game;
        private PlayerComponent _player;
        private Slider _hpBar;
        private EcsWorld _world = null;
        private EcsFilterSingle<GameComponent> _gameFilter = null;
        private EcsFilterSingle<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<EnemyComponent> _enemyFilter = null;

        public void Initialize()
        {
            _game = _gameFilter.Data;
            _player = _playerFilter.Data;
            _hpBar = _game.UI.GetNamedObject(Names.HpBar).GetComponent<Slider>();
            _hpBar.maxValue = _player.Hp;
            _hpBar.value = _player.Hp;
        }

        public void Run()
        {
            for (int i = 0; i < _collisionEvents.EntitiesCount; i++)
            {
                for (int k = 0; k < _enemyFilter.EntitiesCount; k++)
                {
                    if (_collisionEvents.Components1[i].Sender != _enemyFilter.Components1[k].Head) continue;
                    Attack(_enemyFilter.Components1[k]);
                }
            }
        }

        private void Attack(EnemyComponent enemy)
        {
            //HexaCoords coords = HexMath.Pixel2Hexel(enemy.Head.localPosition, _game.S.HexSize, 1);
            enemy.Force = _player.CurrentForce;//enemy.Head.localPosition - _player.Transform.localPosition;
            _player.CurrentSlowing = 0;
            enemy.Hex.Properties[HexProperties.HP] -= 1;
            if (Random.value > _player.CurrentForce.magnitude)
            {
                _player.Hp -= 1;
                _hpBar.value = _player.Hp;
            }
        }


        public void Destroy()
        {
            _game = null;
            _player = null;
        }
    }
}