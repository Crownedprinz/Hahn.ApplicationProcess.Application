namespace  HAF.Domain
{
    public interface IQuery<in TParameters, out TResult>
    {
        TResult Execute(TParameters parameters);
    }
}