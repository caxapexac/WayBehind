using System;
using Client.Scripts.Components;
using Client.Scripts.ComponentsOneFrame;
using Client.Scripts.Miscellaneous;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Client.Scripts.Systems
{
    [EcsInject]
    [Obsolete]
    public class PlayerAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Slider _hpBar;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<CollisionEvent> _collisionEvents = null;
        private EcsFilter<EnemyComponent> _enemyFilter = null;
        private EcsUiEmitter _uiEmitter = null;

        public void Initialize()
        {
            _hpBar = _uiEmitter.GetNamedObject(Names.HpBar).GetComponent<Slider>();

            //_hpBar.maxValue = _player.Hp;
            //_hpBar.value = _player.Hp;
        }

        public void Run()
        {
            /*for (int i = 0; i < _collisionEvents.EntitiesCount; i++)
            {
                for (int k = 0; k < _enemyFilter.EntitiesCount; k++)
                {
                    if (_collisionEvents.Components1[i].Sender != _enemyFilter.Components1[k].Head) continue;
                    Attack(_enemyFilter.Components1[k]);
                }
            }*/
        }

        /*private void Attack(EnemyComponent enemy)
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
            enemy.Hex.Properties[HexProperties.Hp] -= 1;
            enemy.Body.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red,
                (float) (enemy.Hex.Properties[HexProperties.MaxHp] - enemy.Hex.Properties[HexProperties.Hp]) /
                enemy.Hex.Properties[HexProperties.MaxHp]);
        }*/


        public void Destroy()
        { }
    }
}