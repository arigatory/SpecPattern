using CSharpFunctionalExtensions;
using Logic.Utils;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Logic.Movies
{
    public class MovieRepository
    {
        public Maybe<Movie> GetOne(long id)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Get<Movie>(id);
            }
        }

        public IReadOnlyList<Movie> GetList(GenericSpecification<Movie> specification)
        {
            using (ISession session = SessionFactory.OpenSession())
            {
                return session.Query<Movie>()
                    .Where(specification.Expression)
                    .ToList();
            }
        }

        public IQueryable<Movie> Find()
        {
            ISession session = SessionFactory.OpenSession();
            return session.Query<Movie>();

            //in EF core dbcontext.Movies
        }
    }
}
