using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class WeatherForecast
{
    public int Id { get; set; }             // Needed for CRUD
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly List<WeatherForecast> Forecasts = new List<WeatherForecast>();
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // GET: /WeatherForecast
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        if (!Forecasts.Any())
        {
            var rng = new Random();
            Forecasts.AddRange(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }));
        }
        return Forecasts;
    }

    // POST: /WeatherForecast
    [HttpPost]
    public IActionResult Post([FromBody] WeatherForecast forecast)
    {
        forecast.Id = Forecasts.Count > 0 ? Forecasts.Max(f => f.Id) + 1 : 1;
        Forecasts.Add(forecast);
        return CreatedAtAction(nameof(Get), new { id = forecast.Id }, forecast);
    }

    // PUT: /WeatherForecast/{id}
    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] WeatherForecast forecast)
    {
        var existingForecast = Forecasts.FirstOrDefault(f => f.Id == id);
        if (existingForecast == null) return NotFound();

        existingForecast.Date = forecast.Date;
        existingForecast.TemperatureC = forecast.TemperatureC;
        existingForecast.Summary = forecast.Summary;

        return NoContent();
    }

    // DELETE: /WeatherForecast/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var forecast = Forecasts.FirstOrDefault(f => f.Id == id);
        if (forecast == null) return NotFound();

        Forecasts.Remove(forecast);
        return NoContent();
    }
}