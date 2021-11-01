namespace Business.Mappers
{
    public interface IMapper<TEntity, TRequest, TDetailedResult, TResult> 
        where TEntity : class
        where TRequest : class
        where TDetailedResult : class
        where TResult : class
    {
        public TDetailedResult EntityToDetailedDto(TEntity entity);
        public TEntity DtoToEntity(TRequest dto);
        public TResult EntityToDto(TEntity entity);
    }
}
