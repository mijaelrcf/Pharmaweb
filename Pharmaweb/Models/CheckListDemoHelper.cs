using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Pharmaweb.Models
{
    public class CheckListDemoHelper
    {
        public static List<SelectListItem> GetRepeatLayouts()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = RepeatLayout.Table.ToString(), Value = RepeatLayout.Table.ToString(), Selected = true },
                new SelectListItem() { Text = RepeatLayout.Flow.ToString(), Value = RepeatLayout.Flow.ToString() }
            };
        }
        public static List<SelectListItem> GetRepeatDirections()
        {
            return new List<SelectListItem>() {
                new SelectListItem() { Text = RepeatDirection.Horizontal.ToString(), Value = RepeatDirection.Horizontal.ToString(), Selected = true },
                new SelectListItem() { Text = RepeatDirection.Vertical.ToString(), Value = RepeatDirection.Vertical.ToString() }
            };
        }
        public static List<SelectListItem> GetRepeatColumns()
        {
            return new List<SelectListItem>(){
                new SelectListItem(){ Text = "1", Value = "1" },
                new SelectListItem(){ Text = "2", Value = "2" },
                new SelectListItem(){ Text = "3", Value = "3" },
                new SelectListItem(){ Text = "4", Value = "4" },
                new SelectListItem(){ Text = "5", Value = "5", Selected = true},
                new SelectListItem(){ Text = "6", Value = "6" }
            };
        }
    }
}