using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using QuestLog.Data;

namespace QuestLog.Components.Account;

internal sealed class SmtpOptions
{
    public string Host { get; set; } = "";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromEmail { get; set; } = "";
    public string FromName { get; set; } = "QuestLog";
}

internal sealed class SmtpEmailSender(IOptions<SmtpOptions> options) : IEmailSender<ApplicationUser>
{
    private readonly SmtpOptions options = options.Value;

    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(options.FromName, options.FromEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        // DEBUG ONLY — prints the exact SMTP credentials being used at send time so
        // copy/paste or config-key mistakes are visible. Remove before shipping to production.
        Console.WriteLine($"[SMTP DEBUG] Host='{options.Host}' Port={options.Port} Username='{options.Username}' Password='{options.Password}' (len={options.Password.Length}) FromEmail='{options.FromEmail}'");

        using var client = new SmtpClient();
        await client.ConnectAsync(options.Host, options.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(options.Username, options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(quit: true);
    }
}
