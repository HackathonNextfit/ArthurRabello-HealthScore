using BackHackathon.Application.Exemplo;
using BackHackathon.Application.Exemplo.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace BackHackathon.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ExemploController : ControllerBase
{
    private readonly IExemploAppService _exemploAppService;

    public ExemploController(IExemploAppService exemploAppService)
    {
        _exemploAppService = exemploAppService;
    }

    [HttpGet(Name = "GetAmbienteUsuario")]
    public async Task<IActionResult> GetAmbienteUsuario([FromQuery] AmbienteUsuarioRequestDto requestDto)
    {
        var result = await _exemploAppService.RecuperarAmbienteUsuario(requestDto);
        return Ok(result);
    }
    
    [HttpGet(Name = "GetEnums")]
    public async Task<IActionResult> GetEnums([FromQuery] EnumsRequestDto request)
    {
        var result = await _exemploAppService.RecuperarEnums(request);
        return Ok(result);
    }
}