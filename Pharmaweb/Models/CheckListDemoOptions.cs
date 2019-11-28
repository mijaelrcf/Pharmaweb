using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Pharmaweb.Models
{
    public class CheckListDemoOptions
    {
        public const RepeatLayout DefaultRepeatLayout = RepeatLayout.Table;
        public const RepeatDirection DefaultRepeatDirection = RepeatDirection.Horizontal;
        public const int DefaultRepeatColumns = 5;

        public CheckListDemoOptions()
        {
            RepeatLayout = DefaultRepeatLayout;
            RepeatDirection = DefaultRepeatDirection;
            RepeatColumns = DefaultRepeatColumns;
        }

        public RepeatLayout RepeatLayout { get; set; }
        public RepeatDirection RepeatDirection { get; set; }
        public int RepeatColumns { get; set; }
    }
}