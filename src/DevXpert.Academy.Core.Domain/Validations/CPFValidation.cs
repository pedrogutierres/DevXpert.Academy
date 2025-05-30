﻿namespace DevXpert.Academy.Core.Domain.Validations
{
    public class CPFValidation
    {
        public static bool Validar(string cpf) => Validar(cpf, out _);
        public static bool Validar(string cpf, out string msgDeErro)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                msgDeErro = "O CPF não foi preenchido.";
                return false;
            }

            if (cpf.Length > 11)
            {
                msgDeErro = "O CPF deve ser conter no máximo 11 caracteres.";
                return false;
            }

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
            {
                msgDeErro = "O CPF não pode ser igual a uma sequência identica ou incremental.";
                return false;
            }

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    msgDeErro = "CPF inválido.";
                    return false;
                }
            }
            else if (numeros[9] != 11 - resultado)
            {
                msgDeErro = "CPF inválido.";
                return false;
            }

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                {
                    msgDeErro = "CPF inválido.";
                    return false;
                }
            }
            else if (numeros[10] != 11 - resultado)
            {
                msgDeErro = "CPF inválido.";
                return false;
            }

            msgDeErro = null;
            return true;
        }
    }
}
