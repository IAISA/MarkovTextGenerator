﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovTextGenerator
{
    public class Chain
    {
        public Dictionary<String, List<Word>> words;
        public Dictionary<String, List<Word>> startingWord;
        private Dictionary<String, int> sums;
        private Random rand;
        public string nextPart = "";
        public List<string> startingwords;
        public string parts = "";
        public Chain()
        {
            startingwords = new List<string>();
            words = new Dictionary<String, List<Word>>();
            startingWord = new Dictionary<String, List<Word>>();
            sums = new Dictionary<string, int>();
            rand = new Random(System.Environment.TickCount);
        }

        /// <summary>
        /// Returns a random starting word from the stored list of words
        /// This may not be the best approach.. better may be to actually store
        /// a separate list of actual sentence starting words and randomly choose from that
        /// </summary>
        /// <returns></returns>
        public String GetRandomStartingWord()
        {
            return words.Keys.ElementAt(rand.Next() % words.Keys.Count);
        }

        // Adds a sentence to the chain
        // You can use the empty string to indicate the sentence will end
        //
        // For example, if sentence is "The house is on fire" you would do the following:
        //  AddPair("The", "house")
        //  AddPair("house", "is")
        //  AddPair("is", "on")
        //  AddPair("on", "fire")
        //  AddPair("fire", "")

        public void AddString(String sentence)
        {

            string[] parts = sentence.Split(' ');
            for (int i = 0; i <= parts.Length - 1; i++)
            {
                if (parts.Length - 1 == i)
                {
                    AddPair(parts[i], "");
                }
                else
                {
                    AddPair(parts[i], parts[i + 1]);
                }

            }
            startingwords.Add(parts[0]);
            //System.Console.WriteLine(parts[0]);
        }




        // Adds a pair of words to the chain that will appear in order
        public void AddPair(String word, String word2)
        {
            if (!words.ContainsKey(word))
            {
                sums.Add(word, 1);
                words.Add(word, new List<Word>());
                words[word].Add(new Word(word2));
            }
            else
            {
                bool found = false;
                foreach (Word s in words[word])
                {
                    if (s.ToString() == word2)
                    {
                        found = true;
                        s.Count++;
                        sums[word]++;
                    }
                }

                if (!found)
                {
                    words[word].Add(new Word(word2));
                    sums[word]++;
                }
            }
        }

        /// <summary>
        /// Given a word, randomly chooses the next word.  This should be done
        /// by using the list of words in the words Dictionary.  The provided
        /// code allows you to pick a word from the choices array.  Bear in mind
        /// that each word is not equally likely to occur and has their own probability
        /// of occurring.
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public String GetNextWord(String word)
        {
            if (words.ContainsKey(word))
            {
                double test = rand.NextDouble();

                //Console.WriteLine("I picked the number " + choice );

                List<Word> choice = words[word];

                double sum = 0;

                foreach (Word t in choice)
                {
                    sum += t.Probability;

                    if (sum > test)

                        return t.ToString();
                }
            }

            return "";
        }


        /// <summary>
        /// Generates a full randomly generated sentence based that starts with
        /// startingWord.
        /// </summary>
        /// <param name="startingWord"></param>
        /// <returns></returns>
        public String GenerateSentence(string startingWord)
        {
            int startingWordCount = startingwords.Count;
            int randStartingWord = rand.Next(startingWordCount);
            startingWord = startingwords[randStartingWord];

            for (int i = 0; i <= 100; i++)
            {
                string[] parts = startingWord.Split(' ');
                string daWord = parts[i];
                String nextWord = GetNextWord(daWord);

                startingWord = startingWord + " " + nextWord;

            }


            return startingWord;
        }
        /// <summary>
        /// Updates the probability of choosing a second word at random
        /// for a chain of words attached to a first word.
        /// Example: If the starting word is "The" and the only word that
        /// you ever see following it is the word "cat", then "cat" would
        /// have a probability of following "The" of 1.0.  Another scenario
        /// would involve sentences like:
        /// The cat loves milk, The cat is my friend, The dog is in the yard
        /// In this scenario with the starting word of "The":
        /// - cat would have a probability of 0.66 (appears 66% of the time)
        /// - dog would have a probability of 0.33 (appears 33% of the time)
        /// </summary>
        public void UpdateProbabilities()
        {
            foreach (String word in words.Keys)
            {
                double sum = 0;  // Total sum of all the occurences of each followup word



                // Update the probabilities
                foreach (Word s in words[word])
                {

                    s.Probability = (double)s.Count /
                        sums[word];
                }

            }
        }
    }
}


