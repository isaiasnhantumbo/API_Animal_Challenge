using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Users
{
    public class VerifyUserEmail
    {
        public class VerifyUserEmailQuery : IRequest<UserDto>
        {
            public string Key { get; set; }
        }

        public class VerifyUserEmailHandler : IRequestHandler<VerifyUserEmailQuery, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IMapper _mapper;

            public VerifyUserEmailHandler(UserManager<AppUser> userManager, IMapper mapper)
            {
                _userManager = userManager;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(VerifyUserEmailQuery request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.Where(x => x.Id == request.Key).FirstOrDefaultAsync();
                if (user is null) throw new WebException("User not found");
                user.EmailConfirmed = true;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) throw new Exception("Fail to verify user email");
                return _mapper.Map<AppUser, UserDto>(user);
            }
        }
    }
}