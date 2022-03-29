using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth
{
    public class Login
    {
        public class LoginCommand : IRequest<LoginDto>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginValidator : AbstractValidator<LoginCommand>
        {
            public LoginValidator()
            {
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class LoginHandler : IRequestHandler<LoginCommand, LoginDto>
        {
            private readonly SignInManager<AppUser> _signInManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public LoginHandler(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
                IJwtGenerator jwtGenerator)

            {
                _signInManager = signInManager;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.Users.Where(x => x.Email == request.Email).FirstOrDefaultAsync();

                if (user is null) throw new Exception("Email or password fail");

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!result.Succeeded) throw new Exception("Email or password fail");
                return new LoginDto
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }
        }
    }
}