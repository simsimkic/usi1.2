﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.View.Table;
using ZdravoCorp.ViewModel.Table;
using ZdravoCorp.ViewModel;

namespace ZdravoCorp.Commands
{
    public class ShowStaticEquipmentRedistributionTableCommand : CommandBase
    {
        private readonly DirectorViewModel _directorViewModel;

        public ShowStaticEquipmentRedistributionTableCommand(DirectorViewModel directorViewModel)
        {
            _directorViewModel = directorViewModel;
        }

        public override void Execute(object? parameter)
        {
            var equipmentTableView = new StaticEquipmentRedistributionTableView();
            equipmentTableView.DataContext = new StaticEquipmentRedistributionTableViewModel();
            equipmentTableView.ShowDialog();
        }
    }
}
