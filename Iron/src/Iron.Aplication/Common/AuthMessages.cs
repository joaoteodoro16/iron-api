namespace Iron.Aplication.Common;

/// <summary>
/// Mensagens de retorno dos usecases de autenticação.
/// Centralizadas aqui para que uma alteração de texto seja feita em um único lugar.
/// </summary>
public static class AuthMessages
{
    //Validação
    public const string InvalidFields = "Existem campos inválidos";
    public const string InvalidEmail = "Email inválido";
    public const string InvalidPhoneNumber = "Número de telefone inválido";

    //Cadastro
    public const string EmailAlreadyInUse = "Já existe um usuário cadastrado com esse email.";
    public const string UserCreationFailed = "Não foi possível criar o usuário";
    public const string UserCreated = "Usuário criado com sucesso.";

    //Login / Refresh
    //Resposta genérica de propósito: não revelar se o email existe ou se a senha está errada.
    public const string InvalidCredentials = "Email ou senha inválidos.";
    public const string Unauthenticated = "Usuário não autenticado";
    public const string UserAuthenticated = "Usuário autenticado com sucesso!";
    public const string UnexpectedError = "Ocorreu um erro inesperado";
}
