using Leopotam.Ecs;
using LeopotamGroup.Pooling;


namespace Client.Scripts.Components
{
    sealed class PoolsComponent : IEcsAutoResetComponent
    {
        public PoolContainer HexPool;
        public PoolContainer EnemyPool;

        public void Reset()
        {
            HexPool = null;
            EnemyPool = null;
        }
    }
}