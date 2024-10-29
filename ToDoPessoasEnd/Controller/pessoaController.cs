using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

[ApiController]
public class PessoasController : ControllerBase
{
    private TodoDb _context;

    public PessoasController(TodoDb context)
    {
        _context = context;
    }

    
    [HttpGet("/pessoas")]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
    {
        return await _context.Pessoas.Include(p => p.Enderecos).ToListAsync();
    }

   
    [HttpGet("pessoas/{id}")]
    public async Task<ActionResult<Pessoa>> GetPessoa(int id)
    {
        var pessoa = await _context.Pessoas.Include(p => p.Enderecos).FirstOrDefaultAsync(p => p.Id == id);

        if (pessoa == null) return NotFound();

        return pessoa;
    }

    
    [HttpPost("pessoas")]
    public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
    }

    
    [HttpPost("pessoas/{pessoaId}/enderecos")]
    public async Task<ActionResult<Endereco>> PostEndereco(int pessoaId, Endereco endereco)
    {
        var pessoa = await _context.Pessoas.FindAsync(pessoaId);
        if (pessoa == null) return NotFound();

        endereco.PessoaId = pessoaId;
        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoaId }, endereco);
    }

    
    [HttpPut("pessoas/{id}")]
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

    
    [HttpDelete("pessoas/{pessoaId}")]
    public async Task<IActionResult> DeletePessoa(int pessoaId)
    {
        var pessoa = await _context.Pessoas.FindAsync(pessoaId);
        if (pessoa == null) return NotFound();

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("pessoas/{pessoaId}/enderecos/{enderecoId}")]
    public async Task<IActionResult> DeleteEndereco(int pessoaId, int enderecoId)
    {
        var endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == enderecoId && e.PessoaId == pessoaId);
        if (endereco == null) return NotFound();
    
        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();
    
        return NoContent();

    }
}
