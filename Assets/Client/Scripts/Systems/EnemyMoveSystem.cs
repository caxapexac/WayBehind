using System;
using Client.Scripts.Algorithms;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Client.Scripts.Scriptable;
using Leopotam.Ecs;
using Leopotam.Ecs.Ui.Systems;
using UnityEngine;
using UnityEngine.UI;


namespace Client.Scripts.Systems
{
    [EcsInject]
    [Obsolete]
    public class EnemyMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private float _maxDistance;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private EcsFilter<EnemyComponent> _enemyFilter = null;
        private SettingsObject _settings = null;
        private EcsUiEmitter _uiEmitter = null;

        public void Initialize()
        {
            //_maxDistance = _settings.FieldOfView * _settings.HexSize * 2;

            //Obsolete
            //_killedText = _uiEmitter.GetNamedObject(Names.Killed).GetComponent<Text>();
        }

        public void Run()
        {
            /*
            //_playerCoords = HexMath.Pixel2Hexel(_player.Transform.localPosition, _game.S.HexSize);
            for (int i = 0; i < _enemyFilter.EntitiesCount; i++)
            {
                EnemyComponent enemy = _enemyFilter.Components1[i];
                if (enemy.Hex.Properties[HexProperties.Hp] <= 0)
                {
                    _game.HexPool[(int) enemy.Hex.HexType].Recycle(enemy.Hex.Parent);
                    enemy.Hex.Parent = null;
                    enemy.Hex = null;
                    enemy.Head = null;
                    enemy.Body = null;
                    _world.RemoveEntity(_enemyFilter.Entities[i]);
                    _player.Kills += 1;
                    _killedText.text = _player.Kills.ToString();
                    continue;
                }

                if (Vector2.Distance(_player.Transform.localPosition, enemy.Head.localPosition) > _maxDistance)
                {
                    HexaCoords newCoords = HexMath.Pixel2Hexel(enemy.Head.localPosition, _game.S.HexSize, 1);
                    _game.MapComponent[newCoords] = enemy.Hex;
                    _game.HexPool[(int) enemy.Hex.HexType].Recycle(enemy.Hex.Parent);
                    enemy.Hex.Parent = null;
                    enemy.Hex = null;
                    enemy.Head = null;
                    enemy.Body = null;
                    _world.RemoveEntity(_enemyFilter.Entities[i]);
                }
                else
                {
                    ThinkAboutIt(enemy);
                }
            }*/
        }

        /*private void ThinkAboutIt(EnemyComponent enemy)
        {
            HexaCoords enemyPos = HexMath.Pixel2Hexel(enemy.Head.transform.localPosition, _game.S.HexSize);
            if (enemy.Force.magnitude > 0.5f)
            {
                Vector2 nextCoords = (Vector2) enemy.Head.localPosition + enemy.Force;
                enemy.Head.localPosition = Vector2.Lerp(enemy.Head.localPosition, nextCoords,
                    enemy.Hex.Properties[HexProperties.JumpSpeed] * Time.deltaTime);
                enemy.Force *= 0.5f;
                enemy.Target = nextCoords;
            }
            else if (HexMath.HexDistance(_playerCoords, enemyPos) <= enemy.Hex.Properties[HexProperties.AgroRadius])
            {
                //todo agro pathfinding
                enemy.Body.LookAt2D(_player.Transform);
                enemy.Head.localPosition = Vector2.Lerp(enemy.Head.localPosition, _player.Transform.localPosition,
                    enemy.Hex.Properties[HexProperties.AgroSpeed] * Time.deltaTime);
                enemy.Target = enemy.Head.localPosition;
            }
            else
            {
                if (Vector2.Distance(enemy.Head.localPosition, enemy.Target) < 0.1f)
                {
                    HexaCoords coords;
                    do
                    {
                        coords = HexMath.RandomPosition(enemyPos, 2, 1);
                    } while (_game.MapComponent[coords].HexType != HexTypes.Empty);

                    enemy.Target = HexMath.Hexel2Pixel(coords, _game.S.HexSize);
                }

                //todo pathfinding
                enemy.Body.LookAt2D(enemy.Target);
                enemy.Head.localPosition = Vector2.Lerp(enemy.Head.localPosition, enemy.Target,
                enemy.Hex.Properties[HexProperties.Speed] * Time.deltaTime);
            }


//			Transform enemyHead = enemy.HexComponent.Parent.PoolTransform;
//			Transform enemyBody = enemyHead.GetChild(0);
//			enemyHead.Translate((_player.Transform.localPosition - enemyHead.localPosition).normalized * Time.deltaTime);
            //enemyBody.localRotation.SetFromToRotation(enemyHead.localPosition, _player.Transform.localPosition);
        }*/

        public void Destroy()
        { }
    }
}