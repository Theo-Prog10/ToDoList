namespace Model.requests;


// Classe que lida com a operação POST para criar um novo item Todo
public class CreateTodoHandler : IRequestHandler.IRequest<Todo>
{
    private readonly TodoDb _context;

    public CreateTodoHandler(TodoDb context)
    {
        _context = context;
    }

    public async Task<Todo> HandleAsync(params object[] args)
    {
        var todo = (Todo)args[0]; // Recebe o item Todo como parâmetro
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return todo;
    }
}

