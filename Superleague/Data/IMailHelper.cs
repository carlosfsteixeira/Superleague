using Superleague.Helpers;

namespace Superleague.Data
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
