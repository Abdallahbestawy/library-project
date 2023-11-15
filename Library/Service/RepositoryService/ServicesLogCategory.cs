using Domain.IRepositoryService;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.RepositoryService
{
    public class ServicesLogCategory : IServicesRepositoryLog<LogCategory>
    {
        private readonly LibraryContext context;

        public ServicesLogCategory(LibraryContext _context)
        {
            context = _context;
        }
        public async Task<List<LogCategory>> GetAll()
        {
            try
            {
                return await context.LogCategories.Include(c=>c.Category).OrderBy(d=>d.Date).ToListAsync();
            }
            catch(Exception)
            {
                return null;
            }
        }
        public async Task<LogCategory> FindBy(Guid Id)
        {
            try
            {
                return await context.LogCategories.Include(c=>c.Category).FirstOrDefaultAsync(l => l.Id.Equals(Id));
            }
            catch(Exception)
            {
                return null;
            }
        }
        public async Task<bool> Save(Guid Id, Guid UserId)
        {
            try
            {
                var logCatger = new LogCategory 
                {
                    Id = Guid.NewGuid(),
                    CategoryId = Id,
                    UserId = UserId,
                    Date= DateTime.Now,
                    Action = Helper.Save
                };
                context.LogCategories.Add(logCatger);
                context.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<bool> Update(Guid Id, Guid UserId)
        {
            try
            {
                var logCatger = new LogCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryId = Id,
                    UserId = UserId,
                    Date = DateTime.Now,
                    Action = Helper.Update
                };
                context.LogCategories.Add(logCatger);
                context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Delete(Guid Id, Guid UserId)
        {
            try
            {
                var logCatger = new LogCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryId = Id,
                    UserId = UserId,
                    Date = DateTime.Now,
                    Action = Helper.Save
                };
                context.LogCategories.Add(logCatger);
                context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteLog(Guid Id)
        {
            try
            {
                var model = await FindBy(Id);
                if (!model.Equals(null))
                {
                    context.LogCategories.Remove(model);
                    context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
