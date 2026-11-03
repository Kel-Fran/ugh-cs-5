record struct Student(string Name, byte Age, string Class, float Average) {
    public static List<Student> FromCSV(ReadOnlySpan<char> src, bool hasHeader) {
        var students = new List<Student>(src.Count('\n'));

        var ranges = src.Split('\n');
        if (hasHeader) ranges.MoveNext();

        foreach (var range in ranges) {
            var line = src[range].Trim();

            var parts = line.Split(';');

            parts.MoveNext();
            var name = line[parts.Current].Trim().ToString();

            parts.MoveNext();
            var age = byte.Parse(line[parts.Current]);

            parts.MoveNext();
            var @class = line[parts.Current].ToString();

            parts.MoveNext();
            var avg = float.Parse(line[parts.Current].Trim());

            students.Add(new(name, age, @class, avg));
        }

        return students;

    }

    public static List<Student> FromCSVFile(FileInfo file, bool hasHeader = true) {
        using var s = file.OpenText();
        return FromCSV(s.ReadToEnd(), hasHeader);

    }

}

public static class Program {
    public static void Main(string[] args) {
        var students = Student.FromCSVFile(new("diakok.csv"));

        var ex0 = students.GroupBy(x => x.Class).Select(x => (x.Key, x.Average(x => x.Average))).OrderByDescending(x => x.Item2);
        Console.WriteLine(string.Join('\n', ex0));

        var ex1 = students.GroupBy(x => x.Class).Select(x => (x.Key, x.MaxBy(x => x.Average).Name));
        Console.WriteLine(string.Join('\n', ex1));

        var ex2 = students.AggregateBy(x => x.Age, _ => 0, (acc, _) => acc + 1);
        Console.WriteLine(string.Join('\n', ex2));

        var ex3 = students.GroupBy(x => x.Class).Select(x => (x.Key, x.MaxBy(x => x.Average).Average, x.Where(y => y.Average == x.MaxBy(x => x.Average).Average).Select(x => x.Name)));
        Console.WriteLine(string.Join('\n', ex3));

        var ex4 = students.Where(x => x.Average < 2f);
        Console.WriteLine(string.Join('\n', ex4));
    }

}
