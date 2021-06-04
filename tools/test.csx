Console.WriteLine(AppContext.BaseDirectory);
Console.WriteLine(new DirectoryInfo(AppContext.BaseDirectory).Parent.FullName);