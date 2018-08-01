using Systems;
using UnityEngine;

namespace Components
{
    sealed class PlayerComponent
    {
        public PlayerComponent()
        {
            Exp = 0;
            Hp = 100;
            Force = new Force();
        }

        public Transform Transform;
        //public Collider2D Collider;
        public Force Force;
        public int Hp;
        public int Exp;
    }
}