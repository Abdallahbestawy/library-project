using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositoryService
{
    public interface IServicesRepositoryLog<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> FindBy(Guid Id);
        Task<bool> Save(Guid Id,Guid UserId);
        Task<bool> Update(Guid Id,Guid UserId);
        Task<bool> Delete(Guid Id, Guid UserId);
        Task<bool> DeleteLog(Guid Id);
    }
}
