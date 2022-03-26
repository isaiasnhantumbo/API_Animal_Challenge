using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users
{
    public class GetUser
    {
        public class GetUserQuery: IRequest<UserDto>
        {
        }
        
        public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;

            public GetUserHandler(UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
            }
            
            public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.Where(x => x.Email == _userAccessor.GetCurrentUserEmail())
                    .FirstOrDefaultAsync();

                return new UserDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }
        }
    }
}