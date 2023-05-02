using Microsoft.AspNetCore.Mvc;

namespace SSE.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    [HttpGet]
    public async Task Get()
    {
        Response.Headers.Add("Content-Type", "text/event-stream");
        Response.Headers.Add("Cache-Control", "no-cache");
        Response.Headers.Add("Connection", "keep-alive");
        while (!HttpContext.RequestAborted.IsCancellationRequested)
        {
            await Response.WriteAsync($"data: {DateTime.Now}\n\n");
            await Response.Body.FlushAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
    
    [HttpGet("/")]
    public async Task GetIndex()
    {
        Response.Headers.Add("Content-Type", "text/html");
        await HttpContext.Response.WriteAsync(""""
            <!DOCTYPE html>
            <html>
            <head>
                <title>Server Sent Events</title>
            </head>
            <body>
                <h1>Server Sent Events</h1>
                <div id="result"></div>
                <script>
                    var source = new EventSource('/events');
                    source.onmessage = function (event) {
                        document.getElementById('result').innerHTML += event.data + '<br />';
                    };
                </script>
            </body>
            </html>
            """");
    }
}