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
    private readonly IAvaliacaoFisicaService _IAvaliacaoFisicaService;
    private readonly IVendasService _IVendasService;

    public HeathScoreController(IRecuperarPesquisaService iRecuperarPesquisaService, ICalculoScoreService iCalculoScoreService, IAvaliacaoFisicaService iAvaliacaoFisicaService, IVendasService iVendasService)
    {
        _IRecuperarPesquisaService = iRecuperarPesquisaService;
        _ICalculoStore = iCalculoScoreService;
        _IAvaliacaoFisicaService = iAvaliacaoFisicaService;
        _IVendasService = iVendasService;
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
    public async Task<IActionResult> Faixa([FromServices] IMemoryCache cache, [FromRoute] int alunoId)
    {
        if (!cache.TryGetValue("Alunos-Score", out List<Cliente?> clientesAtivos) || clientesAtivos == null)
        {
            clientesAtivos = await _IRecuperarPesquisaService.Faixa(alunoId);
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
    //[HttpGet("{alunoId}")]
    //public async Task<IActionResult> RecuperaAvaliacaoFisica([FromRoute] int alunoId)
    //{
    //    var avaliacaoFisica = await _IAvaliacaoFisicaService.RecuperaAvaliacaoFisica(alunoId);
    //    if (avaliacaoFisica == null || !avaliacaoFisica.Any())
    //    {
    //        return NotFound("Nenhuma avaliação física encontrada para o aluno.");
    //    }
    //    return Ok(avaliacaoFisica);
    //}

    //[HttpGet("{codigoCliente}")]
    //public async Task<IActionResult> RecuperarVendas(int codigoCliente)
    //{
    //    var vendas = await _IVendasService.RecuperarVendas(codigoCliente);
    //    if (vendas == null || !vendas.Any())
    //    {
    //        return NotFound("Nenhuma venda encontrada.");
    //    }
    //    return Ok(vendas);
    //}

    //[HttpGet("{codigoCliente}")]
    //public async Task<IActionResult> RecuperarTreinos(int codigoCliente)
    //{
    //    var treinos = await _IRecuperarPesquisaService.RecuperaTreino(codigoCliente);
    //    if (treinos == null || !treinos.Any())
    //    {
    //        return NotFound("Nenhuma treino encontrado.");
    //    }
    //    return Ok(treinos);
    //}

    //[HttpGet("{codigoCliente}")]
    //public async Task<IActionResult> RecuperarContasAbertas(int codigoCliente)
    //{
    //    var contas = await _IRecuperarPesquisaService.RecuperarContasAbertas(codigoCliente);
    //    if (contas == null || !contas.Any())
    //    {
    //        return NotFound("Nenhuma conta encontrada.");
    //    }
    //    return Ok(contas);
    //}
}

