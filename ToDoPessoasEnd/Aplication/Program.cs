using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o contexto do banco de dados
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("CadastroPessoas"));

// Adiciona o controlador como serviço
builder.Services.AddControllers();

var app = builder.Build();

// Mapeia os endpoints dos controladores
app.MapControllers();

app.Run();
