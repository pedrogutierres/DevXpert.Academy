using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevXpert.Academy.Financeiro.Domain.Pagamentos.ValuesObejcts
{
    public sealed record DadosCartao(string nome, string numero, string vencimento, string ccvCvc);
}
