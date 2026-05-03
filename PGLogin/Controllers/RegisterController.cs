using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;
using Login = PGLogin.Models.Login;
using Register = PGLogin.Models.Register;

namespace PGLogin.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MydBContext _mydBContext;
        APIResponse _apiResponse = new APIResponse();

        public RegisterController(IRegisterRepository registerRepository, IHttpContextAccessor httpContextAccessor, MydBContext mydBContext)
        {
            _registerRepository = registerRepository;
            _httpContextAccessor = httpContextAccessor;
            _mydBContext = mydBContext;
        }

        public async Task<IActionResult> GetsRegisters(int page = 1)
        {
            int pageSize = 20;
            var registers = await _mydBContext.registers.OrderBy(r => r.Email).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(_mydBContext.registers.Count() / (double)pageSize);
            return View(registers);
        }
        public async Task<IActionResult> GetRegisters()
        {
            var result = await _registerRepository.GetAll();
            return View("GetsRegisters", result);
        }

        public IActionResult CreateRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Register registerDTOs)
        {
            string subject = "Welcome";
            string body = "Welcome, " + registerDTOs.First_Name + " " + registerDTOs.Last_Name + " You Have Registered Successfully...";

            if (_mydBContext.registers.All(x => x.Email != registerDTOs.Email || x.IsDeleted == true))
            {
                registerDTOs.LastPassword1 = registerDTOs.Password;
                registerDTOs.LastPassword2 = registerDTOs.LastPassword1;
                registerDTOs.Password = Encryption.Encrypt(registerDTOs.Password);
                registerDTOs.Confirm_Password = Encryption.Encrypt(registerDTOs.Confirm_Password);

                if (registerDTOs.Password != registerDTOs.Confirm_Password)
                {
                    TempData["SuccessMessage"] = "The Password & Confirm Password Must Be Same";
                    return RedirectToAction("CreateRegister", "Register");
                }
                var result = new RegisterDTO
                {
                    First_Name = registerDTOs.First_Name,
                    Last_Name = registerDTOs.Last_Name,
                    Email = registerDTOs.Email,
                    Password = registerDTOs.Password,
                    Confirm_Password = registerDTOs.Confirm_Password,
                    EventDateTime = registerDTOs.EventDateTime,
                    IsDeleted = registerDTOs.IsDeleted
                };
                await _registerRepository.Registers(result);
                EmailSender sender = new EmailSender();
                sender.SendEmail(registerDTOs.Email, subject, body);
                return RedirectToAction("Logins", "Register");
            }
            /*            TempData["SuccessMessage2"] = "You Can't Register With Same Email ID As Of Our Record";
            */
            return RedirectToAction("CreateRegister", "Register");
        }


        [HttpGet]
        public async Task<IActionResult> EditRoom(int id)
        {
            var register = await _registerRepository.Get(id);

            if (register == null)
            {
                TempData["ErrorMessage"] = "Room not found!";
                return RedirectToAction("Index", "Home");
            }
            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Register register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            var response = await _registerRepository.Update(register);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Register edited successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit Register!";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteR(int id)
        {
            await _registerRepository.Delete(id);
            return RedirectToAction("GetsRegisters", "Register");
        }

        public IActionResult Logins()
        {
            var email = _httpContextAccessor.HttpContext.Session.GetString("Email");
            ViewData["Email"] = email;
            return View(email);
        }
        [HttpPost]
        public async Task<IActionResult> LoginUser(RegisterDTO logiRequest)

        {
            logiRequest.Email = logiRequest.Email;
            logiRequest.Password = logiRequest.Password;
            var result1 = await _registerRepository.Token_Call<Register>(logiRequest);
            var result = await _registerRepository.Login<Register>(logiRequest);
            //string r = null;589*-
            var rr = _mydBContext.registers.FirstOrDefault(x => x.Email == logiRequest.Email);
            rr.Password = Encryption.Decrypt(rr.Password);
            //loginRequest.Password = Encryption.Encrypt(loginRequest.Password);
            if (_mydBContext.registers.Where(x => x.Email == logiRequest.Email && rr.Password == logiRequest.Password).IsNullOrEmpty())
            {
                ViewData["LogOut"] = true;
                TempData["SuccessMessage"] = "Please Enter Valid Credentials";
                return RedirectToAction("Logins", "Register");
            }
            else
            {
                // Set the email in the session
                HttpContext.Session.SetString("Email", logiRequest.Email);
                ViewData["LogOut"] = false;
                TempData["SuccessMessage"] = "You Have Logged In successfully!...";
                return RedirectToAction("Search", "PG");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Logins", "Register");
        }

        public async Task<IActionResult> ForgotPass()
        {
            List<string> Emails = new List<string>();

            // Get data directly
            var result1 = await _registerRepository.GetAll();

            // If GetAll() already returns List<Candidate>, no deserialization needed
            ViewData["customers"] = result1;

            foreach (var candidate in result1)
            {
                Emails.Add(candidate.Email);
            }

            ViewBag.Emails = Emails;
            return View();
        }


        public async Task<IActionResult> ForgotPasswordEmailCheck(string email)
        {

            var result = _mydBContext.registers.FirstOrDefault(x => x.Email == email);
            if (result == null)
            {
                return RedirectToAction("ForgotPass", "Register");
            }
            var resetPassUrl = Url.Action("ResetPass", "Register", new { email = result.Email });

            // Return the URL as a JSON response
            return Json(new { redirectTo = resetPassUrl });
        }

        public async Task<IActionResult> ResetPass(string email)
        {
            var result = _mydBContext.registers.FirstOrDefault(x => x.Email == email);
            ViewData["email"] = result.Email;
            return View(new RegisterDTO { Email = email });
        }

        public async Task<IActionResult> ResetPassword(string email, string newPassword, string confirmPassword)
        {
            Register reg = new Register();
            var result = _mydBContext.registers.FirstOrDefault(x => x.Email == email);
            string vv = result.Password;
            String pass1 = Encryption.Decrypt(result.LastPassword1);
            String pass2 = Encryption.Decrypt(result.LastPassword2);
            if (newPassword == confirmPassword && newPassword != pass1
                && newPassword != pass2)
            {
                String subject = "Congratulations!";
                String body = "Congratulations, Your Password Is Updated Successfully!...";
                reg.LastPassword1 = Encryption.Encrypt(newPassword);
                reg.LastPassword2 = result.Password;
                reg.Password = Encryption.Encrypt(newPassword);
                reg.Confirm_Password = Encryption.Encrypt(newPassword);
                reg.Email = email;
                reg.Last_Name = result.Last_Name;
                reg.First_Name = result.First_Name;
                await _registerRepository.Update(reg);
                EmailSender emailSender = new EmailSender();
                emailSender.SendEmail(reg.Email, subject, body);
            }
            else if (newPassword == result.LastPassword1 || newPassword == result.LastPassword2
                || vv == newPassword || vv == confirmPassword)
            {
                TempData["SuccessMessage"] = "Sorry, Your Password Should Not Be Matched With Your Last Two Passwords.";
                return RedirectToAction("ForgotPass", "Register");
            }
            TempData["SuccessMessage"] = "Your Password Is Updated successfully!...";
            return RedirectToAction("Logins", "Register");
        }
    }
}
