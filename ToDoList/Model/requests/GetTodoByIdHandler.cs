namespace Model.requests;


// Classe que lida com a operação GET para obter um item Todo pelo ID
public class GetTodoByIdHandler : IRequestHandler.IRequest<Todo?>
{
    private readonly TodoDb _context;

    public GetTodoByIdHandler(TodoDb context)
    {
        _context = context;
    }

    public async Task<Todo?> HandleAsync(params object[] args)
    {
        var id = (int)args[0]; // Recebe o ID como parâmetro
        return await _context.Todos.FindAsync(id);
    }
}

