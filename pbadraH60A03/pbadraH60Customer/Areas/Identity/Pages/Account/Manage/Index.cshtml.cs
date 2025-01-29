// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using pbadraH60A01.Models;
using pbadraH60Customer.DAL;
using pbadraH60Customer.Models;

namespace pbadraH60Customer.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICustomerRepository _customerRepository;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICustomerRepository customerRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerRepository = customerRepo;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            public string? UserId { get; set; }
            public int? CustomerId { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [RegularExpression(@"^[a-zA-Z' -]+$",
                ErrorMessage = "The first name must contain only letters, apostrophes, dashes, and spaces.")]
            public string? FirstName { get; set; }

            [RegularExpression(@"^[a-zA-Z' -]+$",
                ErrorMessage = "The last name must contain only letters, apostrophes, dashes, and spaces.")]
            public string? LastName { get; set; }

            [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
            [Required(ErrorMessage = "The email is required.")]
            [UniqueEmail]
            public string? Email { get; set; }

            [ValidProvince(ErrorMessage = "The province must either be Quebec, Ontario, New Brunswick, or Manitoba.")]
            public string? Province { get; set; }

            [MinLength(16, ErrorMessage = "The credit card number must be exactly 16 digits long.")]
            [MaxLength(16, ErrorMessage = "The credit card number must not exceed 16 digits.")]
            [RegularExpression(@"^\d+$",
                ErrorMessage = "The credit card number must be composed of only positive integer numbers.")]
            public string? CreditCard { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user);

            if (await _userManager.IsInRoleAsync(user, "Customer"))
            {
                var customer = await _customerRepository.FindByUserId(user.Id);

                if (customer != null)
                {
                    Input = new InputModel
                    {
                        CustomerId = customer.CustomerId,
                        UserId = customer.UserId,
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        PhoneNumber = phoneNumber,
                        Email = customer.Email,
                        Province = customer.Province,
                        CreditCard = customer.CreditCard
                    };
                }
            }
            else
            {
                Input = new InputModel
                {
                    UserId = user.Id,
                    PhoneNumber = phoneNumber,
                    Email = email
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (User.IsInRole("Customer"))
            {
                var customer = await _customerRepository.FindByUserId(user.Id);
                customer.FirstName = Input.FirstName;
                customer.LastName = Input.LastName;
                customer.Email = Input.Email;
                customer.PhoneNumber = Input.PhoneNumber;
                customer.Province = Input.Province;
                customer.CreditCard = Input.CreditCard;

                await _customerRepository.Update(customer);
            }
            else
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                user.EmailConfirmed = true;
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Email);
                if (!setPhoneResult.Succeeded || !setEmailResult.Succeeded || !setUsernameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                
                if (!user.EmailConfirmed)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

                    if (!confirmResult.Succeeded)
                    {
                        StatusMessage = "Unexpected error when trying to confirm email.";
                        return RedirectToPage();
                    }
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}