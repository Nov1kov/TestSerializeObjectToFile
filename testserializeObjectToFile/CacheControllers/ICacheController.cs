namespace TestSerializeObjectToFile.CacheControllers
{
    public interface ICacheController
    {
        public bool Save<T>(T model, string fileName);
        public T Load<T>(string fileName);
    }
}