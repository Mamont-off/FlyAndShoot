namespace Common
{
    public interface IPool<T>
    {
        T GetOnPool();
        void SetToPool(T go);
    }
}