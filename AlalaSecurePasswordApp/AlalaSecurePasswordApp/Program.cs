using System;

using AlalaSecurePasswordLib;

namespace AlalaSecurePasswordApp
{
    public class Program
    {
        static byte[] _additionalEntropy = { 9, 8, 7, 6, 5 };

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong syntax... Please use one of the following calls:");
                Console.WriteLine("SecureDiogenesPasswords.exe -p <original password>");
                Console.WriteLine("SecureDiogenesPasswords.exe -u <encrypted password>");
                return;
            }

            var option = args[0];
            var input = args[1];

            switch (option)
            {
                case "-p":
                    {
                        var protector = new PasswordProtector();
                        protector.Protect(input);
                        break;
                    }
                case "-u":
                    {
                        var protector = new PasswordProtector();
                        var originalPassword = protector.Unprotect(input);

                        Console.WriteLine($"The original password is: {originalPassword}");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Wrong option... Please use one of the following calls:");
                        Console.WriteLine("SecureDiogenesPasswords.exe -p <original password>");
                        Console.WriteLine("SecureDiogenesPasswords.exe -u <encrypted password>");
                        break;
                    }
            }
        }
    }
}
