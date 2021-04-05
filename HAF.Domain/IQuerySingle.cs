namespace  HAF.Domain
{
    public interface IQuerySingle<out TResult>
    {
        TResult Execute(int id);
    }
}