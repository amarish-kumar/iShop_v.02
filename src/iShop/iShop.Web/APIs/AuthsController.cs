﻿//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;

//namespace iShop.Infras.API.APIs
//{
//    /// <summary>
//    /// This Controller is reponsible for creating and validating JWT
//    /// </summary>
//    public class AuthsController : Microsoft.AspNetCore.Mvc.Controller
//    {
//        private readonly IOptions<IdentityOptions> _identityOptions;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public AuthsController(IOptions<IdentityOptions> identityOptions, SignInManager<ApplicationUser> signInManager,
//            UserManager<ApplicationUser> userManager, JwtTokenSettings tokenSettings)
//        {
//            _identityOptions = identityOptions;
//            _signInManager = signInManager;
//            _userManager = userManager;
//        }
//        /// <summary>
//        /// Method for creating a new Token
//        /// </summary>
//        /// <param name="request"></param>
//        /// <param name="user"></param>
//        /// <param name="properties"></param>
//        /// <returns>The new token or token that has been refreshed base on the input grant_type</returns>
      

//        /// <summary>
//        /// Method for creating new Token for authenicated User or refresh the old one
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        [AllowAnonymous]
//        [HttpPost("~/connect/token")]
//        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
//        {
//            // The grant_type is password, creating new token 
//            if (request.IsPasswordGrantType())
//            {
//                // Check if the username is in the database or not
//                var user = await _userManager.FindByNameAsync(request.Username);

//                // Username is not existed
//                if (user == null)
//                {
//                    return BadRequest(new OpenIdConnectResponse
//                    {
//                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
//                        ErrorDescription = "The username/password couple is invalid."
//                    });
//                }

//                // Validate the username/password parameters and ensure the account is not locked out.
//                // Check the password associated with the username
//                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);

//                // Username and password are match
//                if (!result.Succeeded)
//                {
//                    return BadRequest(new OpenIdConnectResponse
//                    {
//                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
//                        ErrorDescription = "The username/password couple is invalid."
//                    });
//                }

//                // Create a new authentication ticket.
//                var ticket = await CreateTicketAsync(request, user);

//                // Log the user in
//                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
//            }

//            // Refresh an old Token, more security measures may be added later
//            else if (request.IsRefreshTokenGrantType())
//            {
//                // Retrieve the claims principal stored in the refresh token.
//                var info = await HttpContext.AuthenticateAsync(OpenIdConnectServerDefaults.AuthenticationScheme);

//                // Retrieve the user profile corresponding to the refresh token.
//                // Note: if you want to automatically invalidate the refresh token
//                // when the user password/roles change, use the following line instead:
//                var user = await _signInManager.ValidateSecurityStampAsync(info.Principal);
//                //var user = await _userManager.GetUserAsync(info.Principal);
//                if (user == null)
//                {
//                    return BadRequest(new OpenIdConnectResponse
//                    {
//                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
//                        ErrorDescription = "The refresh token is no longer valid."
//                    });
//                }

//                // Ensure the user is still allowed to sign in.
//                if (!await _signInManager.CanSignInAsync(user))
//                {
//                    return BadRequest(new OpenIdConnectResponse
//                    {
//                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
//                        ErrorDescription = "The user is no longer allowed to sign in."
//                    });
//                }

//                // Create a new authentication ticket, but reuse the properties stored
//                // in the refresh token, including the scopes originally granted.
//                var ticket = await CreateTicketAsync(request, user, info.Properties);

//                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
//            }

//            return BadRequest(new OpenIdConnectResponse
//            {
//                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
//                ErrorDescription = "The specified grant type is not supported."
//            });
//        }

//        /// <summary>
//        /// Log the user out
//        /// </summary>
//        /// <returns></returns>
//        [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
//        public async Task<IActionResult> Logout()
//        {
//            // Ask ASP.NET Core Identity to delete the local and external cookies created
//            // when the user agent is redirected from the external identity provider
//            // after a successful authentication flow (e.g Google or Facebook).
//            await _signInManager.SignOutAsync();

//            // Returning a SignOutResult will ask OpenIddict to redirect the user agent
//            // to the post_logout_redirect_uri specified by the client application.
//            return SignOut(OpenIdConnectServerDefaults.AuthenticationScheme);
//        }

//    }
//}
