using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.Data.Context.Enums;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Answers
{
    public class AnswersLogic : IAnswersLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public AnswersLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;
            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IAnswersLogic implementation

        public Data GetQuestionnaireAnswersByStudentId(Guid userId)
        {
            //search in databse if exist already a set of answers for the current user
            var answers = _dbContext.StudentsTargets.Where(x => x.UserID == userId).ToList();

            List<StatisticHelpDTO> list = new List<StatisticHelpDTO>();
            foreach (var item in answers)
            {
                StatisticHelpDTO model = new StatisticHelpDTO();
                var statistic = _dbContext.Statistics.Where(x => x.ID == item.StatisticsID).FirstOrDefault();

                if (statistic.SetType == SetType.FirstSet)
                    model.SetType = TS.DTO.Enums.SetType.FirstSet;
                else if (statistic.SetType == SetType.SecondSet)
                    model.SetType = TS.DTO.Enums.SetType.SecondSet;
                else model.SetType = TS.DTO.Enums.SetType.ThirdSet;

                string name = (_dbContext.Users.Where(x => _dbContext.Statistics
                .Where(s => s.ID == item.StatisticsID)
                    .Any(s => s.UserID == x.ID))).FirstOrDefault().UserName;
                model.User = name.Split('.')[0] + " " + name.Split('.')[1];
                model.User_Id = statistic.ID;

                var answer = _dbContext.Answers
                  .Where(a => a.SetType == statistic.SetType && a.User_who_answer_ID == userId
                  && a.UserID == statistic.UserID).FirstOrDefault();

                //if exist, return message
                if (answer == null)
                {
                    list.Add(model);
                }
                else
                {
                    continue;
                }
            }

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //save answers for each questionnaire
        //any user can answer only once
        public Data Answer(TS.Data.Context.Models.Answers answersModel)
        {          

            //search in databse if exist already a set of answers for the current user
            var answer = _dbContext.Answers.Where(a => a.SetType == answersModel.SetType
            && a.User_who_answer_ID == answersModel.UserID && a.UserID == answersModel.UserID)
                .FirstOrDefault();

            //if exist, return message
            if (answer != null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("answers_already");
            }
            else
            {
                //save the new set of answers in database
                _dbContext.Answers.Add(answersModel);
                try
                {
                    _dbContext.SaveChanges();

                    Data.Ok = true;
                    Data.Message = _resourcesLoader.GetStringResourceByName("answers_success");
                    Data.Object = answersModel;
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;

                    //Data.Ok = false;
                    //Data.Message = raise.Message;
                }
            }

            return Data;
        }

        //for each user(teacher) get all answers from databse, for selected Set type
        public Data GetEmployeeAnswers(AnswersHelpDTO answersHelpDTO)
        {
            AnswersHelpDTO data = answersHelpDTO;
            var user = _dbContext.Users.Where(e => e.ID == data.User_Id).FirstOrDefault();
            if (user == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }
            else
            {
                var st = data.SetType;

                var list_answers =
                _dbContext.Answers.Where(a => a.SetType.ToString() == st.ToString() && a.UserID == user.ID).ToList();

                if (list_answers.Count() < 1)
                {
                    Data.Ok = false;
                    Data.Message = _resourcesLoader.GetStringResourceByName("user_no_answers");
                }
                else
                {
                    //the list of number of answers for each distinct questions, for current user
                    List<StatisticAnswersHelpDTO> list = new List<StatisticAnswersHelpDTO>();

                    int nr = 0;

                    foreach (var answer in list_answers)
                    {
                        StatisticAnswersHelpDTO sah = new StatisticAnswersHelpDTO();
                        sah.AnswerId = answer.ID;
                        nr += 1;
                        sah.Nr = nr;
                        var answers = answer.StatisticAnswers.ToList();

                        //count values from each answers
                        sah.n1 = answers.Where(x => x.value == 1).Select(x => x.value).Count();
                        sah.n2 = answers.Where(x => x.value == 2).Select(x => x.value).Count();
                        sah.n3 = answers.Where(x => x.value == 3).Select(x => x.value).Count();
                        sah.n4 = answers.Where(x => x.value == 4).Select(x => x.value).Count();
                        sah.n5 = answers.Where(x => x.value == 5).Select(x => x.value).Count();

                        int suma = sah.n1 + sah.n2 + sah.n3 + sah.n4 + sah.n5;
                        //if (suma >= 10)
                        //{
                            sah.P = (5 * sah.n5 + 4 * sah.n4 + 3 * sah.n3 + 2 * sah.n2 + sah.n1) / suma;
                        //}
                        if (sah.P >= 4.0 && sah.P <= 5.0)
                        {
                            sah.C = "FOARTE BINE";
                        }
                        else if (sah.P >= 2.5 && sah.P <= 3.99)
                        {
                            sah.C = "BINE";
                        }
                        else if (sah.P >= 1.25 && sah.P <= 2.49)
                        {
                            sah.C = "SATISFĂCATOR";
                        }
                        else if (sah.P < 1.25)
                        {
                            sah.C = "NESATISFĂCĂTOR";
                        }
                        list.Add(sah);
                    }

                    data.User = user.FirstName + " " + user.LastName;
                    data.User_Id = user.ID;
                    data.List_Statistics = list;
                    data.SetType = data.SetType;

                    Data.Ok = true;
                    Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
                    Data.Object = data;
                }
            }

            return Data;
        }

        public Data Delete(Guid answerId)
        {
            TS.Data.Context.Models.Answers questionnaire = _dbContext.Answers.FirstOrDefault(a => a.ID == answerId);

            if (questionnaire == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("statistic_does_not_exist");

            }
            _dbContext.Answers.Remove(questionnaire);

            _dbContext.SaveChanges();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("statistic_delete_success");

            return Data;
        }

        #endregion
    }
}
