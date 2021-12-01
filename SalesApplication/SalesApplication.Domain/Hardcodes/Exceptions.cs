using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Hardcodes
{
    public static class ExceptionTexts
    {
        //Genérico
        public static string EntityNotFound(string entityInfo)
        {
            return $"A entidade {entityInfo} não foi encontrada.";
        }

        //Produtos
        public static string NoStockAvailable(string entityInfo)
        {
            return $"Não há estoque disponível para o produto {entityInfo}";
        }
    }
}
