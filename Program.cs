using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 3 && args.Length % 2 == 1)
            {
                var secretKey = new byte[16];
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(secretKey);
                secretKey = Encoding.Default.GetBytes(BitConverter.ToString(secretKey).Replace("-", string.Empty)); // weird, but it works
                int comp_move = new Random().Next(args.Length);
                HMACSHA256 hash = new HMACSHA256(secretKey);
                var hmac_move = hash.ComputeHash(Encoding.Default.GetBytes(args[comp_move]));
                Console.WriteLine("Available moves:");
                for (int i = 0; i < args.Length; i++)
                    Console.WriteLine(i.ToString() + " - " + args[i]);
                Console.WriteLine("HMAC: " + BitConverter.ToString(hmac_move).Replace("-", string.Empty));
                Console.WriteLine("Enter your move: ");
                string move = Console.ReadLine();
                int your_turn;
                try
                {
                    your_turn = Int32.Parse(move);
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Error: Wrong inpun string '{move}'");
                    Console.ReadLine();
                    return;
                }
                if (your_turn < 0 || your_turn > args.Length - 1)
                {
                    Console.WriteLine($"Error: Input number must be in range from 0 to '{args.Length - 1}'");
                    Console.ReadLine();
                    return;
                }
                Console.WriteLine("Computer move: " + args[comp_move]);
                if (your_turn == comp_move)
                {
                    Console.WriteLine("Even");
                    Console.WriteLine("HMAC key: " + Encoding.Default.GetString(secretKey));
                    Console.ReadLine();
                    return;
                }
                if (your_turn == 0 && comp_move > your_turn + (int)(args.Length) / 2)
                {
                    your_turn = args.Length;
                }

                if (comp_move == 0 && your_turn > comp_move + (int)(args.Length) / 2)
                {
                    comp_move = args.Length;
                }
                if (comp_move == args.Length - 1 && your_turn < comp_move - (int)(args.Length) / 2)
                {
                    comp_move = -1;
                }
                if (your_turn == args.Length - 1 && comp_move < your_turn - (int)(args.Length) / 2)
                {
                    your_turn = -1;
                }
                if (your_turn > comp_move)
                {
                    Console.WriteLine("You losed!");
                }
                else if (your_turn < comp_move)
                {
                    Console.WriteLine("You won!");
                }
                else
                {
                    Console.WriteLine("Even");
                }
                Console.WriteLine("HMAC key: " + Encoding.Default.GetString(secretKey));
            }
            else
            {
                Console.WriteLine("Error: Wrong arguments. Argument number must be odd and bigger than 3");
            }
            Console.ReadLine();
        }
    }
}
