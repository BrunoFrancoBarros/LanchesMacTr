using LanchesMac.Context;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider service)
        {
            //define uma sessão
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            //define um serviço do tipo do contexto
            var context = service.GetService<AppDbContext>();

            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //atribui o id do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);

            //retorna o carrinho com o contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        }

        public void AdicionarAoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(l => l.Lanche.LancheId == lanche.LancheId &&
                                     l.CarrinhoCompraId == CarrinhoCompraId);
            if (carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItens.Add(carrinhoCompraItem);
            }
            else
            {
                carrinhoCompraItem.Quantidade++;
            }

            _context.SaveChanges();
        }

        public void RemoverDoCarrinho(Lanche lanche)
        {
            var carrinhoCompraItem = _context.CarrinhoCompraItens.SingleOrDefault(l => l.Lanche.LancheId == lanche.LancheId &&
                                 l.CarrinhoCompraId == CarrinhoCompraId);

            if (carrinhoCompraItem != null && carrinhoCompraItem?.Quantidade > 1)
            {
                carrinhoCompraItem.Quantidade--;
            }
            else
            {
                _context.CarrinhoCompraItens.Remove(carrinhoCompraItem);
            }
            _context.SaveChanges();
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItens()
        {
            return CarrinhoCompraItens ?? (CarrinhoCompraItens = _context.CarrinhoCompraItens
                                                               .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                                                               .Include(s => s.Lanche)
                                                               .ToList());
        }

        public void LimparCarrinho()
        {
            var carrinhoItens = _context.CarrinhoCompraItens.Where(c => c.CarrinhoCompraId == CarrinhoCompraId);

            _context.CarrinhoCompraItens.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }

        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItens
                        .Where(s => s.CarrinhoCompraId == CarrinhoCompraId)
                        .Select(s => s.Lanche.Preco * s.Quantidade).Sum();

            return total;
        }
    }
}