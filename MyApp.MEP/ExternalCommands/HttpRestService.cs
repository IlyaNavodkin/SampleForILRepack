using System.Windows;

namespace MyApp.MEP.ExternalCommands;

public class HttpRestService
{
    public void PushHttpMessage(string json)
    {
        MessageBox.Show($"Отправил сообщение на сервер: {json}");
    }
}
