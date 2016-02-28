using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
        public PersonViewModel(Style personStyle, int age)
        {
            PersonStyle = personStyle;
            Age = age;
        }

        public int Age { get; }

        public Style PersonStyle { get; }
    }
}
