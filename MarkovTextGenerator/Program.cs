﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MarkovTextGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Chain chain = new Chain();
            Console.WriteLine("Welcome to Marky Markov's Random Text Generator!");
            Console.WriteLine("Enter some text I can learn from (enter ! to finish): ");
            //LoadText("Sample.txt", chain);
            while (true)
            {
                Console.Write("> ");
                String line = Console.ReadLine();
                if (line == "!")
                    break;
                chain.AddString(line);  // Let the chain process this string
            }
            // Now let's update all the probabilities with the new data
            chain.UpdateProbabilities();
            // Okay now for the fun part
            Console.WriteLine("Done learning!  Now give me a word and I'll generate a sentence.");
            Console.Write("> ");
            String word = chain.startingwords[0];
            Console.WriteLine(chain.GenerateSentence(word));
            String word2 = Console.ReadLine();
            String nextWord3 = chain.GetNextWord(word);
            Console.WriteLine("I predict the next word will be " + nextWord3);
        }
        static void LoadText(string filename, Chain chain)
        {
            int counter = 0;
            String line;
            string path = Path.Combine(Environment.CurrentDirectory, $"data\\{filename}");
            StreamReader file = new StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                chain.AddString(line);
                counter++;
            }
            file.Close();
        }
    }
}