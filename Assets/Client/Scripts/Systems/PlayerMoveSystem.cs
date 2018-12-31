using Client.ScriptableObjects;
using Client.Scripts.Algorithms;
using Client.Scripts.Algorithms.Legacy;
using Client.Scripts.Components;
using Client.Scripts.Miscellaneous;
using Leopotam.Ecs;
using LeopotamGroup.Math;
using UnityEngine;


namespace Client.Scripts.Systems
{
    [EcsInject]
    public class PlayerMoveSystem : IEcsRunSystem
    {
        //private PlayerComponent _player;
        //private EcsFilter<TriggerEvent> _triggerFilter = null;

        private EcsWorld _world = null;
        private EcsFilter<PlayerComponent> _playerFilter = null;
        private SettingsObject _settings = null;

        public void Run()
        {
            for (int i = 0; i < _playerFilter.EntitiesCount; i++)
            {
                PlayerComponent player = _playerFilter.Components1[i];

                HexaCoords coords = HexMath.Pixel2Hexel(player.Transform.localPosition, _settings.HexSize);

                //TODO OBSOLETE
                //HexComponent hex = _game.Map[coords];
                //player.Slowing = hex.Slowing;

                if (player.Hp <= 0) player.CurrentSlowing *= 0.1f;

                player.CurrentForce =
                    Vector2.Lerp(player.CurrentForce, player.Force, _settings.LerpSpeed * Time.deltaTime);
                player.CurrentSlowing = MathFast.Lerp(player.CurrentSlowing, player.Slowing,
                    _settings.LerpSlowing * Time.deltaTime);
                Vector2 moveVector = (Vector2)player.Transform.localPosition
                    + player.CurrentForce * _settings.SpeedMultipiler * player.CurrentSlowing * Time.deltaTime;
                player.Transform.GetChild(0).transform.LookAt2D(moveVector);
                player.Transform.GetComponent<Rigidbody2D>().MovePosition(moveVector);

                //END OBSOLETE
            }
        }
    }
}