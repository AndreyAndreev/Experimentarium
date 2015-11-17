﻿using System;
using Experimentarium.Cryptography;

namespace Experimentarium
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("App started");

            string first =  "315c4eeaa8b5f8aaf9174145bf43e1784b8fa00dc71d885a804e5ee9fa40b16349c146fb778cdf2d3aff021dfff5b403b510d0d0455468aeb98622b137dae857553ccd8883a7bc37520e06e515d22c954eba5025b8cc57ee59418ce7dc6bc41556bdb36bbca3e8774301fbcaa3b83b22";
            string second = "234c02ecbbfbafa3ed18510abd11fa724fcda2018a1a8342cf064bbde548b12b07df44ba7191d9606ef4081ffde5ad46a5069d9f7f543bedb9c861bf29c7e205132eda9382b0bc2c5c4b45f919cf3a9f1cb74151f6d551f4480c82b2cb24cc5b028aa76eb7b4ab24171ab3cdadb8356f";
            byte[] xored = XorHelper.XorHexStringsToBytes(first, second);


            string result = StringConverter.ConvertBytesToAsciiString(xored);

            Console.WriteLine(result);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }
    }
}
