using Kintai.Data;
using Kintai.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kintai.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            await _userManager.DeleteAsync(user);

            return RedirectToAction("Users");
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string fullName, string email, string password)
        {
            var user = new ApplicationUser
            {
                FullName = fullName,
                Email = email,
                UserName = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");
                return RedirectToAction("Users");
            }

            ModelState.AddModelError("", "作成に失敗しました");
            return View();
        }

        // 社員の勤怠一覧
        public IActionResult AttendanceList(string nameKeyword, DateTime? date)
        {
            var query = _context.Attendance
                .Include(a => a.User)
                .AsQueryable();

            // ① 社員名（FullName）の部分一致
            if (!string.IsNullOrEmpty(nameKeyword))
            {
                query = query.Where(a => a.User.FullName.Contains(nameKeyword));
            }

            // ② 日付指定（カレンダー入力）
            if (date.HasValue)
            {
                query = query.Where(a => a.WorkDate == date.Value.Date);
            }

            var attendances = query
                .OrderByDescending(a => a.WorkDate)
                .ToList();

            return View(attendances);
        }
    }
}
