using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Users
{
    public class CreateUser
    {
        public class CreateUserCommand : IRequest<UserDto>
        {
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }
        
        public class CreateUserValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserValidator()
            {
                RuleFor(x => x.FullName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotEmpty();
            }
        }
        
        public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;

            public CreateUserHandler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }
            
            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new AppUser
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    throw new Exception("Fail to create account");
                }
                
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