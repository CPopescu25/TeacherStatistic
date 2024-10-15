using System;
using TS.DTO.Enums;

namespace Client.Models
{
    public interface IEmailTemplate
    {
        string SubjectUserCreate();
        string SubjectForgotPassword();
        string SubjectSendEmail();
        string BodyForgotPassword(string username, string password, string host);
        string BodySendEmail(string username, string teacher, string host, Guid id, SetType SetType);
        string BodyUserCreate(string name, string username, string password, string host);
    }
}
