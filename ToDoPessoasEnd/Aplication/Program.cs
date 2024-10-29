using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("CadastroPessoas"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
