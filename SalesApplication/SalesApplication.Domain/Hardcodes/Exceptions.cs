namespace SalesApplication.Domain.Hardcodes
{
    public static class ExceptionTexts
    {
        //Genérico
        public static string EntityNotFound(string entityInfo)
        {
            return $"A entidade {entityInfo} não foi encontrada.";
        }

        public static string ArgumentNotValid()
        {
            return "Verifique os dados inseridos e tente novamente";
        }

        //Produtos
        public static string NoStockAvailable(string entityInfo)
        {
            return $"Não há estoque disponível para o produto {entityInfo}";
        }
    }
}
