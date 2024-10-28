namespace Model.requests;


// Classe que lida com a operação DELETE para remover um item Todo
public class DeleteTodoHandler : IRequestHandler.IRequest<bool>
{
    private readonly TodoDb _context;

    public DeleteTodoHandler(TodoDb context)
    {
        _context = context;
    }

    public async Task<bool> HandleAsync(params object[] args)
    {
        var id = (int)args[0];
        var todo = await _context.Todos.FindAsync(id);

        if (todo is null) return false;

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }
}
