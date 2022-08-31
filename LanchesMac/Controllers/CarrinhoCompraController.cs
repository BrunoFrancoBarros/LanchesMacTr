using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class CarrinhoCompraController : Controller
    {
        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public CarrinhoCompraController(ILancheRepository lancheRepository, CarrinhoCompra carrinhoCompra)
        {
            _lancheRepository = lancheRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index()
        {
            var itens = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };

            return View(carrinhoCompraVM);
        }

        public IActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(a => a.LancheId == lancheId);

            if (lanche != null)
            {
                _carrinhoCompra.AdicionarAoCarrinho(lanche);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoverItemNoCarrinhoCompra(int lancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(a => a.LancheId == lancheId);

            if (lanche != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(lanche);
            }

            return RedirectToAction("Index");
        }
    }
}