using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Kintai.Data;
using Kintai.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// ★ 認証 → 認可 の順番
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

async Task CreateRolesAndAdminUserAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 作成したいロール
    string[] roles = { "Admin", "Employee" };

    // ロールが無ければ作成
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // 管理者ユーザーを作成（初回のみ）
    string adminEmail = "test@123.com";
    string adminPassword = "1qaz\"WSX";

    // 管理者ユーザーを取得
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    // ユーザーが存在しない場合は作成
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FullName = "管理者"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
    }

    // ★ ここが重要：存在していてもロール付与する
    if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

}

// ★ ここで初期化処理を実行
await CreateRolesAndAdminUserAsync(app);

app.Run();
