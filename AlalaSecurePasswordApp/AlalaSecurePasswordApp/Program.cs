using System;

using AlalaSecurePasswordLib;

namespace AlalaSecurePasswordApp
{
    public class Program
    {
        static byte[] _additionalEntropy = { 9, 8, 7, 6, 5 };

        /// <summary>
        /// Main function of a simple application developed to protect/unprotect
        /// Alala passwords used for SAP DI connection.
        /// </summary>
        /// <param name="args">An array of arguments including application options and 
        /// the input data depending on the specific option.</param>
        static void Main(string[] args)
        {
            // Check whether there is 2 arguments...
            if (args.Length != 2)
            {
                // If not, print error/help messages to the console and return.
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
                        // Protect an input (plaintext) password.
                        var protector = new PasswordProtector();
                        protector.Protect(input);
                        break;
                    }
                case "-u":
                    {
                        // Unprotect a password stored in an encrypted binary file.
                        // The input of "unprotect" is the path of the binary file.
                        var protector = new PasswordProtector();
                        var originalPassword = protector.Unprotect(input);

                        // Print to the console the original password value.
                        Console.WriteLine($"The original password is: {originalPassword}");
                        break;
                    }
                default:
                    {
                        // In case of wrong option, print error/help messages to the console and return.
                        Console.WriteLine("Wrong option... Please use one of the following calls:");
                        Console.WriteLine("SecureDiogenesPasswords.exe -p <original password>");
                        Console.WriteLine("SecureDiogenesPasswords.exe -u <encrypted password>");
                        break;
                    }
            }
        }
    }
}
