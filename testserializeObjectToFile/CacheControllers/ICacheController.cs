namespace TestSerializeObjectToFile.CacheControllers
{
    public interface ICacheController
    {
        bool Save<T>(T model, string fileName);
        T Load<T>(string fileName);
    }
}