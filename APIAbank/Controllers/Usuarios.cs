using Core.APIAbank.Interfaces;
using Core.APIAbank.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIAbank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Usuarios : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public Usuarios(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _usuarioRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _usuarioRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            usuario.fechanacimiento = DateTime.Now;
            usuario.fechamodificacion = DateTime.Now;
            await _usuarioRepository.AddAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = usuario.id }, usuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            var existingUser = await _usuarioRepository.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound();

            usuario.id = id;
            usuario.fechamodificacion = DateTime.Now;
            await _usuarioRepository.UpdateAsync(usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return NotFound();

            await _usuarioRepository.DeleteAsync(id);
            return NoContent();
        }

    }
}
