namespace Model.requests;
using Microsoft.EntityFrameworkCore;

// Classe que lida com a operação GET para obter todos os itens Todo
public class GetAllTodosHandler : IRequestHandler.IRequest<IEnumerable<Todo>>
{
    private readonly TodoDb _context;

    public GetAllTodosHandler(TodoDb context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Todo>> HandleAsync(params object[] args)
    {
        return await _context.Todos.ToListAsync();
    }
}
