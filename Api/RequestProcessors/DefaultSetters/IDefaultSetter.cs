namespace Api.RequestProcessors.DefaultSetters
{
    public interface IDefaultSetter<T>
    {
        public T SetDefaultValues(T dto);
    }
}
