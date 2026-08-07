using QuestLog.Data;

namespace QuestLog.Components.Account.Pages;

internal static class EmailTemplates
{
    public static string PasswordReset(ApplicationUser user, string email, string resetLink) =>
        Layout(
            title: "Reset your password",
            bodyHtml: $@"
        <h1 class='heading-font' style='font-size: 22px; margin: 0 0 20px; color: #ffffff;'>Reset your password</h1>
        <p>Hi {user.UserName},</p>
        <p>We received a request to reset the password for your account associated with <strong>{email}</strong>.</p>
        <p>Click the button below to choose a new password. <strong>This link will expire shortly.</strong></p>

        <div style='margin: 30px 0;'>
            <a href='{resetLink}' style='background-color: #10b981; color: #0b0b0c; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block; font-weight: bold;'>Reset Password</a>
        </div>

        <p style='color: #a1a1aa; font-size: 14px;'>If the button above does not work, copy and paste this URL into your browser:</p>
        <p style='background-color: #0b0b0c; color: #a1a1aa; padding: 10px; border-radius: 4px; word-break: break-all; font-family: monospace; border: 1px solid #2d2d30;'>{resetLink}</p>

        <hr style='border: 0; border-top: 1px solid #2d2d30; margin: 30px 0;' />
        <p style='color: #a1a1aa; font-size: 12px;'>If you did not request this reset, please ignore this email. Your account remains completely secure.</p>");

    public static string ConfirmEmail(ApplicationUser user, string email, string confirmationLink) =>
        Layout(
            title: "Confirm your email",
            bodyHtml: $@"
        <h1 class='heading-font' style='font-size: 22px; margin: 0 0 20px; color: #ffffff;'>Confirm your email</h1>
        <p>Hi {user.UserName},</p>
        <p>Thanks for signing up! Please confirm that <strong>{email}</strong> is your email address to finish setting up your account.</p>

        <div style='margin: 30px 0;'>
            <a href='{confirmationLink}' style='background-color: #10b981; color: #0b0b0c; padding: 12px 24px; text-decoration: none; border-radius: 4px; display: inline-block; font-weight: bold;'>Confirm Email</a>
        </div>

        <p style='color: #a1a1aa; font-size: 14px;'>If the button above does not work, copy and paste this URL into your browser:</p>
        <p style='background-color: #0b0b0c; color: #a1a1aa; padding: 10px; border-radius: 4px; word-break: break-all; font-family: monospace; border: 1px solid #2d2d30;'>{confirmationLink}</p>

        <hr style='border: 0; border-top: 1px solid #2d2d30; margin: 30px 0;' />
        <p style='color: #a1a1aa; font-size: 12px;'>If you did not create an account, please ignore this email.</p>");

    private static string Layout(string title, string bodyHtml) => $@"<!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>{title}</title>

  <!-- Google Fonts Imports -->
  <link rel=""preconnect"" href=""https://fonts.googleapis.com"">
  <link rel=""preconnect"" href=""https://fonts.gstatic.com"" crossorigin>
  <link href=""https://fonts.googleapis.com/css2?family=Bayon&family=Inter:wght@400;600&display=swap"" rel=""stylesheet"">

  <style type=""text/css"">
    body {{
      margin: 0;
      padding: 0;
      background-color: #0b0b0c;
      font-family: 'Inter', Arial, sans-serif;
      -webkit-font-smoothing: antialiased;
    }}

    .heading-font {{
      font-family: 'Bayon', 'Arial Black', Impact, sans-serif;
      font-weight: 400;
      letter-spacing: 0.5px;
    }}

    .body-font {{
      font-family: 'Inter', Arial, sans-serif;
    }}
  </style>

  <!--[if mso]>
  <style type=""text/css"">
    h1, h2, h3 {{
      font-family: 'Arial Black', Impact, sans-serif !important;
    }}
    body, table, td, p, a, span {{
      font-family: Arial, sans-serif !important;
    }}
  </style>
  <![endif]-->
</head>
<body class=""body-font"">
    <div style='max-width: 600px; margin: 0 auto; padding: 32px 24px; background-color: #1b1b1d; color: #ffffff; border-radius: 8px; border: 1px solid #2d2d30;'>
        <p class='heading-font' style='margin: 0 0 24px; font-size: 1.625rem; letter-spacing: 0.5px; text-transform: uppercase; color: #ffffff;'>Quest<span style='color: #10b981;'>log</span></p>
        {bodyHtml}
    </div>
</body>
</html>";
}
