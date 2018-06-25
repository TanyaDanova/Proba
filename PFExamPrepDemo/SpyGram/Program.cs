using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpyGram
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] privateKey = Console.ReadLine().ToCharArray().Select(a => int.Parse(a.ToString())).ToArray();

            string message = Console.ReadLine();

            SortedDictionary<string, List<string>> output = new SortedDictionary<string, List<string>>();

            while(message != "END")
            {
                string pattern = $@"TO: ([A-Z]+;) MESSAGE: ([{(char)0}-{(char)127}]+);";

                Regex regex = new Regex(pattern);

                Match messageMatch = regex.Match(message);

                if (messageMatch.Success)
                {
                    StringBuilder newMessage = new StringBuilder();

                    int keyIndex = 0;
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (keyIndex == privateKey.Length)
                        {
                            keyIndex = 0;
                        }

                        if (message[i] > 127)
                        {
                            newMessage.Append((char)(0 + privateKey[keyIndex]));
                        }
                        else
                        {
                            newMessage.Append((char)(message[i] + privateKey[keyIndex]));
                        }

                        keyIndex++;
                    }

                    if (!output.ContainsKey(messageMatch.Groups[1].Value))
                    {
                        List<string> messages = new List<string>();
                        output.Add(messageMatch.Groups[1].Value, messages);
                        messages.Add(newMessage.ToString());
                    }
                    else
                    {
                        output[messageMatch.Groups[1].Value].Add(newMessage.ToString());
                    }
                }
               
                message = Console.ReadLine();
            }

            foreach (var item in output.Values)
            {
                foreach (string text in item)
                Console.WriteLine(text);
            }
        }
    }
}
