namespace  HAF.Domain
{
    public interface ICommand<in TParameters>
    {
        void Execute(TParameters parameters);
    }
}