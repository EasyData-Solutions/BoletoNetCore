using BoletoNetCore.Exceptions;
using System;
using System.Collections.Generic;

namespace BoletoNetCore
{
    partial class BancoFebraban<T>
    {
        ///
        /// Como o retorno CNAB400 não tem todos os dados na linha do header
        /// Utiliza o primeiro registro de titulo para pegar mais dados do beneficiario
        ///
        public virtual void CompletarHeaderRetornoCNAB400(string registro)
        {
            if (this.Beneficiario.ContaBancaria == null)
            {
                this.Beneficiario.ContaBancaria = new ContaBancaria();
            }
            if (string.IsNullOrEmpty(this.Beneficiario?.ContaBancaria?.Agencia))
            {
                this.Beneficiario.ContaBancaria.Agencia = registro.Substring(17, this.TamanhoAgencia);
            }

            if (string.IsNullOrEmpty(this.Beneficiario?.ContaBancaria?.Conta))
            {
                var conta = registro.Substring(20, this.TamanhoConta).Trim();
                this.Beneficiario.ContaBancaria.Conta = conta.Substring(0, conta.Length - 1);
                this.Beneficiario.ContaBancaria.DigitoConta = conta.Substring(conta.Length - 1, 1);
            }

            // 01 - cpf / 02 - cnpj
            if (registro.Substring(1, 2) == "01")
                this.Beneficiario.CPFCNPJ = registro.Substring(6, 11);
            else
                this.Beneficiario.CPFCNPJ = registro.Substring(3, 14);
        }

        public virtual void LerHeaderRetornoCNAB400(string registro)
        {
            try
            {
                if (registro.Substring(0, 9) != "02RETORNO")
                    throw new Exception("O arquivo não é do tipo \"02RETORNO\"");

                this.Beneficiario = new Beneficiario();
                this.Beneficiario.Codigo = registro.Substring(26, 20);
                this.Beneficiario.Nome = registro.Substring(46, 30);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao ler HEADER do arquivo de RETORNO / CNAB 400.", ex);
            }
        }

    }
}