using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Universities
{
    public class UniversitiesLogic : IUniversitiesLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        public Data Data;
        #endregion

        #region Constructor
        public UniversitiesLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IUniversitiesLogic implementation
        //get all universities
        public Data Index()
        {
            List<TS.Data.Context.Models.University> univ =
            _dbContext.Universities.OrderBy(l => l.ID).ToList();
            List<JsTreeFormatDTO> list = new List<JsTreeFormatDTO>();

            foreach (TS.Data.Context.Models.University l in univ)
            {
                JsTreeFormatDTO model = new JsTreeFormatDTO();
                model.id = l.ID.ToString();
                model.text = l.Name;
                if (l.Parent == null)
                {
                    model.parent = "#";
                }
                else
                {
                    model.parent = l.Parent.ID.ToString();
                }
                list.Add(model);
            }

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = list;

            return Data;
        }

        //get a university by id
        public Data GetUniversity(int id)
        {
            TS.Data.Context.Models.University univ = _dbContext.Universities.Find(id);
            if (univ == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("This university does not exist!");
            }

            Data.Ok = true;
            Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            Data.Object = univ;

            return Data;
        }

        //create a new university
        public Data Create(JsTreeFormatDTO model)
        {
            TS.Data.Context.Models.University univ = new TS.Data.Context.Models.University();
            univ.Name = model.text;
            if (model.parent != "#")
            {
                univ.Parent = _dbContext.Universities.Find(Convert.ToInt32(model.parent));
            }
            else
            {
                univ.Parent = null;
            }
            _dbContext.Universities.Add(univ);
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("university_created");
                Data.Object = model;
            }
            catch (Exception e)
            {
                Data.Message = e.Message;
                Data.Ok = false;

            }
            return Data;
        }

        //update a university
        public Data Update(JsTreeFormatDTO model)
        {
            TS.Data.Context.Models.University univ = new TS.Data.Context.Models.University();
            univ.Name = model.text;
            univ.ID = Convert.ToInt32(model.id);
            if (model.parent != "#")
            {
                univ.Parent = _dbContext.Universities.Find(Convert.ToInt32(model.parent));
            }
            else
            {
                univ.Parent = null;
            }
            _dbContext.Entry(univ).State = EntityState.Modified;
            try
            {
                _dbContext.SaveChanges();
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("university_edited");
                Data.Object = model;
            }
            catch (Exception e)
            {
                Data.Message = e.Message;
                Data.Ok = false;

            }
            return Data;
        }

        //delete a university
        public Data Delete(int id)
        {
            TS.Data.Context.Models.University l = _dbContext.Universities.Find(id);
            if (l == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("university_does_not_exist");
            }

            var childrens = _dbContext.Universities.Where(x => x.Parent.ID == l.ID).ToList();

            if (childrens.Any())
            {
                Data.Ok = false;
                Data.Message = "Delete childs first!";
            }
            else
            {
                _dbContext.Universities.Remove(l);
                _dbContext.SaveChanges();

                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("university_deleted");
            }
            return Data;
        }

        #endregion
    }
}
