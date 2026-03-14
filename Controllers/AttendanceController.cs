using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Kintai.Data;
using Kintai.Models;

[Authorize(Roles = "Employee")]
public class AttendanceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public AttendanceController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // 今日の打刻状況を表示
    public async Task<IActionResult> Today()
    {
        var userId = _userManager.GetUserId(User);
        var today = DateTime.Today;

        var attendance = _context.Attendance
            .FirstOrDefault(a => a.UserId == userId && a.WorkDate == today);

        return View(attendance);
    }

    // 出勤打刻
    public async Task<IActionResult> ClockIn()
    {
        var userId = _userManager.GetUserId(User);
        var today = DateTime.Today;

        var attendance = _context.Attendance
            .FirstOrDefault(a => a.UserId == userId && a.WorkDate == today);

        if (attendance == null)
        {
            attendance = new Attendance
            {
                UserId = userId,
                WorkDate = today,
                ClockIn = DateTime.Now
            };

            _context.Attendance.Add(attendance);
        }
        else
        {
            // 既に出勤済みなら何もしない
            if (attendance.ClockIn == null)
                attendance.ClockIn = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Today");
    }

    // 退勤打刻
    public async Task<IActionResult> ClockOut()
    {
        var userId = _userManager.GetUserId(User);
        var today = DateTime.Today;

        var attendance = _context.Attendance
            .FirstOrDefault(a => a.UserId == userId && a.WorkDate == today);

        if (attendance != null)
        {
            if (attendance.ClockOut == null)
            {
                attendance.ClockOut = DateTime.Now;

                // ★ 勤務時間を自動計算
                if (attendance.ClockIn != null)
                {
                    attendance.WorkingHours = attendance.ClockOut.Value - attendance.ClockIn.Value;
                }

                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction("Today");
    }

}
