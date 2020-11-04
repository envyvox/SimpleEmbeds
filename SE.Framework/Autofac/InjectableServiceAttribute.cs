using System;

namespace SE.Framework.Autofac
{
    public class InjectableServiceAttribute : Attribute
    {
        public bool IsSingletone { get; set; }
    }
}