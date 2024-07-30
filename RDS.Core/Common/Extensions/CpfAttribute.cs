namespace RDS.Core.Common.Extensions;

public partial class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (string.IsNullOrEmpty(value?.ToString()))
        {
            return true;
        }

        var cpf = value as string;
        if (string.IsNullOrWhiteSpace(cpf))
        {
            return false;
        }

        // Remove caracteres não numéricos
        // cpf = Regex.Replace(cpf, "[^0-9]", "");

        cpf = CpfRegex().Replace(cpf, "");

        if (cpf.Length != 11)
        {
            return false;
        }

        // Verifica se todos os dígitos são iguais
        if (new string(cpf[0], cpf.Length) == cpf)
        {
            return false;
        }

        // Calcula os dígitos verificadores
        int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        }

        int resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        string digito = resto.ToString();
        tempCpf = tempCpf + digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        }

        resto = soma % 11;
        if (resto < 2)
        {
            resto = 0;
        }
        else
        {
            resto = 11 - resto;
        }

        digito = digito + resto.ToString();

        return cpf.EndsWith(digito);
    }

    [GeneratedRegex("[^0-9]")]
    private static partial Regex CpfRegex();
}