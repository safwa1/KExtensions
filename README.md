# KExtensions

A small, pragmatic collection of high‑quality C# extension methods and tiny utilities for everyday .NET apps and libraries. Focused on ergonomics, readability, and reducing boilerplate.

- Targets: .NET 8, .NET 9, .NET 10
- Package Id: `KExtensions`
- License: MIT

<!-- Badges (adjust once published to NuGet) -->
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
![Target Frameworks](https://img.shields.io/badge/TFMs-net8.0%20%7C%20net9.0%20%7C%20net10.0-blue)

## Table of contents
- [Install](#install)
- [Quick start](#quick-start)
- [What you get](#what-you-get)
  - [Strings](#strings)
  - [Collections & arrays](#collections--arrays)
  - [Numbers & percentages](#numbers--percentages)
  - [Parsing numbers from strings](#parsing-numbers-from-strings)
  - [Date & time (string to DateTime)](#date--time-string-to-datetime)
  - [Spans](#spans)
  - [Tasks](#tasks)
  - [Paths & files](#paths--files)
  - [Console output (NET 10+)](#console-output-net-10)
  - [Kotlin-style scope functions](#kotlin-style-scope-functions)
  - [Boolean helpers](#boolean-helpers)
  - [DataTable helpers](#datatable-helpers)
  - [Speed & Size formatting](#speed--size-formatting)
  - [DateTime extras](#datetime-extras)
- [Supported frameworks](#supported-frameworks)
- [Design goals](#design-goals)
- [FAQ](#faq)
- [Roadmap (ideas)](#roadmap-ideas)
- [Contributing](#contributing)
- [License](#license)


## Install

Using the .NET CLI:

```bash
dotnet add package KExtensions
```

Using Package Manager Console:

```powershell
Install-Package KExtensions
```


## Quick start

```csharp
using KExtensions;

// Strings
var title = "hello world".ToTitleCase();        // "Hello World"
var kebab = "HelloWorldXML".ToKebabCase();      // "hello-world-xml"
var part  = "id=42&lang=en".SubstringAfter("id="); // "42&lang=en"

// Collections & arrays
var last3 = new[] { 1, 2, 3, 4, 5 }.TakeLast(3); // Span<int> { 3,4,5 }
var first2 = new[] { 1, 2, 3 }.Take(2);          // int[] { 1, 2 }

// Numbers
int flag = true.ToInt();       // 1
bool b = 1.ToBoolean();        // true
var inc = 10.Increment();      // 11
var pct = 200.CalculatePercentage(10); // 20 (10%)

// Date & time (parsing helpers)
var d1 = "05/11/2025".ParseDate();                    // dd/MM/yyyy by default
var t1 = "13:30:00".ParseTime();                      // HH:mm:ss by default
var dt = "25/12/2025 23:59:59".ParseDateTime();

// Tasks
Task.Run(async () => { /* ... */ }).Forget();         // fire-and-forget safely

// Console table (NET 10+)
#if NET10_0_OR_GREATER
Console.Table(new[] { new { Id = 1, Name = "A" }, new { Id = 2, Name = "B" } });
#endif
```


## What you get

KExtensions groups extensions by domain so you can discover functionality quickly. Below are the most commonly used areas with practical examples.

### Strings
Rich helpers for empty/blank checks, casing, slicing, partitions, simple templating, and byte conversions. Highlights:

- Emptiness & blankness
  ```csharp
  "".IsEmpty();                 // true
  ((string?)null).IsNullOrEmpty();   // true
  "  ".IsBlank();               // true (only whitespace)
  "text".IsNotNullOrBlank();    // true
  "foo".Or("alt");             // "foo"
  ((string?)null).OrEmpty();     // ""
  ```

- Slicing & spans
  ```csharp
  "abcdef".Take(2);            // "ab"
  "abcdef".Drop(2);            // "cdef"
  "abcdef".DropLast(2);        // "abcd"
  "abcdef".TakeLast(3);        // "def"
  var span = "abcdef".DropSpan(2);     // ReadOnlySpan<char> "cdef"
  ```

- Classification & cleanup
  ```csharp
  "123abc".HasDigits();         // true
  "123abc".HasLetters();        // true
  "a_b-cD".ToKebabCase();       // "a-b-cd"
  "aB".ToCamelCase();           // "aB" (safe camelization)
  "abc!@#".RemoveNonAlphanumeric(); // "abc"
  ```

- Similarity & counts
  ```csharp
  "kitten".StringMatchPercentage("sitting"); // 0..100 double
  "one two  three".WordCount();               // 3
  ```

- Parts before/after
  ```csharp
  "path/to/file.txt".SubstringBefore("/");      // "path"
  "path/to/file.txt".SubstringAfterLast("/");  // "file.txt"
  ```

- Lightweight placeholders/interpolation
  ```csharp
  var s = "Hello, {name}!".Interpolate(new { name = "Safwan" });
  // => "Hello, Safwan!"

  var f = "Speed is %speed%".FormatArgs(new { speed = 42 });
  // => "Speed is 42"
  ```

- Bytes & base64
  ```csharp
  var bytes = "hello".ToByteArray();
  var raw   = "aGVsbG8=".Base64StringToByteArray();
  ```


### Collections & arrays
Ergonomic taking, dropping, batching, reversing, and simple for‑each helpers. Works with `T[]`, `Span<T>`, `IEnumerable<T>`.

```csharp
var first = new[] {1,2,3,4}.Take(2);           // int[] {1,2}
var last  = new[] {1,2,3,4}.TakeLast(2);       // Span<int> {3,4}
var rest  = new[] {1,2,3,4}.Drop(2);           // Span<int> {3,4}
var skip2 = Enumerable.Range(1,5).Drop(2);     // IEnumerable<int> {3,4,5}
var rev   = new[] {1,2,3}.Reverse();           // IEnumerable<int> {3,2,1}
var batches = Enumerable.Range(1,5).Batch(2);  // [[1,2],[3,4],[5]]

new[] {"a","b"}.ForEach(x => Console.WriteLine(x));
var dt = new[] { new { Id=1, Name="A" } }.ToDataTable();
```

`JoinExtensions.JoinToString` builds readable joined strings with prefix/postfix/limit:

```csharp
var s = new[]{1,2,3}.JoinToString(
    separator: ", ", prefix: "[", postfix: "]", limit: -1,
    transform: x => (x * 10).ToString());
// => "[10, 20, 30]"
```


### Numbers & percentages
Generic number helpers using `INumber<T>` plus practical conversions:

```csharp
1.ToBoolean();        // true
false.ToInt();        // 0
5.Increment();        // 6
10.Decrement(3);      // 7
(-4).ToPositive();    // 4
4.ToNegative();       // -4
200.CalculatePercentage(10); // 20 (10%)
200.CalculatePercentage(0.25m); // 50 (25%)
```


### Parsing numbers from strings
Convenience methods that consistently use invariant culture:

```csharp
"$1,234.56".ParseCurrency();        // 1234.56m (strict)
"USD 1,234.56".ParseCurrencyLoose(); // 1234.56m (tolerant)
"3,14".ParseDecimalInvariant();     // 3.14m
"42".ToInt();                       // 42
"3.5".ToDouble();                   // 3.5
```


### Date & time (string to DateTime)
Simple, explicit parsing helpers with formats:

```csharp
"05/11/2025".ParseDate();                  // dd/MM/yyyy
"13:45:00".ParseTime();                    // HH:mm:ss
"25/12/2025 23:59:59".ParseDateTime();     // dd/MM/yyyy HH:mm:ss
"2025-10-25".ParseExactInvariant("yyyy-MM-dd");
```


### Spans
Efficient concatenation helpers via `SpanUtils`:

```csharp
ReadOnlySpan<byte> a = stackalloc byte[] {1,2};
ReadOnlySpan<byte> b = stackalloc byte[] {3,4};
var joined = a.Concat(b); // byte[] {1,2,3,4}
```


### Tasks
Fire‑and‑forget helpers and task result/exception split blocks:

```csharp
someTask.Forget();
await someTask.SafeFireAndForget();

var (resultBlock, exceptionBlock) = someTaskReturningT.SplitIntoBlocks();
```


### Paths & files
Utilities for JSON‑safe path literals and validation:

```csharp
Environment.SpecialFolder.MyDocuments.ItPath();
"C:/temp/file.txt".SerializePathForJson();

if (!"report?.txt".IsValidAsFileName(out var err))
{
    Console.WriteLine(err.GetMessage()); 
}

"C:/windows".PathExists();
```

`Types/DirectoryPath` is a small span‑friendly struct for directory operations:

```csharp
using KExtensions.Types;

DirectoryPath root = new DirectoryPath("C:/data");
var sub = root.Combine("logs");
if (sub.Exists())
{
    foreach (var f in sub.GetFiles("*.log")) { /* ... */ }
}
```


### Console output (NET 10+)
Pretty‑print tables to the console without bringing heavy dependencies:

```csharp
#if NET10_0_OR_GREATER
Console.Table(new [] { new { Id = 1, Name = "Alpha" }, new { Id = 2, Name = "Beta" } });
// +----+-------+
// | Id | Name  |
// +----+-------+
// | 1  | Alpha |
// | 2  | Beta  |
// +----+-------+
#endif
```


### Kotlin-style scope functions
Lightweight, Kotlin-inspired helpers from `AnyExtensions` to make object configuration and flow nicer:

```csharp
var person = new Person()
    .Apply(p => { p.Name = "Safwan"; p.Age = 30; })
    .Also(p => Console.WriteLine($"Created: {p.Name}"));

int len = "hello".Run(s => s.Length); // 5

string? nonNull = "value".TakeIf(v => v.Length > 0);   // "value"
string? nullVal = "".TakeUnless(v => v.Length > 0);     // null
```

- `Run(obj => ...)` returns the lambda result.
- `Let(x => ...)` same semantics as `Run` but for stylistic preference.
- `Apply(x => { ... })` returns the original object (for configuration chaining).
- `Also(x => { side effects })` returns the original object.
- `TakeIf(predicate) / TakeUnless(predicate)` return the object or `null`.


### Boolean helpers
Small helpers to reduce `if` boilerplate and to filter reference types:

```csharp
true.IfTrue(() => DoWork());
false.IfFalse(() => Log("was false"));

var kept = someRef.TakeIf(x => x != null); // returns T? based on predicate
```

Definitions live in `BoolExtensions`.


### DataTable helpers
Utilities when you need to interop with `DataTable`/`DataRow`:

```csharp
// Binary search on a sorted column
int idx = table.BinarySearch(columnName: "Id", value: 42);

// Clone a DataRow to a new detached row with the same schema
DataRow copy = row.Clone();
```

Notes:
- `BinarySearch` assumes the target column is already sorted in ascending order by `Comparer<object>.Default`.
- `Clone` copies the `ItemArray` and returns a new row from the same table schema.


### Speed & Size formatting
Human-readable formatting for throughput and sizes:

```csharp
123_456L.SpeedFormat();           // "120.56 KB/s"
123_456_789L.SpeedFormat(true);   // "117.74 MB/s"

10_485_760L.SizeFormat(inMB: true);  // "10.00 MB"
10_240L.SizeFormat(inMB: false);     // "10.00 KB"
```

See `SpeedAndSizeExtensions`.


### DateTime extras
Additional helpers beyond parsing:

```csharp
var dates = new DateTime(2025, 1, 1).GetDatesArrayTo(new DateTime(2025, 1, 05));
// => [2025-01-01, 2025-01-02, 2025-01-03, 2025-01-04]

var hijri = DateTime.Now.ToHijriDate(); // e.g., "1447/05/20"
```

Note: `ToHijriDate` uses `System.Globalization.HijriCalendar`.

## Supported frameworks
- net8.0
- net9.0
- net10.0

The library uses modern C# features (LangVersion: preview). Some APIs are conditionally available on newer TFMs (for example: console `Table<T>` on .NET 10+).


## Design goals
- Small and focused surface area
- Predictable behavior and naming
- Invariant culture for numeric/date parsing helpers
- Allocation‑aware implementations where it matters (spans, stackalloc, batching)


## FAQ
- Why extension methods? Because they compose well, keep call sites readable, and are easy to adopt incrementally.
- Are there breaking changes? We follow SemVer during preview; breaking changes will bump the minor/major version appropriately.
- Can I use parts of it only? Yes. It’s a single assembly; just call the extensions you need.


## Roadmap (ideas)
- More span‑based string utilities
- Additional parsing helpers with Try‑ versions
- Extra LINQ‑like utilities for async streams
- Benchmarks and performance docs


## Contributing
Issues and PRs are welcome. Please try to keep additions small and focused. If you’re unsure, open an issue to discuss the idea first.


## License
MIT © Safwan Abdulghani
