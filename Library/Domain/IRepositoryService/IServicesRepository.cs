using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositoryService
{
    public interface IServicesRepository<T> where T:class
    {
        Task<List<T>> GetAll();
        Task<T> FindBy(Guid Id);
        Task<T> FindBy(string Name);
        Task<bool> Save(T model);
        Task<bool> Delete(Guid Id);


    }
}
