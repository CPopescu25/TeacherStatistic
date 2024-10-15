using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.Data.Context.Models;

namespace TS.Data.Context
{
    public class TSClientContext : DbContext
    {
        #region Constructor
        public TSClientContext()
            : base("name=TSClientContext")
        {
            //AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
        }
        #endregion

        public DbSet<Answers> Answers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<StatisticAnswers> StatisticAnswers { get; set; }
        public DbSet<StatisticQuestions> StatisticQuestions { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<StudentsTargets> StudentsTargets { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserUniversity> UserUniversities { get; set; }
    }
}
