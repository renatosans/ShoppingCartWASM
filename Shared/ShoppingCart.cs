namespace wasmSample
{
    public class ShoppingCart
    {
        public String nomeCliente;
        public List<Produto> produtosAdicionados;
        public Double desconto;

        public ShoppingCart(String nomeCliente)
        {
            this.nomeCliente = nomeCliente;
            produtosAdicionados = new List<Produto>();
        }
        internal void adicionar(Produto produto)
        {
            if (produto is not null) 
                produtosAdicionados.Add(produto);
        }
    }
}
