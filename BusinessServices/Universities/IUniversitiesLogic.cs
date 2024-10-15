using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Universities
{
    public interface IUniversitiesLogic : IBusinessService
    {
        Data Create(JsTreeFormatDTO model);
        Data Update(JsTreeFormatDTO model);
        Data Delete(int id);
        Data GetUniversity(int id);
        Data Index();
    }
}
