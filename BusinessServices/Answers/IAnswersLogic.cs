using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DTO;

namespace BusinessServices.Answers
{
    public interface IAnswersLogic : IBusinessService
    {
        Data Answer(TS.Data.Context.Models.Answers answersModel);
        Data Delete(Guid answerId);
        Data GetEmployeeAnswers(AnswersHelpDTO answersHelpDTO);
        Data GetQuestionnaireAnswersByStudentId(Guid userId);
    }
}
