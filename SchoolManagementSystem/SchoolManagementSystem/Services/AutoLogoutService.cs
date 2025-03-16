using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Services
{
    public class AutoLogoutService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoLogoutService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _scopeFactory.CreateScope())
                {
                    var _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var timeoutThreshold = DateTime.UtcNow.AddMinutes(-2);
                    var inactiveUsers = _context.loginHistories
                        .Where(l => l.LastActivityTime < timeoutThreshold && l.LogoutTime == null).ToList();
                    foreach(var user in inactiveUsers)
                    {
                        user.LogoutTime = DateTime.UtcNow;
                    }
                    if (inactiveUsers.Count > 0)
                    {
                        await _context.SaveChangesAsync();
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
