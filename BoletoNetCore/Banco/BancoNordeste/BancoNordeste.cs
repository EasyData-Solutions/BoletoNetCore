using System;
using System.Collections.Generic;
using BoletoNetCore.Exceptions;
using BoletoNetCore.Extensions;
using static System.String;

namespace BoletoNetCore
{
    internal sealed partial class BancoNordeste : BancoFebraban<BancoNordeste>, IBanco
    {
        public BancoNordeste()
        {
            Codigo = 4;
            Nome = "Banco Nordeste";
            Digito = "3";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
        }

        public override string FormatarNomeArquivoRemessa(int numeroSequencial)
        {
            return $"CB{DateTime.Now.Date.Day:00}{DateTime.Now.Date.Month:00}{numeroSequencial.ToString().PadLeft(9, '0').Right(2)}.rem";
        }
    }
}
