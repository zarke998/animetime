using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnimeTime.WPF.Utility
{
    public static class CommandUtil
    {
        public static void TryExecute(this ICommand command, object param)
        {
            if(command != null && command.CanExecute(param))
                command.Execute(param);

        }
    }
}
