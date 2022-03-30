using System.Threading.Tasks;
using Domain;

namespace Application.Interfaces
{
    public interface ISendConfirmationEmail
    {
        Task SendConfirmationEmail(AppUser user);
    }
}