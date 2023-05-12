using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Model;
using ZdravoCorp.Commands;

namespace ZdravoCorp.ViewModel
{
    public class DirectorViewModel:ViewModelBase
    {
		private readonly Director _director;


        public Director Director => _director;

        public ICommand ShowStaticEquipmentQuantityTable { get; }
        public ICommand ShowDynamicalEquipmentTable { get; }

        public ICommand ShowDynamicalEquipmentRedistributionTable { get; }
        public ICommand ShowStaticEquipmentRedistributionTable { get; }

        public DirectorViewModel(Director director)
        {
            _director = director;
            ShowStaticEquipmentQuantityTable = new ShowStaticEquipmentQuantityTableCommand(this);
            ShowDynamicalEquipmentTable = new ShowDynamicalEquipmentTableCommand(this);
            ShowDynamicalEquipmentRedistributionTable = new ShowDynamicalEquipmentRedistributionTableCommand(this);
            ShowStaticEquipmentRedistributionTable = new ShowStaticEquipmentRedistributionTableCommand(this);
        }




    }
}
