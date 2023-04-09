using FluentNHibernate.Mapping;
using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Movies
{
    public class Director : Entity
    {
        public virtual string Name { get; }
    }

    public class DirectorMap : ClassMap<Director>
    {
        public DirectorMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
