using InvoicePeru.Application.Common.Interfaces.Services;

namespace InvoicePeru.Infrastructure.Services;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}