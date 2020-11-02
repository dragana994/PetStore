using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetStore.Entities;
using PetStore.Models;
using PetStore.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.ApiController
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<UserEntity> userManager;
		private readonly JwtSettings _jwtSettings;

		public AuthController(
			UserManager<UserEntity> userManager,
			 IOptionsSnapshot<JwtSettings> jwtSettings)
		{
			this.userManager = userManager;
			_jwtSettings = jwtSettings.Value;
		}

		[HttpPost("SignUp")]
		public async Task<IActionResult> SignUp([FromBody] UserModel model)
		{
			var newUser = model.Adapt<UserEntity>();

			var result = await userManager.CreateAsync(newUser, model.Password);

			if (result.Succeeded)
			{
				if (!string.IsNullOrEmpty(model.RoleName))
					result = await userManager.AddToRoleAsync(newUser, model.RoleName);

				if (result.Succeeded)
					return Ok();

				return Problem(result.Errors.First().Description, null, StatusCodes.Status500InternalServerError);
			}
			else
				return Problem(result.Errors.First().Description, null, StatusCodes.Status500InternalServerError);
		}

		[HttpPost("SignIn")]
		public async Task<IActionResult> SignIn(UserLoginModel model)
		{
			var user = userManager
				.Users
				.SingleOrDefault(u => u.UserName == model.UserName);

			if (user is null)
				return NotFound("User not found");

			var result = await userManager.CheckPasswordAsync(user, model.Password);

			if (result)
			{
				var roles = await userManager.GetRolesAsync(user);
				return Ok(GenerateJwt(user, roles));
			}

			return BadRequest("Username or password incorrect.");
		}

		#region Private methods

		private string GenerateJwt(UserEntity user, IList<string> roles)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
			};

			var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
			claims.AddRange(roleClaims);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

			var token = new JwtSecurityToken(
					issuer: _jwtSettings.Issuer,
					audience: _jwtSettings.Issuer,
					claims,
					expires: expires,
					signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		#endregion
	}
}