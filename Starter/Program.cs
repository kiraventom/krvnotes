using System.Runtime.InteropServices;
using GUI;
using Logic;
using Logic.Dumping;
using Starter;

[DllImport("kernel32.dll")]
static extern IntPtr GetConsoleWindow();

[DllImport("user32.dll")]
static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

const int swHide = 0;

var handle = GetConsoleWindow();
var thread = new Thread(() =>
{
    var model = new Model();
    var eventManager = new EventManager(model);

    var app = App.Create(eventManager);
    app.InitializeComponent();
    app.Run();
});

thread.SetApartmentState(ApartmentState.STA);
thread.Start(); 
ShowWindow(handle, swHide);