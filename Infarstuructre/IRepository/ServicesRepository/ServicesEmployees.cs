using Domin.Entity;
using Infarstuructre.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Infarstuructre.IRepository.ServicesRepository
{
    public class ServicesEmployees : IServicesAppRepository<Employees>
    {


        private readonly MyServiceDbContext _context;

        public ServicesEmployees(MyServiceDbContext context)
        {
            _context = context;
        }
      

       
        

        public bool Delete(int Id)
        {
            try
            {
                var result = FindBy(Id);
                if (result != null)
                {
                    result.CurrentStatus = (int)Helper.eCurrentState.Delete;
                    _context.employees.Update(result);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Employees FindBy(int Id)
        {
            try
            {
                return _context.employees.FirstOrDefault(x => x.Id == Id && x.CurrentStatus > 0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Employees FindBy(string Name)
        {
            try
            {
                return _context.employees.FirstOrDefault(x => x.Name.Equals(Name.Trim()) && x.CurrentStatus > 0);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Employees> GetAll()
        {
            try
            {
                return _context.employees.OrderBy(x => x.Name).Where(x => x.CurrentStatus > 0).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Save(Employees model)
        {
            try
            {
                var result = FindBy(model.Id);
                if (result == null)
                {

                    


                    model.CurrentStatus = (int)Helper.eCurrentState.Active;
                    _context.employees.Add(model);
                }
                else
                {
                    result.Name = model.Name;
                    result.Email = model.Email;
                    result.TitleResume = model.TitleResume;
                    result.PhoneNumber = model.PhoneNumber;
                   
                    result.CurrentStatus = (int)Helper.eCurrentState.Active;
                    _context.employees.Update(result);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        private string SaveFile(IFormFile file)
        {
            var filePath = Path.Combine("wwwroot/Image", file.FileName); // Define the file path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return filePath; // Return the file path to save it in the database
        }
    }
}
