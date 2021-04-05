namespace  HAF.Domain
{
    public interface ICreateCommand<in TParameters>
    {
        int Execute(TParameters parameters);
    }
}