using Systems;
using UnityEngine;

namespace Components
{
    sealed class PlayerComponent
    {
        public Transform Transform;
        public Collider2D Collider;
        public Force Force;
        public int Hp;
        public int Exp;
    
    }
}
