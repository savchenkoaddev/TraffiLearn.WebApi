namespace TraffiLearn.Application.Abstractions
{
    public interface IMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);

        IEnumerable<TDestination> Map(IEnumerable<TSource> sources);
    }
}
