using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace AlalaSecurePasswordLib
{
    public class PasswordProtector
    {
        byte[] _additionalEntropy = { 9, 8, 7, 6, 5 };

        /// <summary>
        /// Protects a given string and write the derived encrypted
        /// cipher to a binary file.
        /// </summary>
        /// <param name="input">The given string is to be encrypted.</param>
        public void Protect(string input)
        {
            //Encrypt the data.
            var encryptedData = ProtectData(
                Encoding.Unicode.GetBytes(input));

            WriteBytesToFile(encryptedData);
        }

        /// <summary>
        /// Unprotects the data of an encrypted binary file 
        /// given its path.
        /// </summary>
        /// <param name="input">The path of the binary file to be decrypted.</param>
        /// <returns>The string respects to the decrypted binary file data.</returns>
        public string Unprotect(string input)
        {
            var encryptedData = ReadBytesFromFile(input);

            // Decrypt the data and store in a byte array.
            var originalData = UnprotectData(encryptedData);

            return Encoding.Unicode.GetString(originalData);
        }

        /// <summary>
        /// Protects a given byte array using DataProtectionScope.LocalMachine
        /// </summary>
        /// <param name="data">The input byte array to be encrypted.</param>
        /// <returns>A byte array that corresponds to the encrypted data.</returns>
        private byte[] ProtectData(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.LocalMachine. The result can be decrypted
                // only at the same local machine.
                return ProtectedData.Protect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Unprotects a given byte array using DataProtectionScope.LocalMachine.
        /// </summary>
        /// <param name="data">The input byte array to be decrypted.</param>
        /// <returns>A byte array that corresponds to the decrypted data.</returns>
        private byte[] UnprotectData(byte[] data)
        {
            try
            {
                // Decrypt the data using DataProtectionScope.LocalMachine.
                return ProtectedData.Unprotect(data, _additionalEntropy, DataProtectionScope.LocalMachine);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Writes an array of bytes to a file
        /// </summary>
        /// <param name="bytes">The input byte array to be written to the file.</param>
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

        /// <summary>
        /// Reads an array of bytes from a file.
        /// </summary>
        /// <param name="inputFile">The path of the file to be read.</param>
        /// <returns>A byte array consists of the file data read.</returns>
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
