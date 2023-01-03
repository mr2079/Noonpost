using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Generator;

    public static class NameGenerator
    {
        public static string Generate() => Guid.NewGuid().ToString().Replace("-", "");
    }
