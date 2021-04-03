using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
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
            [JsonPropertyName("avgFitness")]
            public double AvgFitness { get; set; }

            [JsonPropertyName("minFitness")]
            public double MinFitness { get; set; }

            [JsonPropertyName("maxFitness")]
            public double MaxFitness { get; set; }
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
            foreach(Character c in generation)
            {
                avgFitness += c.Fitness;
                minFitness = c.Fitness < minFitness ? c.Fitness : minFitness;
                maxFitness = c.Fitness > maxFitness ? c.Fitness : maxFitness;
            }
            avgFitness /= generation.Count();
            metricsBuffer.Post(new GenerationMetrics() { 
                AvgFitness = avgFitness,
                MinFitness = minFitness,
                MaxFitness = maxFitness
            });
        }
        public async Task HandleIncomingConnections(GeneticEngine engine)
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
                    var population = Enumerable.Range(0, engine.N).Select(_ => new Character());
                    var engineThread = new Thread(() =>
                    {
                        metricsBuffer = new BufferBlock<GenerationMetrics>();
                        engine.UntilFinish(population, MeasureGenerationMetrics);
                        metricsBuffer.Post(null);
                        metricsBuffer.Complete();
                    });
                    engineThread.Start();
                    resp.StatusCode = 204;
                }
                else if ((req.HttpMethod == "GET") && (req.Url.AbsolutePath == "/data"))
                {
                    //Get data
                    List<GenerationMetrics> values = new List<GenerationMetrics>();
                    while (metricsBuffer.TryReceive(out GenerationMetrics data))
                    {
                        values.Add(data);
                    }
                    byte[] responseData = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(values);
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
