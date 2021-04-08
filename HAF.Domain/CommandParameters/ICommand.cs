using HAF.Domain.Entities;

namespace  HAF.Domain.CommandParameters
{
    public interface ICommand
    {
        void ExecuteAdd(Asset asset);
        void ExecuteUpdate(Asset asset);
        void ExecuteDelete(Asset asset);
    }
}