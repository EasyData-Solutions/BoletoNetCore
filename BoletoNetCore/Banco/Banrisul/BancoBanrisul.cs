using System;
using System.Collections.Generic;
using BoletoNetCore.Exceptions;
using BoletoNetCore.Extensions;

namespace BoletoNetCore
{
    internal sealed partial class BancoBanrisul : BancoFebraban<BancoBanrisul>, IBanco
    {
        public BancoBanrisul()
        {
            Codigo = 41;
            Nome = "Banrisul";
            Digito = "8";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { "1" };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoBanrisul>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados("PAGÁVEL EM QUALQUER BANCO.", "SAC BANRISUL: 0800 646 1515<br>OUVIDORIA BANRISUL: 0800 644 2200", "", 8);

            Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia} {contaBancaria.Conta}{contaBancaria.DigitoConta}";

        }

        public override string FormatarNomeArquivoRemessa(int numeroSequencial)
        {
            return $"CB{DateTime.Now.Date.Day:00}{DateTime.Now.Date.Month:00}{numeroSequencial.ToString().PadLeft(9, '0').Right(2)}.rem";
        }
    }
}