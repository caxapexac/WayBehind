using Client.Scripts.MonoBehaviours;
using Leopotam.Ecs;
using UnityEngine;


namespace Client.Scripts.Components
{
    public class PlayerComponent : IEcsAutoResetComponent
    {
        public MonoPlayer Parent;

//        public Vector2 Force = new Vector2();
//        public Vector2 CurrentForce = new Vector2();
//        public float CurrentSlowing = 0;
//        public float Slowing = 0;
        public int Hp = 1;
//        public int Exp = 0;
//        public int Kills = 0;

        public void Reset()
        {
            Parent = null;
        }
    }
}