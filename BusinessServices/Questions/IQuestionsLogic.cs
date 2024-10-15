using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Questions
{
    public interface IQuestionsLogic : IBusinessService
    {
        Data Create(TS.Data.Context.Models.Questions question);
        Data Delete(Guid questionId);
        Data Edit(TS.Data.Context.Models.Questions question);
        Data Get(Guid questionId);
        Data Index(int setType);
    }
}
