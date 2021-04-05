using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TP2.Models;

namespace TP2
{
    public class GeneticEngineServer
    {

        private class GenerationMetrics
        {
            public double AvgFitness { get; set; }

            public double MinFitness { get; set; }

            public double MaxFitness { get; set; }

            public double Diversity { get; set; }

            public Character BestCharacter { get; set; }
            public GenerationMetrics() { }
        }
        private HttpListener listener;
        private string url;
        private BufferBlock<GenerationMetrics> metricsBuffer;

        public GeneticEngineServer(string url)
        {
            this.url = url;
            this.metricsBuffer = new BufferBlock<GenerationMetrics>();
        }
        private void MeasureGenerationMetrics(IEnumerable<Character> generation)
        {
            double avgFitness = 0, minFitness = double.MaxValue, maxFitness = 0;
            Character bestCharacter = generation.First();
            IDictionary<Character, int> equalGroups = new Dictionary<Character, int>();
            int generationSize = generation.Count();
            foreach (Character c in generation)
            {
                avgFitness += c.Fitness;
                minFitness = c.Fitness < minFitness ? c.Fitness : minFitness;
                maxFitness = c.Fitness > maxFitness ? c.Fitness : maxFitness;
                bestCharacter = c.Fitness > bestCharacter.Fitness ? c : bestCharacter;
                equalGroups[c] = equalGroups.ContainsKey(c) ? equalGroups[c] + 1 : 1;
            }
            avgFitness /= generationSize;
            double diversity = equalGroups.Aggregate(0.0,(total,g) => total + (-g.Value / (double)generationSize) * Math.Log2(g.Value / (double)generationSize));
            metricsBuffer.Post(new GenerationMetrics() { 
                AvgFitness = avgFitness,
                MinFitness = minFitness,
                MaxFitness = maxFitness,
                BestCharacter = bestCharacter,
                Diversity = diversity
            });
        }
        public async Task HandleIncomingConnections()
        {
            bool runServer = true;
            // Create a Http server and start listening for incoming connections
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Server listening...");
            while (runServer)
            {
                // Will wait here until we hear from a connection
                HttpListenerContext ctx = await listener.GetContextAsync();

                // Peel out the requests and response objects
                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse resp = ctx.Response;
                resp.AddHeader("Access-Control-Allow-Origin", "*");
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                    resp.StatusCode = 204;
                }
                else if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/start"))
                {

                    //Start
                    string jsonConfiguration;
                    using (var reader = new StreamReader(req.InputStream, req.ContentEncoding))
                    {
                        jsonConfiguration = reader.ReadToEnd();
                    }
                    try
                    {
                        Configuration config = Configuration.FromJson(jsonConfiguration);
                        GeneticEngine engine = new GeneticEngine(config);
                        var population = Enumerable.Range(0, engine.N).Select(_ => new Character(config.CharacterType));
                        var engineThread = new Thread(() =>
                        {
                            metricsBuffer = new BufferBlock<GenerationMetrics>();
                            int gen = 0;
                            var currentPopulation = population;
                            Stopwatch sw = new Stopwatch();
                            sw.Start();
                            while (!engine.Finish.IsFinished(currentPopulation, gen++, sw.ElapsedMilliseconds))
                            {
                                currentPopulation = engine.AdvanceGeneration(currentPopulation).ToList();
                                MeasureGenerationMetrics(currentPopulation);
                            }
                            sw.Stop();
                            metricsBuffer.Post(null);
                            metricsBuffer.Complete();
                        });
                        engineThread.Start();
                        resp.StatusCode = 204;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        byte[] responseData = Encoding.UTF8.GetBytes(ex.ToString());
                        resp.StatusCode = 400;
                        resp.ContentEncoding = Encoding.UTF8;
                        resp.ContentLength64 = responseData.LongLength;
                        await resp.OutputStream.WriteAsync(responseData,0,responseData.Length);
                    }
                }
                else if ((req.HttpMethod == "GET") && (req.Url.AbsolutePath == "/data"))
                {
                    //Get data
                    List<GenerationMetrics> values = new List<GenerationMetrics>();
                    while (metricsBuffer.TryReceive(out GenerationMetrics data))
                    {
                        values.Add(data);
                    }
                    byte[] responseData = Encoding.UTF8.GetBytes(
                            JsonConvert.SerializeObject(values,Formatting.None, new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver()
                            })
                        );
                    resp.ContentType = "application/json";
                    resp.ContentEncoding = Encoding.UTF8;
                    resp.ContentLength64 = responseData.LongLength;
                    resp.StatusCode = 200;
                    await resp.OutputStream.WriteAsync(responseData, 0, responseData.Length);
                }
                resp.Close();
            }
            listener.Close();
        }
    }
}
