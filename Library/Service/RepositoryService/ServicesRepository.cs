using Domain.IRepositoryService;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Service.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Service.RepositoryService
{
    public class ServicesRepository : IServicesRepository<Category>
    {
        private readonly LibraryContext context;

        public ServicesRepository(LibraryContext _context)
        {
            context = _context;
        }

        public async Task<List<Category>> GetAll()
        {
            try
            {
                return  await context.Categories.OrderBy(x=>x.Name).Where(c=>c.CurrentState>0).ToListAsync();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public  async Task<Category> FindBy(Guid Id)
        {
            try
            {
                return await context.Categories.FirstOrDefaultAsync(c => c.Id == Id && c.CurrentState>0); 
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Category> FindBy(string Name)
        {
            try
            {
                return await context.Categories.FirstOrDefaultAsync(c => c.Name.Equals(Name.Trim()) && c.CurrentState > 0);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<bool> Save(Category model)
        {
            try
            {
                var newmodel = await FindBy(model.Id);
                if (newmodel == null)
                {
                    model.Id=Guid.NewGuid();
                    model.CurrentState = (int)Helper.eCurrentState.Active;
                    context.Categories.Add(model);  
                }
                else
                {
                    newmodel.Description = model.Description;
                    newmodel.Name = model.Name;
                    newmodel.CurrentState = (int)Helper.eCurrentState.Active;
                    context.Categories.Update(newmodel);
                }
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Delete(Guid Id)
        {
            try
            {
                var model = await FindBy(Id);
                model.CurrentState = (int) Helper.eCurrentState.Delete;
                context.Update(model);
                await context.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
