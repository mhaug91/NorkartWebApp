﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(norkartSommerWebApp.Startup))]
namespace norkartSommerWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
