namespace TraffiLearn.Application.Abstractions.Data
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);

        IEnumerable<TDestination> Map(IEnumerable<TSource> sources);
    }
}
