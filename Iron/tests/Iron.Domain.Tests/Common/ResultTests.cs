using AwesomeAssertions.Types;
using Iron.Domain.Common;

namespace Iron.Domain.Tests.Common;

public class ResultTests
{
    //Literal escrito à mão: o valor esperado nunca deve ser derivado do código sob teste.
    private const string DefaultSuccessMessage = "Operação realizada com sucesso";
    private const string DefaultFailureMessage = "Não foi possível concluir essa operação";

    #region Ok
    [Fact]
    public void Ok_WhenCalled_ShouldReturnSuccessResult()
    {
        var result = Result.Ok();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Equal(string.Empty, result.Error);
        Assert.Equal(DefaultSuccessMessage, result.Message);
    }

    [Fact]
    public void Ok_GivenNoMessage_ShouldUseDefaultSuccessMessage()
    {
        var result = Result.Ok();

        Assert.Equal(DefaultSuccessMessage, result.Message);
    }

    [Fact]
    public void Ok_GivenSuccessMessage_ShouldReturnSuccessResult()
    {
        var expectedMessage = "success";
        // Act
        var result = Result.Ok(expectedMessage);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Empty(result.Errors);
        Assert.Equal(expectedMessage, result.Message);
    }

    [Fact]
    public void Ok_WhenCalled_ShouldCarryFailureErrorType()
    {
        var result = Result.Ok();

        Assert.Equal(ErrorType.Failure, result.ErrorType);
    }
    #endregion

    #region OkOfT

    [Fact]
    public void OkOfT_GivenValue_ShouldStoreValue()
    {
        var expectedValue = 42L;

        var result = Result.Ok(expectedValue);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedValue, result.Value);
        Assert.Empty(result.Errors);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void OkOfT_GivenNoMessage_ShouldUseDefaultSuccessMessage()
    {
        var result = Result.Ok(42L);

        Assert.Equal(DefaultSuccessMessage, result.Message);
    }

    [Fact]
    public void OkOfT_GivenCustomMessage_ShouldUseIt()
    {
        var expectedMessage = "Usuário criado com sucesso.";

        var result = Result.Ok(42L, expectedMessage);

        Assert.Equal(expectedMessage, result.Message);
    }
    #endregion

    #region Fail

    [Fact]
    public void Fail_GivenErrorMessage_ShouldReturnFailResult()
    {
        var expectedMessage = "error";
        var result = Result.Fail(expectedMessage);
        Assert.Equal(expectedMessage, result.Error);
        Assert.NotEmpty(result.Errors);
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public void Fail_GivenNoErrorMessage_ShouldUseDefaultFailureMessage()
    {
        var expectedMessage = "error";
        var result = Result.Fail(expectedMessage);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void Fail_GivenSingleError_AndFailureType_ShouldUseDefaultFailureMessage()
    {
        var error = "error";
        var result = Result.Fail(error, ErrorType.Failure);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void Fail_GivenSingleError_AndNotFailureType_ShouldUseError()
    {
        var expectedError = "error";
        var result = Result.Fail(expectedError, ErrorType.Conflict);
        Assert.Equal(expectedError, result.Message);
    }

    [Fact]
    public void Fail_GivenSingleError_AndMessage_ShouldUseMessage()
    {
        var expectedMessage = "message";
        var result = Result.Fail("error", message: expectedMessage);
        Assert.Equal(expectedMessage, result.Message);
    }

    [Fact]
    public void Fail_GivenErrorsList_ShouldReturnFailResult()
    {
        var errors = new List<string> { "error1", "error2" };
        var result = Result.Fail(errors);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(2, result.Errors.Count);
    }

    [Fact]
    public void Fail_GivenEmptyErrorsList_ShouldUseDefaultFailureMessage()
    {
        var errors = new List<string>();
        var result = Result.Fail(errors);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void Fail_GivenErrorList_AndFailureType_ShouldUseDefaultFailureMessage()
    {
        var errors = new List<string> { "error1" };
        var result = Result.Fail(errors, ErrorType.Failure);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    #endregion

    #region FailOfT

    [Fact]
    public void FailOfT_GivenSingleError_ShouldReturnFailResult()
    {
        var expectedError = "error";

        var result = Result.Fail<long>(expectedError);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(default, result.Value);
    }

    [Fact]
    public void FailOfT_GivenSingleError_AndNotFailureErrorType_ShouldUseErrorForMessage()
    {
        var error = "error";
        var result = Result.Fail<long>(error, errorType: ErrorType.Validation);
        Assert.Equal(error, result.Message);
    }

    [Fact]
    public void FailOfT_GivenSingleError_AndFailureErrorType_ShouldUseDefaultFailureMessageForMessage()
    {
        var error = "error";
        var result = Result.Fail<long>(error, errorType: ErrorType.Failure);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void FailOfT_GivenSingleError_AndMessage_ShouldUseMessageForMessage()
    {
        var error = "error";
        var expectedMessage = "message";
        var result = Result.Fail<long>(error, errorType: ErrorType.Failure, message: expectedMessage);
        Assert.Equal(expectedMessage, result.Message);
    }

    [Fact]
    public void FailOfT_GivenErrorList_ShouldReturnFailResult()
    {
        var errors = new List<string> { "error1", "error2" };
        var result = Result.Fail<long>(errors);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.NotEmpty(result.Errors);
        Assert.Equal(2, result.Errors.Count());
        Assert.Equal(default, result.Value);
    }

    [Fact]
    public void FailOfT_GivenEmptyErrorList_ShouldUseDefaultFailureMessage()
    {
        var errors = new List<string>();
        var result = Result.Fail<long>(errors);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void FailOfT_GivenErrorList_AndFailureErrorType_ShouldUseDefaultFailureMessage()
    {
        var errors = new List<string> { "error1", "error2" };
        var result = Result.Fail<long>(errors, ErrorType.Failure);
        Assert.Equal(DefaultFailureMessage, result.Message);
    }

    [Fact]
    public void FailOfT_GivenErrorList_AndNotFailureErrorType_ShouldUseFirstErrorForMessage()
    {
        var errors = new List<string> { "error1", "error2" };
        var result = Result.Fail<long>(errors, ErrorType.Conflict);
        Assert.Equal(errors.First(), result.Message);
    }

    #endregion

    #region Try

    [Fact]
    public void Try_GivenFunction_ShouldReturnOkResult()
    {
        var result = Result.Try(() => 42L);
        Assert.True(result.IsSuccess);
        Assert.Equal(42L, result.Value);
    }

    [Fact]
    public void Try_GivenArgumentException_ShouldReturnValidationFailure()
    {
        var result = Result.Try<long>(() => throw new ArgumentException("inválido"));

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.ErrorType);
    }

    [Fact]
    public void Try_GivenInvalidOperationException_ShouldReturnConflictFailure()
    {
        var result = Result.Try<long>(() => throw new InvalidOperationException("inválido"));
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Conflict, result.ErrorType);
    }

    [Fact]
    public void Try_GivenUnexpectedException_ShouldReturnGenericFailure()
    {
        var result = Result.Try<long>(() => throw new InvalidCastException("boom"));

        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Failure, result.ErrorType);
    }

    #endregion
}
