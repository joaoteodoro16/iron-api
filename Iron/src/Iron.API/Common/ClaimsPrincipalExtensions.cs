using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Iron.Api.Common;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Extrai o Id do usuário autenticado a partir da claim 'sub' do token.
    /// Por padrão o handler do JWT mapeia 'sub' para NameIdentifier na leitura
    /// (fallback para o 'sub' bruto caso o mapeamento seja desligado).
    /// Retorna null se a claim estiver ausente ou não for um long válido —
    /// cabe ao controller decidir o que fazer (tipicamente, retornar 401).
    /// </summary>
    public static long? GetUserId(this ClaimsPrincipal user)
    {
        var subject = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return long.TryParse(subject, out var userId) ? userId : null;
    }

    /// <summary>
    /// Versão para endpoints [Authorize], onde o token é garantido pelo middleware.
    /// Se a claim não existir aqui, é BUG do sistema (token emitido sem 'sub' válido),
    /// não erro do cliente — por isso lança em vez de devolver null.
    /// </summary>
    public static long GetRequiredUserId(this ClaimsPrincipal user)
        => user.GetUserId()
           ?? throw new UnauthorizedAccessException("Token sem claim de usuário válida.");
}
