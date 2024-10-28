namespace Model.requests;

public interface IRequestHandler
{
    // Interface que define um manipulador de requisição genérico
    public interface IRequest<T>
    {
        Task<T> HandleAsync(params object[] args);
    }
}



