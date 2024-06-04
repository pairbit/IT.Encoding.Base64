using IT.Encoding.Base64.Benchmarks;

new Base64UrlBenchmark().Test();
BenchmarkDotNet.Running.BenchmarkRunner.Run<Base64UrlBenchmark>();

Console.WriteLine("End....");