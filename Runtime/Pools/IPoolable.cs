namespace Pools
{
    public interface IPoolable
    {
        void OnTakeFromPool();
        void OnReturnToPool();
    }
}