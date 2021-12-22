using Model;

var controller = new Controller();
controller.Board.Add("Header 0", "Text text text");
controller.Board.Add("Longer header", "Text text text text");

while (true)
{
    // list all notes
    Console.WriteLine($"Notes ({controller.Board.Notes.Count}):");
    foreach (var (_, note) in controller.Board.Notes)
    {
        string date = note.CreatedAt.ToString("dd-MM-yy HH:mm:ss");
        string header = note.Header;
        string format = $"{{0,-{date.Length + 3}}} {{1, -{header.Length}}}"; // fix this or maybe not
        Console.WriteLine(format, date, header);
    }

    Console.ReadLine();
    Console.Clear();
}