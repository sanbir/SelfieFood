using System;
using System.Collections.Generic;
using System.Linq;

namespace SelfieFood.SilverlightApp.ViewModels
{
    public class PeopleViewModel
    {
        public List<PersonViewModel> PeopleViewModels { get; set; }

        public string AgeDiapason
        {
            get
            {
                return PeopleViewModels == null
                    ? ""
                    : PeopleViewModels.Count == 0
                        ? ""
                        : PeopleViewModels.Count == 1
                            ? PeopleViewModels[0].Age.ToString()
                            : GetAgeDiapason();
            }
        }

        private string GetAgeDiapason()
        {
            int min = PeopleViewModels.Select(p=>p.Age).Min();
            int max = PeopleViewModels.Select(p => p.Age).Max();
            return string.Format("{0} - {1}", min, max);
        }
    }

    public class PersonViewModel
    {
        public PersonType PersonType { get; set; }

        public int Age { get; set; }
    }

    public enum PersonType
    {
        Infant,
        Child,
        Man,
        BeardedMan,
        Woman,
        OldMan
    }
}
