using BoletoNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoletoNetCore
{
    [CarteiraCodigo("4")]
    internal class BancoNordesteCarteira4 : ICarteira<BancoNordeste>
    {
        internal static Lazy<ICarteira<BancoNordeste>> Instance { get; } = new Lazy<ICarteira<BancoNordeste>>(() => new BancoNordesteCarteira4());

        private BancoNordesteCarteira4()
        {

        }
        public void FormataNossoNumero(Boleto boleto)
        {

            // Nosso número não pode ter mais de 7 dígitos

            if (string.IsNullOrWhiteSpace(boleto.NossoNumero) || boleto.NossoNumero == "0000000")
            {
                // Banco irá gerar Nosso Número
                boleto.NossoNumero = new String('0', 7);
                boleto.NossoNumeroDV = "0";
                boleto.NossoNumeroFormatado = "000/00000000000-0";
            }
            else
            {
                // Nosso Número informado pela empresa
                if (boleto.NossoNumero.Length > 7)
                    throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve conter 7 dígitos.");
                boleto.NossoNumero = boleto.NossoNumero.PadLeft(7, '0');

                boleto.NossoNumeroDV = boleto.NossoNumero.CalcularDVNordeste();
                boleto.NossoNumeroFormatado = $"{boleto.NossoNumero}-{boleto.NossoNumeroDV}/{boleto.Banco.Beneficiario.CodigoTransmissao.PadLeft(2, '0')}";
            }

        }

        public string FormataCodigoBarraCampoLivre(Boleto boleto)
        {
            var contaBancaria = boleto.Banco.Beneficiario.ContaBancaria;
            return $"{contaBancaria.Agencia}{contaBancaria.Conta}{contaBancaria.DigitoConta}{boleto.NossoNumero}{boleto.NossoNumeroDV}{boleto.Banco.Beneficiario.CodigoTransmissao}{"000"}";
        }
    }
}
