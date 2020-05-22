using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Hadoop.Avro.Serializers;
using Microsoft.Hadoop.Avro;
using Avro.File;
using Newtonsoft.Json;
namespace TestingCefSharp
{
    public class BoundObject
    {
        List<List<int[]>> structure = new List<List<int[]>>();
        List<BinaryWriter> binsList = new List<BinaryWriter>();
        List<StringBuilder> csvList = new List<StringBuilder>();
        List<string> jsonsList = new List<string>();


        public BoundObject()
        {

        }
        public string init(int partitions, int total, int series,string testType)
        {
            partitions = partitions > 0 ? partitions : 1;
            DateTime init = DateTime.Now;  
            int interval = total / partitions;
            structure = new List<List<int[]>>();
            Random rd = new Random();

            switch (testType)
            {
                case "bin":
                    {
                        for (int i = 0; i < partitions; i++)
                        {
                            BinaryWriter parame = new BinaryWriter(new MemoryStream());
                            for (int j = 0; j < interval; j++)
                            {
                                parame.Write(rd.Next(0, 10000000)); 
                                parame.Write(rd.Next(0, 10000000));
                            }
                            parame.Close();
                            binsList.Add(parame);
                        }
                    }
                    break;
                case "csv":
                    {
                        for (int i = 0; i < partitions; i++)
                        {
                            StringBuilder parame = new StringBuilder();
                            for (int j = 0; j < interval; j++)
                            {
                                parame.AppendLine(string.Format("{0},{1}", rd.Next(0, 10000000), rd.Next(0, 10000000)));
                            }
                            csvList.Add(parame);
                        }
                    }
                    break;
                case "json":
                    {
                        
                        for (int i = 0; i < partitions; i++)
                        {
                            List<string> parame = new List<string>();
                            for (int j = 0; j < interval; j++)
                            {
                                parame.Add(rd.Next(0, 10000000).ToString());
                                parame.Add(rd.Next(0, 10000000).ToString());
                            }
                            var aux = parame.ToArray();
                            var json = JsonConvert.SerializeObject(aux);
                            jsonsList.Add(json);
                        }
                    }
                    break;
                case "avro":
                    {
                        //BinaryEncoder encoder = new BinaryEncoder()
                        //vroSerializer avroFile = new AvroSerializer();
                        //AvroSerializer.

                    }
                    break;
                case "parquet":
                    {

                    }
                    break;
                case "bound":
                    {
                        for (int i = 0; i < partitions; i++)
                        {
                            List<int[]> parame = new List<int[]>();
                            int linealIndex = 0;
                            for (int j = 0; j < interval; j++)
                            {
                                int x = rd.Next(linealIndex, linealIndex + 10) + 8;
                                int y = rd.Next(x, x+20);
                                int[] aux = new int[]
                                {
                                    x,
                                    y
                                };
                                parame.Add(aux);
                                linealIndex = x;
                            }
                            structure.Add(parame);
                        }
                        
                    }
                    break;
            }
           

            DateTime finishDateTime = DateTime.Now;
            

            return (finishDateTime - init).TotalSeconds.ToString();
        }

        public List<int[]> bound(int index)
        {
            return structure[index];
        }

        public dynamic bin(int index)
        {
            return binsList[index];
        }


    }
}
