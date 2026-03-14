📘 README.md
# 勤怠打刻システム（ASP.NET Core MVC）

ASP.NET Core MVC と Entity Framework Core を使用して構築した勤怠打刻システムです。  
社員は出勤・退勤の打刻ができ、管理者は社員の勤怠一覧を確認できます。

---

## 🚀 主な機能

### 社員向け
- ログイン／ログアウト（ASP.NET Core Identity）
- 出勤打刻（ClockIn）
- 退勤打刻（ClockOut）
- 勤務時間の自動計算（ClockOut 時に WorkingHours を算出）
- 今日の勤怠状況の確認（/Attendance/Today）

### 管理者向け
- 社員一覧の表示・編集・削除
- 社員の勤怠一覧表示
- 社員名の部分一致検索
- 日付指定（カレンダー入力）による勤怠絞り込み
- 勤務時間の確認

---

## 🏗 使用技術

- **ASP.NET Core MVC 8**
- **Entity Framework Core**
- **ASP.NET Core Identity**
- **SQL Server LocalDB / SQLite**
- **Bootstrap 5**
- **C# 12**

---

## 📂 プロジェクト構成


Kintai/  
├── Controllers/  
│   ├── HomeController.cs  
│   ├── AttendanceController.cs  
│   └── AdminController.cs  
│   
├── Models/  
│   ├── ApplicationUser.cs  
│   └── Attendance.cs  
│  
├── Data/  
│   └── ApplicationDbContext.cs  
│  
├── Views/  
│   ├── Home/  
│   │   └── Index.cshtml  
│   │   └── Privacy.cshtml  
│   │  
│   ├── Attendance/  
│   │   ├── Today.cshtml  
│   │   └── Index.cshtml  
│   │  
│   ├── Admin/  
│   │   ├── Users.cshtml  
│   │   ├── EditUser.cshtml  
│   │   ├── DeleteUser.cshtml  
│   │   ├── AttendanceList.cshtml  
│   │   └── CreateUser.cshtml  
│   │  
│   └── Shared/  
│       ├── _Layout.cshtml  
│       │   └──  _Layout.cshtml.css  
│       ├── _LoginPartial.cshtml  
│       ├── Error.cshtml  
│       └── _ValidationScriptsPartial.cshtml  
│  
├── appsettings.json  
├── appsettings.Development.json  
├── Program.cs  
├── Kintai.csproj  
└── README.md  


---

## 🧮 勤務時間の計算ロジック

退勤時に以下の式で勤務時間を自動計算します。


WorkingHours = ClockOut - ClockIn

C# の `TimeSpan` を使用して保存します。

---

## 🔐 ロール管理

- **Admin**  
  社員管理・勤怠一覧の閲覧が可能  
  ```
  初期アカウント 
   mail：`test@123.com`
   Pass：`1qaz"WSX`
  ```

- **User（社員）**  
  出勤／退勤の打刻が可能

---

## 🛠 セットアップ手順

### 1. パッケージ復元


dotnet restore

### 2. DB マイグレーション


dotnet ef database update

### 3. 開発サーバー起動


dotnet run

---

## 📅 画面一覧

### 社員
- `/Attendance/Today`  
  今日の打刻状況を表示

### 管理者
- `/Admin/Users`  
  社員一覧
- `/Admin/AttendanceList`  
  勤怠一覧（社員名部分一致・日付絞り込み）

---

## 🔧 環境変数・設定

`appsettings.Development.json` に接続文字列を設定します。

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=kintai.db"
}
```


📄 ライセンス
MIT License

---



