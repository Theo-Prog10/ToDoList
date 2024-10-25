using Microsoft.AspNetCore.Mvc;
using Model;
using Model.requests;

namespace Controller;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly GetAllTodosHandler _getAllHandler;
    private readonly GetTodoByIdHandler _getByIdHandler;
    private readonly CreateTodoHandler _createHandler;
    private readonly UpdateTodoHandler _updateHandler;
    private readonly DeleteTodoHandler _deleteHandler;

    public TodoController(
        GetAllTodosHandler getAllHandler,
        GetTodoByIdHandler getByIdHandler,
        CreateTodoHandler createHandler,
        UpdateTodoHandler updateHandler,
        DeleteTodoHandler deleteHandler)
    {
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodoItems()
    {
        var todos = await _getAllHandler.HandleAsync();
        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodoItem(int id)
    {
        var todo = await _getByIdHandler.HandleAsync(id);
        return todo is not null ? Ok(todo) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> PostTodoItem(Todo todo)
    {
        var createdTodo = await _createHandler.HandleAsync(todo);
        return CreatedAtAction(nameof(GetTodoItem), new { id = createdTodo.Id }, createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(int id, Todo todo)
    {
        var success = await _updateHandler.HandleAsync(id, todo);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var success = await _deleteHandler.HandleAsync(id);
        return success ? NoContent() : NotFound();
    }
}