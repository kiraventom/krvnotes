using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int swHide = 0;

var handle = GetConsoleWindow();
var thread = new Thread(() =>
{
    Logic.Board.OnStartup();
    var app = new GUI.App();
    app.InitializeComponent();
    app.Run();
});

thread.SetApartmentState(ApartmentState.STA);
thread.Start(); 
ShowWindow(handle, swHide);