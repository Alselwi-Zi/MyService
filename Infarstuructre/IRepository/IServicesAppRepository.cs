using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infarstuructre.IRepository
{
    public interface IServicesAppRepository<T> where T : class
    {
        List<T> GetAll();

        T FindBy(int Id);

        T FindBy(string Name);

        bool Save(T model);

        bool Delete(int Id);
    }
}
