namespace BackHackathon.Application.Auth.Dtos;

public class LoginRequestDto
{
    public string Usuario { get; set; }
    public string Senha { get; set; }

    public string? CodigoTenant { get; set; }
}