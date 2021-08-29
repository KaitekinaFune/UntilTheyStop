namespace Pools
{
    public interface IFactory<T>
    {
        T Create();
    }
}