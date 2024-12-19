```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22631.4602/23H2/2023Update/SunValley3) (Hyper-V)
Intel Xeon Platinum 8370C CPU 2.80GHz, 1 CPU, 16 logical and 8 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9282.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9282.0), X64 RyuJIT VectorSize=256


```
| Method                   | Mean              | Error              | StdDev             | Median            | Ratio         | RatioSD       |
|------------------------- |------------------:|-------------------:|-------------------:|------------------:|--------------:|--------------:|
| NoCollector              |          31.91 ns |           0.053 ns |           0.045 ns |          31.90 ns |          1.00 |          0.00 |
| StartTracerProvider      |   4,913,470.31 ns |      31,652.634 ns |      29,607.894 ns |   4,908,724.22 ns |    153,849.22 |        895.57 |
| StartCollectorStandalone | 387,379,210.81 ns | 131,036,027.441 ns | 386,362,683.387 ns | 630,750,087.50 ns | 10,734,070.91 | 12,928,744.81 |
