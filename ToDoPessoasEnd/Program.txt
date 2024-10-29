using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("CadastroPessoas"));
var app = builder.Build();


app.MapGet("/pessoas", async (TodoDb db) =>
    await db.Pessoas.Include(p => p.Enderecos).ToListAsync());


app.MapGet("/pessoas/{id}", async (int id, TodoDb db) =>
    await db.Pessoas.Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Id == id)
        is Pessoa pessoa
        ? Results.Ok(pessoa)
        : Results.NotFound());

//cadastrar uma pessoa
app.MapPost("/pessoas", async (Pessoa pessoa, TodoDb db) =>
{
    db.Pessoas.Add(pessoa);
    await db.SaveChangesAsync();
    return Results.Created($"/pessoas/{pessoa.Id}", pessoa);
});

//cadastrar um endereço
app.MapPost("/pessoas/{pessoaId}/enderecos", async (int pessoaId, Endereco endereco, TodoDb db) =>
{
    var pessoa = await db.Pessoas.FindAsync(pessoaId);
    if (pessoa is null) return Results.NotFound();

    endereco.PessoaId = pessoaId;
    db.Enderecos.Add(endereco);
    await db.SaveChangesAsync();

    return Results.Created($"/pessoas/{pessoaId}/enderecos/{endereco.Id}", endereco);
});

app.Run();

//atualizar uma pessoa
app.MapPut("/pessoas/{id}", async (int id, Pessoa inputPessoa, TodoDb db) =>
{
    var pessoa = await db.Pessoas.FindAsync(id);

    if (pessoa is null) return Results.NotFound();

    pessoa.Nome = inputPessoa.Nome;
    pessoa.Idade = inputPessoa.Idade;
    pessoa.Email = inputPessoa.Email;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Para deletar uma pessoa
app.MapDelete("/pessoas/delete/{pessoaId}", async (int pessoaId, TodoDb db) =>
{
    var pessoa = await db.Pessoas.FindAsync(pessoaId);
    if (pessoa == null) return Results.NotFound("Pessoa não encontrada.");

    db.Pessoas.Remove(pessoa);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


//deletar um endereço de uma pessoa
app.MapDelete("/pessoas/{pessoaId}/enderecos/{enderecoId}", async (int pessoaId, int enderecoId, TodoDb db) =>
{
    var endereco = await db.Enderecos.FirstOrDefaultAsync(e => e.Id == enderecoId && e.PessoaId == pessoaId);
    if (endereco is null) return Results.NotFound();

    db.Enderecos.Remove(endereco);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
