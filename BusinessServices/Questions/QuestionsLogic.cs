using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.Data.Context.Enums;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Questions
{
    public class QuestionsLogic : IQuestionsLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public QuestionsLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IQuestionsLogic implementation
        //get the list of questions by Set type Questionnaire
        public Data Index(int setType)
        {
            List<TS.Data.Context.Models.Questions> list = new List<TS.Data.Context.Models.Questions>();

            list = _dbContext.Questions
            .Where(q => q.SetType == (SetType)setType).OrderBy(q => q.ID).ToList();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get a question by id
        public Data Get(Guid questionId)
        {
            TS.Data.Context.Models.Questions question = _dbContext.Questions.First(q => q.ID == questionId);

            if (question == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("question_does_not_exist");
            }
            else
            {
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
                Data.Object = question;
            }

            return Data;
        }

        //edit a question
        public Data Edit(TS.Data.Context.Models.Questions question)
        {
            _dbContext.Entry(question).State = EntityState.Modified;
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("question_edited");
                Data.Object = question;
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

        //create a new question
        public Data Create(TS.Data.Context.Models.Questions question)
        {
            _dbContext.Questions.Add(question);
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("question_created");
                Data.Object = question;
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

        //delete a question
        public Data Delete(Guid questionId)
        {
            TS.Data.Context.Models.Questions question = _dbContext.Questions.Find(questionId);

            if (question == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("question_does_not_exist");
            }

            _dbContext.Questions.Remove(question);
            _dbContext.SaveChanges();

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("question_deleted");

            return Data;
        }

        #endregion
    }
}
