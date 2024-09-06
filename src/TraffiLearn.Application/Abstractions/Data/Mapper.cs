namespace TraffiLearn.Application.Abstractions.Data
{
    public abstract class Mapper<TSource, TDestination>
    {
        public abstract TDestination Map(TSource source);

        public virtual IEnumerable<TDestination> Map(IEnumerable<TSource> sources)
        {
            return sources.Select(Map);
        }
    }
}
