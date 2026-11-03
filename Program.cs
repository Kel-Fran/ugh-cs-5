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
    }

}
