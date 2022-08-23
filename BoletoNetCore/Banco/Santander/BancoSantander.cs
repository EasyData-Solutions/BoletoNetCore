using System;
using System.Collections.Generic;
using BoletoNetCore.Exceptions;
using BoletoNetCore.Extensions;
using static System.String;

namespace BoletoNetCore
{
    internal sealed partial class BancoSantander : BancoFebraban<BancoSantander>, IBanco
    {
        public BancoSantander()
        {
            Codigo = 033;
            Nome = "Santander";
            Digito = "7";
            IdsRetornoCnab400RegistroDetalhe = new List<string> { };
            RemoveAcentosArquivoRemessa = true;
        }

        public void FormataBeneficiario()
        {
            var contaBancaria = Beneficiario.ContaBancaria;

            if (!CarteiraFactory<BancoSantander>.CarteiraEstaImplementada(contaBancaria.CarteiraComVariacaoPadrao))
                throw BoletoNetCoreException.CarteiraNaoImplementada(contaBancaria.CarteiraComVariacaoPadrao);

            contaBancaria.FormatarDados("PAGÁVEL EM QUALQUER BANCO.", "", "", digitosConta: 9);

            if (contaBancaria.CarteiraComVariacaoPadrao != "1")
            {
                var codigoBeneficiario = Beneficiario.Codigo;
                Beneficiario.Codigo = codigoBeneficiario.Length <= 7
                    ? codigoBeneficiario.PadLeft(7, '0')
                    : throw BoletoNetCoreException.CodigoBeneficiarioInvalido(codigoBeneficiario, 7);

                Beneficiario.CodigoFormatado = $"{contaBancaria.Agencia} / {Beneficiario.Codigo}";
            }
        }

        public override string FormatarNomeArquivoRemessa(int numeroSequencial)
        {
            return $"CB{DateTime.Now.Date.Day:00}{DateTime.Now.Date.Month:00}{numeroSequencial.ToString().PadLeft(9, '0').Right(2)}.rem";
        }





    }
}