using System.Collections.Generic;
using System.Linq;

namespace Floomeen.Flow
{
    public class SettingsList
    {
        public List<Setting> Sets { get; private set; }

        public SettingsList(List<Setting> sets)
        {
            Sets = sets;
        }

        public SettingsList()
        {
            Sets = new List<Setting>();
        }

        public string StartState => Sets.First(s => s.AsStartElement).Element;
    }
}
