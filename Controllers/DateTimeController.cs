using Microsoft.AspNetCore.Mvc;

namespace sro.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DateTimeController : ControllerBase
{
    private readonly ILogger<DateTimeController> _logger;

    public DateTimeController(ILogger<DateTimeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Retorna a data e hora atual do servidor
    /// </summary>
    /// <returns>Data e hora atual</returns>
    [HttpGet]
    public ActionResult<object> GetCurrentDateTime()
    {
        var currentDateTime = DateTime.Now;
        var utcDateTime = DateTime.UtcNow;

        var response = new
        {
            LocalDateTime = currentDateTime,
            UtcDateTime = utcDateTime,
            TimeZone = TimeZoneInfo.Local.DisplayName,
            Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds(),
            FormattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            FormattedUtcDateTime = utcDateTime.ToString("yyyy-MM-dd HH:mm:ss")
        };

        _logger.LogInformation("DateTime requested at {DateTime}", currentDateTime);

        return Ok(response);
    }

    /// <summary>
    /// Retorna apenas a data atual
    /// </summary>
    /// <returns>Data atual</returns>
    [HttpGet("date")]
    public ActionResult<object> GetCurrentDate()
    {
        var currentDate = DateTime.Today;

        var response = new
        {
            Date = currentDate,
            FormattedDate = currentDate.ToString("yyyy-MM-dd"),
            DayOfWeek = currentDate.DayOfWeek.ToString(),
            DayOfYear = currentDate.DayOfYear
        };

        return Ok(response);
    }

    /// <summary>
    /// Retorna apenas a hora atual
    /// </summary>
    /// <returns>Hora atual</returns>
    [HttpGet("time")]
    public ActionResult<object> GetCurrentTime()
    {
        var currentTime = DateTime.Now.TimeOfDay;

        var response = new
        {
            Time = currentTime,
            FormattedTime = DateTime.Now.ToString("HH:mm:ss"),
            Hour = DateTime.Now.Hour,
            Minute = DateTime.Now.Minute,
            Second = DateTime.Now.Second
        };

        return Ok(response);
    }
}