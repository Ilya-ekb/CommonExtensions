namespace Pools
{
    public interface IPoolService
    {
        T Get<T>() where T : class, IPoolable;
        void Return<T>(T obj) where T : class, IPoolable;
    }
}