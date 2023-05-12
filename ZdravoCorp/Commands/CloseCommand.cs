using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZdravoCorp.Commands
{
    public class CloseCommand : CommandBase
    {
        private readonly Window _window;
        public CloseCommand(Window window)
        {
            _window = window;
        }

         

        public override void Execute(object parameter)
        {
            _window.Close();
        }


    }
}
