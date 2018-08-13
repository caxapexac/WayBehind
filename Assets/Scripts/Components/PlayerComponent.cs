using Systems;
using UnityEngine;

namespace Components
{
    sealed class PlayerComponent
    {
        public PlayerComponent()
        {
            Force = new Vector2();
            Slowing = 0;
            Hp = 1;
            Exp = 0;
            Kills = 0;
        }

        public Transform Transform;
        public Vector2 Force;
        public Vector2 CurrentForce;
        public float CurrentSlowing;
        public float Slowing;
        public int Hp;
        public int Exp;
        public int Kills;
    }
}