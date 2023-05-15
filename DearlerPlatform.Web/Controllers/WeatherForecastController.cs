using DearlerPlatform.Common.RedisModule;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DearlerPlatform.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    IRepository<Customer> _customerRepo;

   public IRedisWorker RedisWorker { get;}

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IRepository<Customer> customerRepo,
        IRedisWorker redisWorker,
        RedisCore redisCore
        )
    {
        RedisWorker = redisWorker;
        _logger = logger;
        _customerRepo = customerRepo;
    }

    [HttpGet]
    public async Task<string> Get(string userName)
    {
        await RedisWorker.SetStringAsync("UserName",userName,TimeSpan.FromSeconds(20));
        //return await _customerRepo.GetListAsync();
        return await RedisWorker.GetStringAsync("UserName");
    }

    [HttpPost]
    public string HashSet()
    {
        List<UserInfo> userInfos = new();
        userInfos.Add(new()
        {
            Id = 1,
            UserName = "Ace",
            Age = 18
        });
        userInfos.Add(new()
        {
            Id = 2,
            UserName = "DK",
            Age = 18
        });
        userInfos.Add(new()
        {
            Id = 3,
            UserName = "Leo",
            Age = 15
        });
        RedisWorker.SetHashMemory<UserInfo>("ultraman", userInfos, m =>
            new[]
            {
                m.Id.ToString(),
                m.UserName 
            });
        RedisWorker.SetHashMemory<UserInfo>("user",new UserInfo
        {
            Id = 4,
            UserName= "user",
            Age = 18
        });
        return "run";
    }

    [HttpGet("hash")]
    public List<UserInfo> GetHashEntries()
    {
        return RedisWorker.GetHashMemory<UserInfo>("ultraman:*:*");
    }

}

public class UserInfo
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int Age { get; set; }
}
