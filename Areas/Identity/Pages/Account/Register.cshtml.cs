using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BCrypt.Net; // ใช้สำหรับการเข้ารหัสรหัสผ่าน

namespace FinalProject.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, ErrorMessage = "ชื่อผู้ใช้ต้องมีความยาวระหว่าง 1 ถึง 50 ตัวอักษร", MinimumLength = 1)]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "อีเมลต้องมีความยาวไม่เกิน 100 ตัวอักษร")]
            [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
            public string Email { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "ชื่อจริงต้องมีความยาวไม่เกิน 50 ตัวอักษร")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "นามสกุลต้องมีความยาวไม่เกิน 50 ตัวอักษร")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "รหัสผ่านต้องมีความยาวอย่างน้อย {2} และไม่เกิน {1} ตัวอักษร", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/Index");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl ??= Url.Content("~/Index");

            if (ModelState.IsValid)
            {
                try
                {
                    string connectionString = "Server=tcp:celestialfinalproject.database.windows.net,1433;Initial Catalog=Finalproject;Persist Security Info=False;User ID=Celestial;Password=Easy12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        await connection.OpenAsync();

                        string sql = @"
                            INSERT INTO Users (Username, PasswordHash, FirstName, LastName, Email) 
                            VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Email)";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Username", Input.UserName);
                            command.Parameters.AddWithValue("@Email", Input.Email);
                            command.Parameters.AddWithValue("@FirstName", Input.FirstName);
                            command.Parameters.AddWithValue("@LastName", Input.LastName);

                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Input.Password);
                            command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                            await command.ExecuteNonQueryAsync();
                        }
                    }

                    _logger.LogInformation("ผู้ใช้สร้างบัญชีใหม่พร้อมรหัสผ่านสำเร็จแล้ว");

                    // เปลี่ยนเส้นทางไปยังหน้า Index
                    return RedirectToPage("/Index");
                }
                catch (SqlException sqlEx)
                {
                    _logger.LogError(sqlEx, "เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล");
                    ModelState.AddModelError(string.Empty, "เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล กรุณาลองใหม่อีกครั้ง");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "เกิดข้อผิดพลาดทั่วไป");
                    ModelState.AddModelError(string.Empty, "เกิดข้อผิดพลาดในการสร้างบัญชี กรุณาลองใหม่อีกครั้ง");
                }
            }

            // หาก ModelState ไม่ผ่านการตรวจสอบ ให้แสดงฟอร์มอีกครั้ง
            return Page();
        }
    }
}
