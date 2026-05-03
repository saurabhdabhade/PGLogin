using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly MydBContext _mydBContext;
        public UserController(IUserRepository userRepository, MydBContext mydBContext)
        {
            _userRepository = userRepository;
            _mydBContext = mydBContext;
        }
        public IActionResult GetUsers()
        {
            var users = _mydBContext.users.ToList();
            return View(users);
        }
        public async Task<IActionResult> GetsUsers()
        {
            var result = await _userRepository.GetAllUsers();
            return View("GetUsers", result);
        }
        public IActionResult Details()
        {
            var users = _mydBContext.users.ToList();
            return View(users);
        }
        public async Task<IActionResult> GetUser_ById(Guid id)
        {
            var result = await _userRepository.GetUserById(id);
            return View("GetUsers", result);
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Create_User(User user)
        {
            var result = new UserDTO()
            {
                User_Name = user.User_Name,
                User_Email = user.User_Email,
                Role_Id = user.Role_Id,
            };
            await _userRepository.AddUser(result);
            return RedirectToAction("GetUsers", "User");
        }
        public IActionResult Edit_User(Guid id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found!";
                return RedirectToAction("GetUsers", "User");
            }
            return View(user);
        }
        public async Task<IActionResult> User_Edit(User user)
        {
            var result = _userRepository.UpdateUser(user);
            if (result != null)
            {
                TempData["SuccessMessage"] = "User edited successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit User!";
            }
            return RedirectToAction("GetUsers", "User");
        }

        public async Task<IActionResult> Delete_User(Guid id)
        {
            var result = await _userRepository.DeleteUser(id);
            return RedirectToAction("GetUsers", "User");
        }
    }
}