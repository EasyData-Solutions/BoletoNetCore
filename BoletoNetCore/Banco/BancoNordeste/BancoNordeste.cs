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
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoNordeste>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados("PAGÁVEL EM QUALQUER BANCO.", "", "", 7);

            var codigoBeneficiario = Beneficiario.Codigo;
            Beneficiario.Codigo = codigoBeneficiario.Length <= 10 ? codigoBeneficiario.PadLeft(10, '0') : throw BoletoNetCoreException.CodigoBeneficiarioInvalido(codigoBeneficiario, 10);

            Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia} / {contaBancaria.Conta}-{contaBancaria.DigitoConta}";
        }

        public override string FormatarNomeArquivoRemessa(int numeroSequencial)
        {
            return $"CB{DateTime.Now.Date.Day:00}{DateTime.Now.Date.Month:00}{numeroSequencial.ToString().PadLeft(9, '0').Right(2)}.rem";
        }
    }
}
