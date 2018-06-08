using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AlalaSecurePasswordLib
{
    public class PasswordProtector
    {
        byte[] _additionalEntropy = { 9, 8, 7, 6, 5 };

        public void Protect(string input)
        {
            //Encrypt the data.
            var encryptedData = ProtectData(
                Encoding.Unicode.GetBytes(input));

            WriteBytesToFile(encryptedData);
        }

        public string Unprotect(string input)
        {
            var encryptedData = ReadBytesFromFile(input);

            // Decrypt the data and store in a byte array.
            var originalData = UnprotectData(encryptedData);

            return Encoding.Unicode.GetString(originalData);
        }

        private byte[] ProtectData(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                //  only by the same current user.
                return ProtectedData.Protect(data, _additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        private byte[] UnprotectData(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, _additionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        // Write an array of bytes to a file
        private void WriteBytesToFile(byte[] bytes)
        {
            string path = "Secret.dat";

            // Delete the file if it exists
            if (File.Exists(path))
                File.Delete(path);

            //Create the file and write the bytes. Use of 'using' will close
            // the stream at the end of the block
            using (FileStream fs = File.Create(path))
            {
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        // Reads an array of bytes from a file
        private byte[] ReadBytesFromFile(string inputFile)
        {
            string path = inputFile;

            // Delete the file if it exists
            if (!File.Exists(path))
            {
                Console.WriteLine("Error: input file not found");
                Environment.Exit(1);
            }

            //Create the file and write the bytes. Use of 'using' will close
            // the stream at the end of the block
            int nRead = 0;
            byte[] b = new byte[1024];
            using (FileStream fs = File.OpenRead(path))
            {
                nRead = fs.Read(b, 0, b.Length);
            }

            if (nRead > 0)
            {
                byte[] b2 = new byte[nRead];
                Array.Copy(b, b2, nRead);
                return b2;
            }
            else
                return null;
        }
    }
}
