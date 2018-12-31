using Client.Scripts.OBSOLETE.Components;
using Client.Scripts.OBSOLETE.Events;
using Client.Scripts.OBSOLETE.Misc;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Scripts.OBSOLETE.Systems
{
    [EcsInject]
    public class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private GameComponent _game;
        private PlayerComponent _player;
        private Slider _hpBar;
        private EcsWorld _world = null;
        private EcsFilter<GameComponent> _gameFilter = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<EnemyComponent> _enemyFilter = null;

        public void Initialize()
        {
            _game = _gameFilter.Components1[0];
            _player = _playerFilter.Components1[0];
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
            if (Random.value > _player.CurrentForce.magnitude - 0.3f)
            {
                _player.Hp -= 1;
                _hpBar.value = _player.Hp;
                if (_player.Hp <= 0)
                {
                    _player.Transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                    
                }
            }

            enemy.Force = _player.CurrentForce; //enemy.Head.localPosition - _player.Transform.localPosition;
            _player.CurrentForce = -Vector2.ClampMagnitude(_player.CurrentForce * 10, 1);
            _player.CurrentSlowing = 0;
            enemy.Hex.Properties[HexProperties.HP] -= 1;
            enemy.Body.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red,
                (float) (enemy.Hex.Properties[HexProperties.MaxHP] - enemy.Hex.Properties[HexProperties.HP]) /
                enemy.Hex.Properties[HexProperties.MaxHP]);
        }


        public void Destroy()
        {
            _game = null;
            _player = null;
        }
    }
}