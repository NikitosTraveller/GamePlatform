using GamePlatform.Models;
using GamePlatform.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GamePlatform.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;
    private readonly IActivityService _activityService;

    public PlayerController(
        IPlayerService playerService, 
        IActivityService activityService)
    {
        _playerService = playerService;
        _activityService = activityService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] Player player)
    {
        await _playerService.CreateOrUpdatePlayerAsync(player);
        await _activityService.AddUniquePlayerAsync(player.Id);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayer(string id)
    {
        var player = await _playerService.GetPlayerAsync(id);
        return player == null ? NotFound() : Ok(player);
    }

    [HttpGet("{id}/stats")]
    public async Task<IActionResult> GetStats(string id)
    {
        var stats = await _playerService.GetStatsAsync(id);
        return Ok(stats);
    }

    [HttpPost("{id}/login")]
    public async Task<IActionResult> Login(string id)
    {
        await _activityService.MarkLoginAsync(id);
        return Ok();
    }

    [HttpGet("activity/daily/{date}")]
    public async Task<IActionResult> DailyActive(string date)
    {
        if (!DateTime.TryParse(date, out var dt)) return BadRequest("Invalid date format");
        var count = await _activityService.CountDailyActivePlayersAsync(dt);
        return Ok(new { date = dt.ToString("yyyy-MM-dd"), activePlayers = count });
    }

    [HttpGet("unique/count")]
    public async Task<IActionResult> UniqueCount()
    {
        var count = await _activityService.GetUniquePlayerCountAsync();
        return Ok(new { uniquePlayers = count });
    }
}
