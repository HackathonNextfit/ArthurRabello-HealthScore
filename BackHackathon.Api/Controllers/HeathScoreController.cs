using BackHackathon.Application.Entities;
using BackHackathon.Application.Exemplo;
using BackHackathon.Application.Exemplo.Dtos;
using BackHackathon.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
namespace BackHackathon.Api.Controllers;



[ApiController]
[Route("api/[Controller]/[Action]")]
public class HeathScoreController : ControllerBase
{
    private readonly IRecuperarPesquisaService _IRecuperarPesquisaService;
    private readonly ICalculoScoreService _ICalculoStore;

    public HeathScoreController(IRecuperarPesquisaService iRecuperarPesquisaService, ICalculoScoreService iCalculoScoreService)
    {
        _IRecuperarPesquisaService = iRecuperarPesquisaService;
        _ICalculoStore = iCalculoScoreService;
    }

    [HttpGet("{alunoId}")]
    public async Task<IActionResult> RecuperarClientesAtivos([FromRoute] int alunoId)
    {
        var result = await _IRecuperarPesquisaService.RecuperarClientesAtivos();

        if (result == null)
        {
            return NotFound("Nenhum cliente ativo encontrado.");
        }
        var aluno = result.Where(registro => registro.Id == alunoId);
        return Ok(aluno);
    }

    //Lista todos os clientes e os Scores atuais dos mesmos
    [HttpPost]
    public async Task<IActionResult> Atualizar([FromServices] IMemoryCache cache)
    {
        var clientesAtivos = await _IRecuperarPesquisaService.RecuperarClientesAtivos();

        if (clientesAtivos == null)
        {
            return NotFound("Nenhum cliente ativo encontrado.");
        }
        clientesAtivos = await _ICalculoStore.CalcularScore(clientesAtivos);

        var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        cache.Set("Alunos-Score", clientesAtivos, options);

        return Ok(clientesAtivos);
    }

    //Metodo que mostra o aluno e o Score atual dele(Atualizado pós Hackathon)
    [HttpGet("{alunoId}")]
    public async Task<IActionResult> MostraScore([FromServices] IMemoryCache cache, [FromRoute] int alunoId)
    {
        if (!cache.TryGetValue("Alunos-Score", out List<Cliente?> clientesAtivos) || clientesAtivos == null)
        {
            clientesAtivos = await _IRecuperarPesquisaService.MostraScore(alunoId);
            await _ICalculoStore.CalcularScore(clientesAtivos);
            if (clientesAtivos == null)
                return NotFound("Dados não encontrados.");
        }

        var aluno = clientesAtivos.FirstOrDefault(c => c != null && c.Id == alunoId);

        if (aluno == null)
            return NotFound("Aluno não encontrado.");
        var resultado = clientesAtivos
            .Select(c => new
            {
                c.Id,
                c.Nome,
                Score = c.Score
            });

        return Ok(resultado);
    }
    //Retorna a Faixa de enjamento do cliente (Feito Pós Hackathon)
    [HttpGet("{alunoId}")]
    public async Task<IActionResult> FaixaScore([FromServices] IMemoryCache cache, [FromRoute] int alunoId)
    {
        if (!cache.TryGetValue("Alunos-Score", out List<Cliente?> clientesAtivos) || clientesAtivos == null)
        {
            clientesAtivos = await _IRecuperarPesquisaService.FaixaScore(alunoId);
            await _ICalculoStore.CalcularScore(clientesAtivos);
            if (clientesAtivos == null)
                return NotFound("Dados não encontrados.");
        }

        var aluno = clientesAtivos.FirstOrDefault(c => c != null && c.Id == alunoId);

        if (aluno == null)
            return NotFound("Aluno não encontrado.");
        var resultado = clientesAtivos
            .Select(c => new {
                c.Id,
                c.Nome,
                Faixa = c.Faixa
            });

        return Ok(resultado);
    }

}

