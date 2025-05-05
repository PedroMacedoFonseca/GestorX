using System;
using System.Security.Cryptography;
using System.Text;

public static class Seguranca
{
    public static string GerarHashSenha(string senha)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(senha);
            byte[] hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }

    public static bool VerificarSenha(string senhaInformada, string hashArmazenado)
    {
        string hashInformado = GerarHashSenha(senhaInformada);
        return hashInformado == hashArmazenado;
    }
}
