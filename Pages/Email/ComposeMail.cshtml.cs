using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class ComposeMailModel : PageModel
{
    [BindProperty]
    public string Sender { get; set; }

    [BindProperty]
    public string Recipient { get; set; }

    [BindProperty]
    public string Subject { get; set; }

    [BindProperty]
    public string Body { get; set; }

    public void OnGet(string sender)
    {
        Sender = sender; // รับค่าจาก URL พารามิเตอร์
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            // ตรรกะการส่งอีเมล (ตัวอย่าง)
            // EmailService.SendEmail(Sender, Recipient, Subject, Body);

            // หลังส่งสำเร็จกลับไปหน้าแรก
            return RedirectToPage("/Index");
        }

        return Page(); // หากข้อมูลไม่ถูกต้องให้แสดงหน้าเดิม
    }
}