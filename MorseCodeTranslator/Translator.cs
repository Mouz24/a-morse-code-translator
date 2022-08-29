using System;
using System.Text;

#pragma warning disable CA1304
#pragma warning disable CA1062
#pragma warning disable S2368

namespace MorseCodeTranslator
{
    public static class Translator
    {
        public static string TranslateToMorse(string message)
        {
            if (message is null)
            {
                throw new ArgumentNullException($"{message}");
            }

            var res = new StringBuilder();
            for (int i = 0; i < message.Length; i++)
            {
               res.Append(CheckTable(MorseCodes.CodeTable, message[i]));
               if (message[i] != ',' && message[i] != '.' && message[i] != ' ' && i + 1 != message.Length)
               {
                    res.Append(' ');
               }
            }

            if (res.Length > 0 && res[res.Length - 1] == ' ')
            {
                return new string(res.ToString().ToCharArray(), 0, res.Length - 1);
            }

            return res.ToString();
        }

        public static string TranslateToText(string morseMessage)
        {
            if (morseMessage is null)
            {
                throw new ArgumentNullException(nameof(morseMessage));
            }

            var sb = new StringBuilder();
            var temp = new StringBuilder();
            int start = 0;
            int mark = 0;
            for (int i = 0; i < morseMessage.Length; i++)
            {
                if (morseMessage[i] == ' ')
                {
                    mark = i;
                    for (int j = start; j < mark; j++)
                    {
                        temp.Append(morseMessage[j]);
                    }

                    start = i + 1;
                    string temp2 = FromTable(MorseCodes.CodeTable, temp.ToString());
                    sb.Append(temp2);
                    temp.Remove(0, temp.Length);
                }
                else if (i == morseMessage.Length - 1)
                {
                    mark = i + 1;
                    for (int j = start; j < mark; j++)
                    {
                        temp.Append(morseMessage[j]);
                    }

                    start = i + 1;
                    string temp2 = FromTable(MorseCodes.CodeTable, temp.ToString());
                    sb.Append(temp2);
                    temp.Remove(0, temp.Length);
                }
            }

            return sb.ToString();
        }

        public static void WriteMorse(char[][] codeTable, string message, StringBuilder morseMessageBuilder, char dot = '.', char dash = '-', char separator = ' ')
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (morseMessageBuilder is null)
            {
                throw new ArgumentNullException(nameof(morseMessageBuilder));
            }

            if (codeTable is null)
            {
                throw new ArgumentNullException(nameof(codeTable));
            }

            for (int i = 0; i < message.Length; i++)
            {
                morseMessageBuilder.Append(CheckTable1(codeTable, message[i]));
                if (message[i] != ',' && message[i] != dot && message[i] != ' ' && i + 1 != message.Length)
                {
                    morseMessageBuilder.Append(separator);
                }
            }

            if (morseMessageBuilder.Length > 0 && morseMessageBuilder[morseMessageBuilder.Length - 1] == '.')
            {
                var res = new string(morseMessageBuilder.ToString().ToCharArray(), 0, morseMessageBuilder.Length - 1);
                morseMessageBuilder.Remove(0, morseMessageBuilder.Length);
                morseMessageBuilder.Append(res);
            }

            morseMessageBuilder.Replace('.', separator);
            morseMessageBuilder.Replace('*', dot);
            morseMessageBuilder.Replace('=', dash);
            if (morseMessageBuilder.Length > 0 && morseMessageBuilder[morseMessageBuilder.Length - 1] == ' ')
            {
                var res = new string(morseMessageBuilder.ToString().ToCharArray(), 0, morseMessageBuilder.Length - 1);
                morseMessageBuilder.Remove(0, morseMessageBuilder.Length);
                morseMessageBuilder.Append(res);
            }
        }

        public static void WriteText(char[][] codeTable, string morseMessage, StringBuilder messageBuilder, char dot = '.', char dash = '-', char separator = ' ')
        {
            if (morseMessage is null)
            {
                throw new ArgumentNullException(nameof(morseMessage));
            }

            if (messageBuilder is null)
            {
                throw new ArgumentNullException(nameof(messageBuilder));
            }

            if (codeTable is null)
            {
                throw new ArgumentNullException(nameof(codeTable));
            }

            var temp = new StringBuilder();
            int start = 0;
            int mark = 0;
            if (morseMessage.Contains('=', StringComparison.Ordinal))
            {
                for (int i = 0; i < morseMessage.Length; i++)
                {
                    if (morseMessage[i] == '.')
                    {
                        mark = i;
                        for (int j = start; j < mark; j++)
                        {
                            temp.Append(morseMessage[j]);
                        }

                        start = i + 1;
                        string temp2 = FromTable1(MorseCodes.CodeTable, temp.ToString());
                        messageBuilder.Append(temp2);
                        temp.Remove(0, temp.Length);
                    }
                    else if (i == morseMessage.Length - 1)
                    {
                        mark = i + 1;
                        for (int j = start; j < mark; j++)
                        {
                            temp.Append(morseMessage[j]);
                        }

                        start = i + 1;
                        string temp2 = FromTable1(MorseCodes.CodeTable, temp.ToString());
                        messageBuilder.Append(temp2);
                        temp.Remove(0, temp.Length);
                    }
                }
            }
            else
            {
                var sb2 = new StringBuilder();
                sb2.Append(morseMessage);
                var res = TranslateToText(sb2.ToString());
                messageBuilder.Append(res);
            }
        }

        public static string CheckTable1(char[][] table, char forcheck)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < table.Length; i++)
            {
               if (char.ToLower(table[i][0]) == char.ToLower(forcheck))
                {
                    for (int j = 1; j < table[i].Length; j++)
                    {
                        sb.Append(table[i][j]);
                    }
               }
            }

            sb.Replace('.', '*');
            sb.Replace('-', '=');
            sb.Replace(' ', '.');
            return sb.ToString();
        }

        public static string CheckTable(char[][] table, char forcheck)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < table.Length; i++)
            {
                if (char.ToLower(table[i][0]) == char.ToLower(forcheck))
                {
                    for (int j = 1; j < table[i].Length; j++)
                    {
                        sb.Append(table[i][j]);
                    }
                }
            }

            return sb.ToString();
        }

        public static string FromTable1(char[][] table, string forcheck)
        {
            var sb = new StringBuilder();
            string res = null;
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 1; j < table[i].Length; j++)
                {
                    sb.Append(table[i][j]);
                }

                sb.Replace('.', '*');
                sb.Replace('-', '=');
                if (sb.ToString() == forcheck)
                {
                    res = table[i][0].ToString();
                }

                sb.Remove(0, sb.Length);
            }

            return res;
        }

        public static string FromTable(char[][] table, string forcheck)
        {
            var sb = new StringBuilder();
            string res = null;
            for (int i = 0; i < table.Length; i++)
            {
                for (int j = 1; j < table[i].Length; j++)
                {
                    sb.Append(table[i][j]);
                }

                if (sb.ToString() == forcheck)
                {
                    res = table[i][0].ToString();
                }

                sb.Remove(0, sb.Length);
            }

            return res;
        }
    }
}
