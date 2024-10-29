using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

[Route("[controller]")]
[ApiController]
public class PessoasController : ControllerBase
{
    private TodoDb _context;

    public PessoasController(TodoDb context)
    {
        _context = context;
    }

    // GET: api/pessoas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
    {
        return await _context.Pessoas.Include(p => p.Enderecos).ToListAsync();
    }

    // GET: api/pessoas/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Pessoa>> GetPessoa(int id)
    {
        var pessoa = await _context.Pessoas.Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Id == id);

        if (pessoa == null) return NotFound();

        return pessoa;
    }

    // POST: api/pessoas
    [HttpPost]
    public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
    }

    // POST: api/pessoas/{pessoaId}/enderecos
    [HttpPost("{pessoaId}/enderecos")]
    public async Task<ActionResult<Endereco>> PostEndereco(int pessoaId, Endereco endereco)
    {
        var pessoa = await _context.Pessoas.FindAsync(pessoaId);
        if (pessoa == null) return NotFound();

        endereco.PessoaId = pessoaId;
        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoaId }, endereco);
    }

    // PUT: pessoas/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPessoa(int id, Pessoa inputPessoa)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);
        if (pessoa == null) return NotFound();

        pessoa.Nome = inputPessoa.Nome;
        pessoa.Idade = inputPessoa.Idade;
        pessoa.Email = inputPessoa.Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pessoas/{pessoaId}
    [HttpDelete("{pessoaId}")]
    public async Task<IActionResult> DeletePessoa(int pessoaId)
    {
        var pessoa = await _context.Pessoas.FindAsync(pessoaId);
        if (pessoa == null) return NotFound();

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{pessoaId}/enderecos/{enderecoId}")]
    public async Task<IActionResult> DeleteEndereco(int pessoaId, int enderecoId)
    {
        var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == enderecoId && e.PessoaId == pessoaId);
        if (endereco == null) return NotFound();
    
        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();
    
        return NoContent();

    }
}
