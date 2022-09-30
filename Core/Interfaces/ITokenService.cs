using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        // We're going to use information from the AppUser object to populate the fields inside the token.
        string CreateToken(AppUser user);
    }
}
