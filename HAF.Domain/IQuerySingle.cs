using HAF.Domain.Entities;

namespace  HAF.Domain
{
    public interface IQuerySingle
    {
        Asset Execute(int id);
    }
}