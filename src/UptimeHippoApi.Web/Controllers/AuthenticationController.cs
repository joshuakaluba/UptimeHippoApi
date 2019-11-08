using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using UptimeHippoApi.Common.Models;
using UptimeHippoApi.Data.DataAccessLayer.Authentication;
using UptimeHippoApi.Data.DataAccessLayer.PushNotificationTokens;
using UptimeHippoApi.Data.Models.Authentication;
using UptimeHippoApi.Data.Models.Notification;
using UptimeHippoApi.Data.Models.Static;
using UptimeHippoApi.Data.Services.Authentication;
using TokenOptions = UptimeHippoApi.Data.Models.Authentication.TokenOptions;

namespace UptimeHippoApi.Web.Controllers
{
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenOptions _tokenOptions;
        private readonly IUserRepository _userRepository;
        private readonly IPushNotificationTokensRepository _pushNotificationTokensRepository;
        private readonly IUserValidatorService _userValidatorService;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserRepository userRepository,
            IPushNotificationTokensRepository pushNotificationTokensRepository,
            IUserValidatorService userValidatorService,
            ITokenGeneratorService  tokenGeneratorService,
            IOptions<TokenOptions> tokens,
            ILogger<AuthenticationController> logger)
            : base(userManager, logger)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenOptions = tokens.Value;
            _pushNotificationTokensRepository = pushNotificationTokensRepository;
            _userValidatorService = userValidatorService;
            _tokenGeneratorService = tokenGeneratorService;
        }

        //Authentication/Login
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.IncompleteDataReceived));
                }

                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToLogIn));
                }

                var isUserValid = _userValidatorService.ValidateUser(user);

                if (!isUserValid)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.AccountDeactivated));
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (!result.Succeeded)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToLogIn));
                }

                var token = await _tokenGeneratorService.CreateJwtToken(user, UserManager, _tokenOptions);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/Register
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.IncompleteDataReceived));
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _userRepository.RegisterNewUser(UserManager, user, model.Password);

                if (result.Succeeded == false)
                {
                    return BadRequest(new ErrorMessage(ErrorMessageResponses.UnableToRegister));
                }

                var token = await _tokenGeneratorService.CreateJwtToken(user, UserManager, _tokenOptions);
                return Ok(token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/RegisterPushNotifications
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> RegisterPushNotifications([FromBody] PushNotificationModel model)
        {
            try
            {
                var user = await GetUser();

                var pushNotificationToken = new PushNotificationToken
                {
                    Token = model.Token,
                    ApplicationUserId = user.Id
                };

                await _pushNotificationTokensRepository.SavePushNotificationToken(pushNotificationToken);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //Authentication/UnRegisterPushNotifications
        [HttpPost]
        public async Task<IActionResult> UnRegisterPushNotifications([FromBody] PushNotificationModel model)
        {
            try
            {
                var pushNotificationToken = new PushNotificationToken
                {
                    Token = model.Token
                };

                await _pushNotificationTokensRepository.RemovePushNotificationToken(pushNotificationToken);
                return Ok(model);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }
    }
}