namespace Model.requests;


// Classe que lida com a operação PUT para atualizar um item Todo
public class UpdateTodoHandler : IRequestHandler.IRequest<bool>
{
    private readonly TodoDb _context;

    public UpdateTodoHandler(TodoDb context)
    {
        _context = context;
    }

    public async Task<bool> HandleAsync(params object[] args)
    {
        var id = (int)args[0];
        var item = (Todo)args[1];
        
        var todo = await _context.Todos.FindAsync(id);
        if (todo is null) return false;

        todo.Name = item.Name;
        todo.IsComplete = item.IsComplete;
        await _context.SaveChangesAsync();
        return true;
    }
}

