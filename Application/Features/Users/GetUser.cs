using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users
{
    public class GetUser
    {
        public class GetUserQuery : IRequest<UserDto>
        {

        }

        public class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public GetUserHandler(UserManager<AppUser> userManager,IUserAccessor userAccessor, IMapper mapper)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }
            public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.Where(x => x.Email == _userAccessor.GetCurrentUserEmail())
                    .FirstOrDefaultAsync();
                return _mapper.Map<AppUser, UserDto>(user);

            }
        }
    }
}