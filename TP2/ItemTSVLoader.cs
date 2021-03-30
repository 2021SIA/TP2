using System.Collections.Generic;
using System.IO;
using System.Text;
using TP2.Models;

namespace TP2
{
    public class ItemTSVLoader
    {
        public static IEnumerable<Item> Load(string path, Item.Type type)
        {
            var sb = new StringBuilder();
            var lineParser = new ItemLineParser(type);
            int bufferSize = 20480;
            List<Item> list = new List<Item>();

            using var reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize);
            while (reader.ReadByte() != '\n') ;
            while (reader.CanRead)
            {
                sb.Clear();
                while (true)
                {
                    var readByte = reader.ReadByte();
                    switch (readByte)
                    {
                        case -1:
                            goto endOfFile;
                        case '\r':
                            continue;
                        case '\n':
                            goto endOfLine;
                        default:
                            sb.Append((char)readByte);
                            break;
                    }
                }
            endOfLine:

                list.Add(lineParser.ParseLine(sb));
            }
        endOfFile:;
            return list;
        }

        //public static IEnumerable<Item> Load(Stream stream, Item.Type type)
        //{
        //    using var reader = new TextFieldParser(stream);
        //    reader.TextFieldType = FieldType.Delimited;
        //    reader.Delimiters = new string[] { "\t" };
        //    reader.ReadLine();
        //    while (!reader.EndOfData)
        //    {
        //        var row = reader.ReadFields();
        //        yield return new Item(int.Parse(row[0]),
        //                              double.Parse(row[1]),
        //                              double.Parse(row[2]),
        //                              double.Parse(row[3]),
        //                              double.Parse(row[4]),
        //                              double.Parse(row[5]),
        //                              type);
        //    }
        //}



        private interface ILineParser<T>
        {
            T ParseLine(StringBuilder line);
        }
        private class ItemLineParser : ILineParser<Item>
        {
            public ItemLineParser(Item.Type type)
            {
                Type = type;
            }

            public Item.Type Type { get; }

            public Item ParseLine(StringBuilder line)
            {
                var startIndex = 0;
                int id = ParseSectionAsInt(line, ref startIndex);
                double force = ParseSectionAsDouble(line, ref startIndex);
                double agility = ParseSectionAsDouble(line, ref startIndex);
                double expertice = ParseSectionAsDouble(line, ref startIndex);
                double resistance = ParseSectionAsDouble(line, ref startIndex);
                double life = ParseSectionAsDouble(line, ref startIndex);
                var valueHolder = new Item(id, force, agility, expertice, resistance, life, Type);
                return valueHolder;
            }

            private static double ParseSectionAsDouble(StringBuilder line, ref int startIndex)
            {
                double val = 0d;
                bool seenDot = false;
                double fractionCounter = 10d;
                bool flip = false;

                int length = line.Length;
                for (var index = startIndex; index < length; index++)
                {
                    char c = line[index];
                    switch (c)
                    {
                        case '\t':
                            startIndex = index + 1;
                            goto exit;
                        case '-':
                            flip = true;
                            continue;
                        case '.':
                            seenDot = true;
                            continue;
                        default:
                            if (seenDot == false)
                            {
                                val *= 10d;
                                val += c - '0';
                            }
                            else
                            {
                                val += (c - '0') / fractionCounter;
                                fractionCounter *= 10d;
                            }
                            break;
                    }
                }
            exit:

                return flip ? -val : val;
            }

            private static decimal ParseSectionAsDecimal(StringBuilder line, ref int startIndex)
            {
                decimal val = 0m;
                bool seenDot = false;
                decimal fractionCounter = 10m;
                bool flip = false;

                int length = line.Length;
                for (var index = startIndex; index < length; index++)
                {
                    char c = line[index];
                    switch (c)
                    {
                        case '\t':
                            startIndex = index + 1;
                            goto exit;
                        case '-':
                            flip = true;
                            continue;
                        case '.':
                            seenDot = true;
                            continue;
                        default :
                            if (seenDot == false)
                            {
                                val *= 10m;
                                val += c - '0';
                            }
                            else
                            {
                                val += (c - '0') / fractionCounter;
                                fractionCounter *= 10m;
                            }
                            break;
                    }
                }
                exit:

                return flip ? -val : val;
            }

            private static int ParseSectionAsInt(StringBuilder line, ref int startIndex)
            {
                int val = 0;
                bool flip = false;

                int length = line.Length;
                for (var index = startIndex; index < length; index++)
                {
                    char c = line[index];
                    switch(c)
                    {
                        case '\t':
                            startIndex = index + 1;
                            goto exit;
                        case '-':
                            flip = true;
                            continue;
                        default:
                            val *= 10;
                            val += c - '0';
                            break;
                    }
                }
                exit:

                return flip ? -val : val;
            }
        }
    }
}
