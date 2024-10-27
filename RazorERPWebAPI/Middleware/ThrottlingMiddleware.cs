namespace RazorERPWebAPI.Middleware
{
    public class ThrottlingMiddleware
    {
        private readonly RequestDelegate _request;
        private static readonly Dictionary<string, (int Count, DateTime Expiry)> _requestLog = new();
        private const int RequestLimit = 10; // Limit of requests per user per time window
        private const int TimeWindowInMinutes = 1; // 1-minute time window for throttling


        public ThrottlingMiddleware(RequestDelegate request)
        {
            _request = request;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the username from the current user's identity
            //var username = context.User.Identity.Name;
            var username = "test3";
            // If the user is not authenticated, proceed with the request without throttling
            if (username == null)
            {
                await _request(context);
                return;
            }

            // Check if the user already has a log entry or if the previous entry expired
            if (!_requestLog.TryGetValue(username, out var log) || log.Expiry < DateTime.UtcNow)
            {
                // Create a new log entry or reset the expired entry
                _requestLog[username] = (1, DateTime.UtcNow.AddMinutes(TimeWindowInMinutes));
            }
            else if (log.Count < RequestLimit)
            {
                // Increment the request count within the time window
                _requestLog[username] = (log.Count + 1, log.Expiry);
            }
            else
            {
                // If the request count exceeds the limit, return a 429 Too Many Requests status
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return; // Stop processing and exit the middleware
            }

            // Proceed with the request pipeline if within the rate limit
            await _request(context);
        }
    }
}
