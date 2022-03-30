using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
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
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly ISendConfirmationEmail _sendEmail;
            private readonly IMapper _mapper;

            public CreateUserHandler(UserManager<AppUser> userManager, ISendConfirmationEmail sendEmail,
                IMapper mapper)
            {
                _userManager = userManager;
                _sendEmail = sendEmail;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var user = new AppUser
                {
                    Email = request.Email,
                    FullName = request.FullName,
                    UserName = request.Email,
                    PhoneNumber = request.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded) throw new Exception("Fail to create account");

                await _sendEmail.SendConfirmationEmail(user);

                return _mapper.Map<AppUser, UserDto>(user);
            }
        }
    }
}