using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace Restaurant
{
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
