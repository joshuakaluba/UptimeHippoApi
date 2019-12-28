using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UptimeHippoApi.Common.Models;
using UptimeHippoApi.Data.DataAccessLayer.Authentication;
using UptimeHippoApi.Data.Models.Authentication;

namespace UptimeHippoApi.Web.Controllers
{
    public sealed class UserSettingsController : BaseController<UserSettingsController>
    {
        private readonly IUserRepository _userRepository;

        public UserSettingsController(IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            ILogger<UserSettingsController> logger)
            : base(userManager, logger)
        {
            _userRepository = userRepository;
        }

        //UserSettings/GetMySettings
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> GetMySettings()
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();
                var userViewModel = new UserSettingsViewModel(user);

                return Ok(userViewModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }

        //UserSettings/UpdateUserSettings
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Trusted")]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UserSettingsViewModel userSettingsViewModel)
        {
            try
            {
                var user = await GetCurrentAuthenticatedUser();
                user.UpdateFromUserViewModel(userSettingsViewModel);

                await _userRepository.UpdateUser(UserManager, user);

                return Ok(userSettingsViewModel);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                return BadRequest(new ErrorMessage(ex));
            }
        }
    }
}