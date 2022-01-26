using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NetOpenIdDict.Data;
using NetOpenIdDict.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetOpenIdDict.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserModel registerUserModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = new ApplicationUser
            {
                UserName = registerUserModel.Email,
                Email = registerUserModel.Email,
                FirstName = registerUserModel.FirstName,
                LastName = registerUserModel.LastName,
                UserGuid = Guid.NewGuid()
            };

            var identityResult = await _userManager.CreateAsync(newUser, registerUserModel.Password);

            // If the Create didn't succeed, generate errors and return a Bad Result code with the Model.Errors
            if (!identityResult.Succeeded)
            {
                return GenerateErrorResult(identityResult);
                //return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError, "An error on the server occurred.");
            }
            //@($"{Context.Request.Scheme}://{Context.Request.Host}{Context.Request.Path}{Context.Request.QueryString}")
            string currentUri = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            var returnVal = new
            {
                Id = newUser.Id,
                UserGuid = newUser.UserGuid,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            };
            return Created(currentUri, returnVal);
            //return Accepted();

            /*
            string confirmEmailCode = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var callbackUrl = Url.Page("Areas/Identity/Account/ConfirmEmail", new { userId = User.Identities, confirmEmailCode });

            //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute",
            //    new { userId = user.Id, code }));

            await _userManager.se
               
            await AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

            return Created(locationHeader, TheModelFactory.Create(user));
            */


            // ------------------------------------------------------------
            // Begin copy of Register.cshtml.cs
            //_logger.LogInformation("User created a new account with password.");

            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            //var callbackUrl = Url.Page(
            //    "/Account/ConfirmEmail",
            //    pageHandler: null,
            //    values: new { area = "Identity", userId = newUser.Id, code = code, returnUrl = returnUrl },
            //    protocol: Request.Scheme);

            //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            //if (_userManager.Options.SignIn.RequireConfirmedAccount)
            //{
            //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
            //}
            //else
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return LocalRedirect(returnUrl);
            //}
            // End copy of Register.cshtml.cs
            // ------------------------------------------------------------


            // not enemy otherwise we wouldn't help you
            // know when you crash and burn
            // three quarters, only 6 weeks left

        }


        protected ActionResult GenerateErrorResult(IdentityResult result)
        {
            if(result is null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
            }

            if (!result.Succeeded)
            {
                if(result.Errors != null)
                {
                    foreach(var error in result.Errors)
                    {
                        var errorString = error.Code + ": " + error.Description;
                        ModelState.AddModelError("", errorString);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

       






    }
}
