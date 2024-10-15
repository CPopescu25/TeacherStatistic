using CommonCfg;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context;
using TS.Data.Context.Enums;
using TS.Data.Context.Models;
using TS.DTO;
using TS.Resources.Services;

namespace BusinessServices.Install
{
    public class InstallLogic : IInstallLogic
    {
        #region Fields
        private readonly TSClientContext _dbContext;
        private readonly IResourcesLoader _resourcesLoader;

        private string FileName = AppDomain.CurrentDomain.BaseDirectory;
        private StreamReader sr;
        public Application excel;
        public Workbook workbook;
        public Worksheet worksheet;
        private int iRow;

        public Data Data;
        #endregion

        #region Constructor
        public InstallLogic(TSClientContext dbContext,
          IResourcesLoader resourcesLoader)
        {
            _dbContext = dbContext;
            _resourcesLoader = resourcesLoader;

            excel = new Application();
            Data = new Data();
            Data.Object = null;
        }
        #endregion

        #region IInstallLogic implementation

        public Data CkeckDatasInDatabase()
        {
            var list = _dbContext.Users.AsNoTracking().ToList();

            if (list.Any())
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("install_exist_datas");
            }
            else
            {
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("install_doesnt_exist_datas");
            }

            return Data;
        }

        #region Install Questions from Excel
        public bool Questions()
        {
            workbook = null;
            worksheet = null;
            iRow = 2;

            //install questions
            workbook = excel.Workbooks.Open(FileName + "Install\\Questions.xlsx");
            worksheet = (Worksheet)workbook.Sheets["Questions"];
            var array = new[] { "Id", "Name", "A1", "A2", "A3", "A4", "A5", "V1", "V2", "V3", "V4", "V5", "Type" };

            while (worksheet.Cells[iRow, GetColumnIndex(array, "Id")].Value != null)
            {
                TS.Data.Context.Models.Questions questions = new TS.Data.Context.Models.Questions();

                questions.ID = Guid.NewGuid();
                questions.QuestionName = worksheet.Cells[iRow, GetColumnIndex(array, "Name")].Value as string;
                questions.A1 = worksheet.Cells[iRow, GetColumnIndex(array, "A1")].Value as string;
                questions.A2 = worksheet.Cells[iRow, GetColumnIndex(array, "A2")].Value as string;
                questions.A3 = worksheet.Cells[iRow, GetColumnIndex(array, "A3")].Value as string;
                questions.A4 = worksheet.Cells[iRow, GetColumnIndex(array, "A4")].Value as string;
                questions.A5 = worksheet.Cells[iRow, GetColumnIndex(array, "A5")].Value as string;
                questions.V1 = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "V1")].Value);
                questions.V2 = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "V2")].Value);
                questions.V3 = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "V3")].Value);
                questions.V4 = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "V4")].Value);
                questions.V5 = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "V5")].Value);
                questions.SetType = (SetType)Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(array, "Type")].Value);

                _dbContext.Questions.Add(questions);
                try
                {
                    _dbContext.SaveChanges();
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
                }

                iRow++;
            }
            return true;
        }
        #endregion

        #region Install Universities from Excel
        public bool Universities()
        {
            workbook = null;
            worksheet = null;
            iRow = 2;

            //install universities
            workbook = excel.Workbooks.Open(FileName + "Install\\Universitati.xlsx");
            worksheet = (Worksheet)(workbook.Sheets["Universitati"]);
            var properties = new[] { "Id", "Name", "Parent" };

            while (worksheet.Cells[iRow, GetColumnIndex(properties, "Id")].Value != null)
            {
                University university = new University();

                university.ID = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "Id")].Value);
                university.Name = worksheet.Cells[iRow, GetColumnIndex(properties, "Name")].Value as string;
                if (Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "Parent")].Value) != 0)
                {
                    int id = Convert.ToInt32(worksheet.Cells[iRow, GetColumnIndex(properties, "Parent")].Value);
                    university.Parent = _dbContext.Universities.Find(id);

                }
                else
                {
                    university.Parent = null;
                }
                _dbContext.Universities.Add(university);
                try
                {
                    _dbContext.SaveChanges();
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
                }

                iRow++;
            }
            return true;
        }
        #endregion

        #region Install Groups
        public bool Group()
        {
            Group groupAdmin = new Group();
            //install group administrator
            groupAdmin.ID = Guid.NewGuid();
            groupAdmin.Name = "Admin";
            _dbContext.Groups.Add(groupAdmin);

            _dbContext.SaveChanges();

            Group groupTeacher = new Group();
            groupTeacher.ID = Guid.NewGuid();
            groupTeacher.Name = "Teacher";
            _dbContext.Groups.Add(groupTeacher);

            _dbContext.SaveChanges();

            Group groupStudent = new Group();
            groupStudent.ID = Guid.NewGuid();
            groupStudent.Name = "Student";
            _dbContext.Groups.Add(groupStudent);

            _dbContext.SaveChanges();

            return true;
        }
        #endregion

        #region Install User Admin from Csv
        public bool Admin()
        {
            //install user admin
            sr = new StreamReader(FileName + "Install\\Admin.csv");

            while (!sr.EndOfStream)
            {
                TS.Data.Context.Models.User user = new TS.Data.Context.Models.User();

                string[] splitRow = sr.ReadLine().Split(',');

                user.ID = Guid.NewGuid();
                user.UserName = splitRow[1];
                user.Password = splitRow[2];
                user.Enable = Convert.ToBoolean(splitRow[3]);
                user.Email = "cozmycosminutza@yahoo.com";
                user.Phone = "0758087511";

                _dbContext.Users.Add(user);

                _dbContext.SaveChanges();
                

                //install user group for admin
                UserGroup ug = new UserGroup();
                ug.GroupID = _dbContext.Groups.FirstOrDefault(g => g.Name == "Admin").ID;
                ug.UserID = user.ID;

                _dbContext.UserGroups.Add(ug);
                _dbContext.SaveChanges();

                ////install user group for teacher
                //UserGroup ug2 = new UserGroup();
                //ug2.GroupID = _dbContext.Groups.FirstOrDefault(g => g.Name == "Teacher").ID;
                //ug2.UserID = user.ID;

                //_dbContext.UserGroups.Add(ug2);
                //_dbContext.SaveChanges();

                ////install user group for student
                //UserGroup ug3 = new UserGroup();
                //ug3.GroupID = _dbContext.Groups.FirstOrDefault(g => g.Name == "Student").ID;
                //ug3.UserID = user.ID;

                //_dbContext.UserGroups.Add(ug3);
                //_dbContext.SaveChanges();
            }

            sr.Close();

            return true;
        }
        #endregion

        #region export to Excel

        public bool ExportStatisticsById(List<StatisticAnswersHelpDTO> list, string name)
        {
            workbook = null;
            worksheet = null;
            iRow = 2;

            var wbs = excel.Workbooks;
            workbook = wbs.Add(XlSheetType.xlWorksheet);
            worksheet = (Worksheet)excel.ActiveSheet;
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                worksheet.Cells[1, "A"] = "Nr.chestionar";
                worksheet.Cells[1, "B"] = "n5";
                worksheet.Cells[1, "C"] = "n4";
                worksheet.Cells[1, "D"] = "n3";
                worksheet.Cells[1, "E"] = "n2";
                worksheet.Cells[1, "F"] = "n1";
                worksheet.Cells[1, "G"] = "Punctaj rezultat [P]";
                worksheet.Cells[1, "H"] = "Calificativul rezultat";
                worksheet.Cells[1, "I"] = "Statistic type";

                // ------------------------------------------------
                // Populate sheet with some real data from "cars" list
                // ------------------------------------------------
                // start row (in row 1 are header cells)
                foreach (var a in list)
                {
                    worksheet.Cells[iRow, "A"] = a.Nr;
                    worksheet.Cells[iRow, "B"] = a.n5;
                    worksheet.Cells[iRow, "C"] = a.n4;
                    worksheet.Cells[iRow, "D"] = a.n3;
                    worksheet.Cells[iRow, "E"] = a.n2;
                    worksheet.Cells[iRow, "F"] = a.n1;
                    worksheet.Cells[iRow, "G"] = a.P;
                    worksheet.Cells[iRow, "H"] = a.C;
                    worksheet.Cells[iRow, "I"] = a.SetType;

                    iRow++;
                }

                //// Apply some predefined styles for data to look nicely :)
                //workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                //// Define filename
                string fileName = string.Format(@"{0}\" + name + ".xlsx", FileName + "StatisticsExcel");

                //// Save this data as a file
                worksheet.SaveAs(fileName);

                //// Display SUCCESS message
                //MessageBox.Show(string.Format("The file '{0}' is saved successfully!", fileName));
                return true;
            }
            catch (Exception)
            {
                //MessageBox.Show("Exception",
                //    "There was a PROBLEM saving Excel file!\n" + exception.Message,
                //    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                //// Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (worksheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);

                //// Empty variables
                excel = null;
                worksheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }

        }

        #endregion

        public Data ExportTeacher(Guid teacherId)
        {
            var user = _dbContext.Users.Where(u => u.ID == teacherId).FirstOrDefault();

            if (user == null)
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_not_exist");
            }

            var list_answers = _dbContext.Answers.Where(a => a.UserID == teacherId).ToList();

            if (!list_answers.Any())
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("user_no_answers");
                return Data;
            }
            
            //the list of number of answers for each distinct questions, for current user
            List<StatisticAnswersHelpDTO> list = new List<StatisticAnswersHelpDTO>();

            var name = user.FirstName + "." + user.LastName;

            int nr = 0;
            foreach (var answer in list_answers)
            {
                StatisticAnswersHelpDTO sah = new StatisticAnswersHelpDTO();
                sah.SetType = answer.SetType.ToString();
                var answers = answer.StatisticAnswers.ToList();

                //count values from each answers
                sah.n1 = answers.Where(x => x.value == 1).Select(x => x.value).Count();
                sah.n2 = answers.Where(x => x.value == 2).Select(x => x.value).Count();
                sah.n3 = answers.Where(x => x.value == 3).Select(x => x.value).Count();
                sah.n4 = answers.Where(x => x.value == 4).Select(x => x.value).Count();
                sah.n5 = answers.Where(x => x.value == 5).Select(x => x.value).Count();

                int suma = sah.n1 + sah.n2 + sah.n3 + sah.n4 + sah.n5;
                if (suma >= 10)
                {
                    sah.P = (5 * sah.n5 + 4 * sah.n4 + 3 * sah.n3 + 2 * sah.n2 + sah.n1) / suma;
                }
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
                nr++;

                sah.Nr = nr;
                list.Add(sah);
            }

            if (ExportStatisticsById(list, name))
            {
                Data.Ok = true;
                Data.Message = _resourcesLoader.GetStringResourceByName("message_success");
            }
            else
            {
                Data.Ok = false;
                Data.Message = _resourcesLoader.GetStringResourceByName("install_failure");
            }

            return Data;
        }

        #endregion

        #region Column Index for Excel
        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }
        #endregion
    }
}
