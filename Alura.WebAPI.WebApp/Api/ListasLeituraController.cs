using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeituraController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ListasLeituraController(IRepository<Livro> repo)
        {
            _repo = repo;
        }

        private Lista CriarLista(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                    .Where(l => l.Lista == tipo)
                    .Select(l => l.ToApi())
                    .ToList()
            };
        }

        [HttpGet]
        public IActionResult TodasListas()
        {
            var paraLer = CriarLista(TipoListaLeitura.ParaLer);
            var lendo = CriarLista(TipoListaLeitura.Lendo);
            var lidos = CriarLista(TipoListaLeitura.Lidos);

            var colecao = new List<Lista>();
            
            colecao.Add(paraLer);
            colecao.Add(lendo);
            colecao.Add(lidos);

            return Ok(colecao);
        }

        [HttpGet("{tipo}")]
        public IActionResult Recuperar(TipoListaLeitura tipo)
        {
            var lista = CriarLista(tipo);
            return Ok(lista);
        }
    }
}
