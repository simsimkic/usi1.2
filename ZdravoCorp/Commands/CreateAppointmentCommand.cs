using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Commands
{
    public class CreateAppointmentCommand : CommandBase
    {

        public override void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}
