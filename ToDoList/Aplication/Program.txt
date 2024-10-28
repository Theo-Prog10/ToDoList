using Microsoft.EntityFrameworkCore;
using Model;
using Model.requests;

var builder = WebApplication.CreateBuilder(args);

// Adicionando os controladores
builder.Services.AddControllers();

// Registrando o DbContext para ser usado com o banco em memória
builder.Services.AddDbContext<TodoDb>(options =>
    options.UseInMemoryDatabase("TodoList"));

// Registrando os handlers para injeção de dependências
builder.Services.AddScoped<GetAllTodosHandler>();
builder.Services.AddScoped<GetTodoByIdHandler>();
builder.Services.AddScoped<CreateTodoHandler>();
builder.Services.AddScoped<UpdateTodoHandler>();
builder.Services.AddScoped<DeleteTodoHandler>();

var app = builder.Build();

// Mapeando as rotas para os controladores
app.MapControllers();

app.Run();