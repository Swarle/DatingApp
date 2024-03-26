﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.DAL.Specification.Infrastructure
{
    public class PagingSpecification
    {
        public int Skip { get; }
        public int Take { get; }

        public PagingSpecification(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

    }
}
