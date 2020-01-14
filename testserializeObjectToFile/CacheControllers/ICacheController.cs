namespace TestSerializeObjectToFile.CacheControllers
{
    public interface ICacheController
    {
        bool Save<T>(T model);
        T Load<T>();
    }
}