using Microsoft.AspNetCore.Mvc;
using PRN231.API.Enums;
using PRN231.API.Payload.Request.Auth;
using PRN231.API.Payload.Response;
using PRN231.API.Utils;
using PRN231.Repo.Interfaces;

namespace PRN231.API.Controllers;

[ApiController]
[Route("auths")]
public class AuthenticationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        var loginAccount = _unitOfWork.AccountRepository
            .Get(account => account.Username.Equals(loginRequest.Username)
                            && account.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password)),
                includeProperties: "Role").FirstOrDefault();
        if (loginAccount == null)
            throw new KeyNotFoundException("Invalid username or password");

        var token = JwtUtil.GenerateJwtToken(loginAccount);
        var loginResponse = new BasicResponse
        {
            IsSuccess = true,
            Message = "",
            StatusCode = 200,
            Data = new
            {
                accessToken = token,
                user = loginAccount
            }
        };
        return Ok(loginResponse);
    }
}