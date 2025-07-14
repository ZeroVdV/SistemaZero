using System.Windows.Threading;

namespace SistemaZero.Helpers
{
    public class Timers
    {
        private static DispatcherTimer timer;

        // Usarei para impor um limite de tempo de edição do produto, max 2 horas
        public static event EventHandler? TimerFinalizado;

        public static void SetTimerEdit()
        {
            Console.WriteLine("Iniciando temporizador...");

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromHours(2);
            timer.Tick += OnTimedEvent;
            timer.Start();

            Console.WriteLine("Temporizador iniciado!");
        }

        private static void OnTimedEvent(object sender, EventArgs e)
        {
            timer.Stop();
            TimerFinalizado?.Invoke(null, EventArgs.Empty);
        }

        public static void StopTimeEvent()
        {
            if(timer != null)
                timer.Stop();
        }
    }
}