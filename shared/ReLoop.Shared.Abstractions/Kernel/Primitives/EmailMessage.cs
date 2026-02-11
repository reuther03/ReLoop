using System.Diagnostics.CodeAnalysis;

namespace ReLoop.Shared.Abstractions.Kernel.Primitives;

public class EmailMessage
{
    public string Email { get; }
    public string Subject { get; }
    public string Body { get; }

    public EmailMessage(string email, string subject, [StringSyntax(StringSyntaxAttribute.Xml)] string body)
    {
        Email = email;
        Subject = subject;
        Body = body;
    }
}