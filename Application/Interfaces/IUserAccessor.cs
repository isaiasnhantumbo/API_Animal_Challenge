namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUserEmail();
        string GetCurrentUserName();
        
    }
}