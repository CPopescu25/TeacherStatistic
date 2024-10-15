
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TS.DTO.Enums;

namespace Client.Models
{
    public class EmailTemplate : IEmailTemplate
    {
        public string SubjectUserCreate()
        {
            return "You have an account created!";
        }

        public string SubjectForgotPassword()
        {
            return "Password Recovery!";
        }

        public string SubjectSendEmail()
        {
            return "Statistic Questionnaire!";
        }

        public string BodyUserCreate(string name,string username,string password,string host)
        {
            return "Hello, " + name
                    + "!<br/>A new account was created for you in <strong>Statistic Application</strong>.<br/>For log in, use this unique <strong>Username:</strong>&nbsp;"
                    + username + "&nbsp; and this <strong>password</strong>:&nbsp; " + password + ".<br/> "
                    + "To access your account click here:<a href=\" " + "http://localhost:"
                     + host + "/Home/Index" + "\">" + "&nbsp;" + "Account </a>" + ".<br/> ";
        }

        public string BodyForgotPassword(string username,string password,string host)
        {
            return "Hello, " + username + "!<br/>Your Password is:<strong>\" "
                    + password + "</strong>.\"<br/> If you want to access your account click here:<a href=\" "
                    + "http://localhost:" + host + "/Home/Index" + "\">" + "&nbsp;" + "Account" + "</a>" + ".\"";
        }

        public string BodySendEmail(string username,string teacher,string host,Guid id, SetType SetType)
        {
            return "Hello," + username + "!<br/>I will invite you to answer some questions!<br/>Tell us your opinion about this teacher &nbsp;"
                   + teacher + ".<br/>To see the questions and answer them click here:<a href=\" " + "http://localhost:" + host
                   + "/Answer/Answer?id=" + id + "&SetType=" + SetType + "\">" + "&nbsp;" + "Questionnaire" + "</a>" + ".<br/>. ";
        }
    }
}