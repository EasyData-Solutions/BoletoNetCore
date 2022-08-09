using System;
using System.Collections.Generic;
using BoletoNetCore.Exceptions;
using static System.String;
using BoletoNetCore.Extensions;

namespace BoletoNetCore
{
    internal sealed partial class BancoItau : BancoFebraban<BancoItau>, IBanco
    {
        public BancoItau()
        {
            Codigo = 341;
            Nome = "Itaú";
            Digito = "7";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoItau>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados("ATÉ O VENCIMENTO EM QUALQUER BANCO. APÓS O VENCIMENTO SOMENTE NO ITAÚ.", "", "", 5);

            Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia} / {contaBancaria.Conta}-{contaBancaria.DigitoConta}";
        }

        public override string FormatarNomeArquivoRemessa(int numeroSequencial)
        {
            return $"CB{DateTime.Now.Date.Day:00}{DateTime.Now.Date.Month:00}{numeroSequencial.ToString().PadLeft(9, '0').Right(2)}.rem";
        }

    }
}


