using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.Data.Context.Enums;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Statistics
{
    public class StatisticsLogic : IStatisticsLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public StatisticsLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IStatisticsLogic implementation
        //create a new questionnaire
        public Data Create(TS.Data.Context.Models.Statistics model)
        {
            _dbContext.Statistics.Add(model);
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("questionnaire_created");
                Data.Object = model;
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
            return Data;
        }

        //get all questionnaires knowing Set Type
        public Data Index(int setType)
        {
            List<StatisticHelpDTO> model = new List<StatisticHelpDTO>();

            //from questionnaire database table, select all questionnaires for the current Set Type
            model = _dbContext.Statistics.Where(s => s.SetType == (SetType)setType)
                //convert Statistics model to StatisticHelp model
                .Select(x => new StatisticHelpDTO
                {
                    Id = x.ID,
                    AddDate = x.AddDate.Value.Month + "/" + x.AddDate.Value.Day + "/" + x.AddDate.Value.Year,
                    SetType = (TS.DTO.Enums.SetType)setType,
              //get the user name from users table, knowing userId
              User = _dbContext.Users.Where(e => e.ID == x.UserID).FirstOrDefault().FirstName + " "
                + _dbContext.Users.Where(e => e.ID == x.UserID).FirstOrDefault().LastName,

                    CountStudents = _dbContext.StudentsTargets.Where(c => c.StatisticsID == x.ID).ToList().Count()
                })
            .OrderByDescending(s => s.Id)
            .ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = model;

            return Data;
        }

        //get all questionnaires knowing Teacher id
        public Data TeacherQuestionnaires(Guid teacherId)
        {
            List<StatisticHelpDTO> model = new List<StatisticHelpDTO>();

            //from questionnaire database table, select all questionnaires for the current Set Type
            model = _dbContext.Statistics.Where(s => s.UserID==teacherId)
                //convert Statistics model to StatisticHelp model
                .Select(x => new StatisticHelpDTO
                {
                    Id = x.ID,
                    AddDate = x.AddDate.Value.Month + "/" + x.AddDate.Value.Day + "/" + x.AddDate.Value.Year,
                    SetType = (TS.DTO.Enums.SetType)x.SetType,
                    //get the user name from users table, knowing userId
                    User = _dbContext.Users.Where(e => e.ID == teacherId).FirstOrDefault().FirstName + " "
                + _dbContext.Users.Where(e => e.ID == teacherId).FirstOrDefault().LastName,

                    CountStudents = _dbContext.StudentsTargets.Where(c => c.StatisticsID == x.ID).ToList().Count()
                })
            .OrderByDescending(s => s.Id)
            .ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = model;

            return Data;
        }

        //get all statistics about teachers knowing university
        public List<TeachersStatisticsDTO> TeachersStatisticsByUniversity(SearchModelDTO model)
        {
            List<TeachersStatisticsDTO> list = new List<TeachersStatisticsDTO>();
            List<TeachersStatisticsDTO> list_distinct = new List<TeachersStatisticsDTO>();

            if (model.ids_int != null)
            {
                foreach(int id in model.ids_int)
                {
                    var l = _dbContext.UserUniversities.Where(u => u.UniversityID == id)
                        .Join(_dbContext.UserGroups.Where(g=>g.Group.Name=="Teacher" && g.User.Enable==true)
                        ,u=>u.UserID,g=>g.UserID,(u,g)=>new TeachersStatisticsDTO()
                        {
                            TeacherID=u.UserID,
                            TeacherName=g.User.UserName
                        }).ToList();

                    list.AddRange(l);
                }

                list_distinct = list.DistinctBy(x => x.TeacherID).ToList();

                foreach(var teacher in list_distinct)
                {
                    double p1 = GetResult(SetType.FirstSet, teacher.TeacherID);
                    double p2 = GetResult(SetType.SecondSet, teacher.TeacherID);

                    teacher.P = (p1 + p2) / 2;
                    teacher.C = teacher.P!=0 ? GetQualifying(teacher.P) : String.Empty;
                }
            }           

            //Data.Ok = true;
            //Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            //Data.Object = list_distinct;

            return list_distinct;
        }

        //delete a questionnaire
        public Data Delete(Guid id)
        {
            TS.Data.Context.Models.Statistics questionnaire = _dbContext.Statistics
             .FirstOrDefault(s => s.ID == id);

            if (questionnaire == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("questionnaire_does_not_exist");
            }
            _dbContext.Statistics.Remove(questionnaire);

            _dbContext.SaveChanges();
            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("questionnaire_deleted");

            return Data;
        }

        //get details for a questionnaire
        public Data Details(StatisticHelpDTO model)
        {
            TS.Data.Context.Models.Statistics survey =
              _dbContext.Statistics.Where(x => x.ID == model.Id && x.SetType.ToString() == model.SetType.ToString())
                  .FirstOrDefault();
            if (survey == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("questionnaire_does_not_exist");
            }
            else
            {
                model.Id = survey.ID;
                model.AddDate = survey.AddDate.Value.Month + "/" + survey.AddDate.Value.Day + "/" + survey.AddDate.Value.Year;

                if (survey.SetType == SetType.FirstSet)
                    model.SetType = TS.DTO.Enums.SetType.FirstSet;
                else if (survey.SetType == SetType.SecondSet)
                    model.SetType = TS.DTO.Enums.SetType.SecondSet;
                else model.SetType = TS.DTO.Enums.SetType.ThirdSet;

                //get the user name knowing user id
                model.User = _dbContext.Users.Where(e => e.ID == survey.UserID).FirstOrDefault().FirstName
                    + " " + _dbContext.Users.Where(e => e.ID == survey.UserID).FirstOrDefault().LastName;
                //get the students name
                model.List_Students = (_dbContext.Users.Where(d => _dbContext.StudentsTargets
                    .Where(c => c.StatisticsID == survey.ID)
                    .Any(c => c.UserID == d.ID)).ToList())
                    .Select(l => new StudentsTargetsHelpDTO
                    {
                        Id = l.ID,
                        User_Name = l.FirstName + " " + l.LastName
                    }).ToList();
                //get the questions for questionnaire
                model.List_Questions = (_dbContext.Questions
                .Where(q => _dbContext.StatisticQuestions.Where(s => s.StatisticsID == survey.ID)
                    .Any(s => s.QuestionsID == q.ID)).ToList())
                    .Select(m => new StatisticQuestionHelpDTO
                    {
                        Id = m.ID,
                        Question_Name = m.QuestionName,
                        A1 = m.A1,
                        A2 = m.A2,
                        A3 = m.A3,
                        A4 = m.A4,
                        A5 = m.A5,
                        V1 = m.V1,
                        V2 = m.V2,
                        V3 = m.V3,
                        V4 = m.V4,
                        V5 = m.V5
                    }).ToList();

                model.User_Id = survey.UserID;
                Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
                Data.Object = model;
                Data.Ok = true;

            }
            return Data;
        }


        #endregion

        private double GetResult(SetType setType,Guid teacherId)
        {
            var list_answers =
            _dbContext.Answers.Where(a => a.SetType.ToString() == setType.ToString()
            && a.UserID == teacherId).ToList();

            if (list_answers.Count() < 1)
            {
                return 0;
            }
            else
            {
                double result = 0;
                int count = 0;

                foreach (var answer in list_answers)
                {
                    var answers = answer.StatisticAnswers.ToList();

                    //count values from each answers
                    int n1 = answers.Where(x => x.value == 1).Select(x => x.value).Count();
                    int n2 = answers.Where(x => x.value == 2).Select(x => x.value).Count();
                    int n3 = answers.Where(x => x.value == 3).Select(x => x.value).Count();
                    int n4 = answers.Where(x => x.value == 4).Select(x => x.value).Count();
                    int n5 = answers.Where(x => x.value == 5).Select(x => x.value).Count();

                    int suma = n1 + n2 + n3 + n4 + n5;
                    
                    double p = (5 * n5 + 4 * n4 + 3 * n3 + 2 * n2 + n1) / suma;

                    result += p;
                    count += 1;
                    
                }

                return result/count;
            }
        }

        private string GetQualifying(double result)
        {
            string c = String.Empty;

            if (result >= 4.0 && result <= 5.0)
            {
                c = "FOARTE BINE";
            }
            else if (result >= 2.0 && result <= 3.99)
            {
                c = "BINE";
            }
            else if (result >= 1.0 && result <= 1.99)
            {
                c = "SATISFĂCATOR";
            }
            else if (result < 0.99)
            {
                c = "NESATISFĂCĂTOR";
            }

            return c;
        }
    }
}
