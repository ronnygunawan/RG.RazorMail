```

BenchmarkDotNet v0.15.6, Linux Ubuntu 24.04.3 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.100
  [Host]     : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
  Job-VLVDMF : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3

IterationCount=10  LaunchCount=1  WarmupCount=1  

```
| Method                           | Mean       | Error     | StdDev    | Rank | Gen0   | Gen1   | Allocated |
|--------------------------------- |-----------:|----------:|----------:|-----:|-------:|-------:|----------:|
| &#39;Render .cshtml View&#39;            |  26.301 μs | 0.2010 μs | 0.1330 μs |    2 | 0.4272 |      - |   7.01 KB |
| &#39;Render .razor Component&#39;        |   2.265 μs | 0.0945 μs | 0.0562 μs |    1 | 0.0877 | 0.0420 |   1.45 KB |
| &#39;Render+Inline .cshtml View&#39;     | 119.993 μs | 9.7699 μs | 5.1098 μs |    4 | 3.9063 |      - |  65.33 KB |
| &#39;Render+Inline .razor Component&#39; |  54.033 μs | 0.8433 μs | 0.4411 μs |    3 | 3.1738 | 0.7324 |  55.04 KB |
